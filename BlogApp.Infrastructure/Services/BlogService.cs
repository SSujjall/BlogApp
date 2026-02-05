using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.AppHelpers;
using BlogApp.Application.Helpers.CloudinaryService.Service;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using BlogApp.Infrastructure.Redis_Cache.Service;
using System.Net;

namespace BlogApp.Infrastructure.Services
{
    public class BlogService(
        IBlogRepository _blogRepository,
        ICloudinaryService _cloudinary,
        IBaseRepository<BlogHistory> _blogHistoryRepo,
        IMapper _mapper,
        IUserRepository _userRepository,
        IRedisCache _redisCache,
        ITransactionService _tranService
    ) : IBlogService
    {
        public async Task<ApiResponse<IEnumerable<BlogsDTO>>> GetAllBlogs(GetRequest<Blogs> request, CacheRequestItems requestForCache)
        {
            // [DEV] checking if the key is being deleted or not
            //var y = _redisCache.GetAllKeys();
            //await _redisCache.DeleteKeysByPrefix(CacheKeys.GetAllBlogsPrefix);
            //var y2 = _redisCache.GetAllKeys();

            var cacheKey = RedisCacheHelper.GenerateCacheKey(CacheKeys.GetAllBlogsNameForHelper, requestForCache);
            var result = await _redisCache.GetOrCreateCache(
                cacheKey,
                async () => await _blogRepository.GetFilteredBlogs(request),
                TimeSpan.FromDays(1)
            );

            //var result = await _blogRepository.GetFilteredBlogs(request);
            if (result.Blogs != null)
            {
                #region response model mapping
                var response = new List<BlogsDTO>();

                foreach (var item in result.Blogs)
                {
                    var blog = new BlogsDTO
                    {
                        BlogId = item.BlogId,
                        User = new BlogUser
                        {
                            UserId = item.UserId,
                            Name = item.User.UserName ?? ""
                        },
                        Title = item.Title,
                        Description = item.Description,
                        ImageUrl = item.ImageUrl,
                        UpVoteCount = item.UpVoteCount,
                        DownVoteCount = item.DownVoteCount,
                        CommentCount = item.CommentCount
                    };
                    response.Add(blog);
                }
                #endregion
                return ApiResponse<IEnumerable<BlogsDTO>>.Success(response, "All Blogs Listed", HttpStatusCode.OK, result.Count);
            }
            return ApiResponse<IEnumerable<BlogsDTO>>.Failed(null, "Failed to load data");
        }

        public async Task<ApiResponse<BlogsDTO>> GetBlogById(int id)
        {
            var cacheKey = RedisCacheHelper.GenerateCacheKey("GetBlogById", new CacheRequestItems { Id = id.ToString() });
            var result = await _redisCache.GetOrCreateCache(
                cacheKey,
                async () => await _blogRepository.GetByIdAsync(id),
                TimeSpan.FromDays(1)
            );
            //var result = await _blogRepository.GetById(id);
            if (result != null && result.IsDeleted == false)
            {
                #region response model mapping
                var response = new BlogsDTO
                {
                    BlogId = result.BlogId,
                    User = new BlogUser
                    {
                        UserId = result.UserId,
                        Name = result.User.UserName ?? ""
                    },
                    Title = result.Title,
                    Description = result.Description,
                    ImageUrl = result.ImageUrl,
                    UpVoteCount = result.UpVoteCount,
                    DownVoteCount = result.DownVoteCount,
                    CommentCount = result.CommentCount
                };
                #endregion
                return ApiResponse<BlogsDTO>.Success(response, $"Blog id: {result.BlogId}");
            }
            return ApiResponse<BlogsDTO>.Failed(null, "No Blog Found.");
        }

        public async Task<ApiResponse<IEnumerable<BlogsDTO>>> GetBlogsByUserId(string userId)
        {
            var reqFilter = new GetRequest<Blogs>()
            {
                Filter = (x => x.UserId == userId && x.IsDeleted == false)
            };
            var result = await _blogRepository.GetAllAsync(reqFilter);
            if (result.Any())
            {
                // mapping response model
                var response = _mapper.Map<IEnumerable<BlogsDTO>>(result);
                return ApiResponse<IEnumerable<BlogsDTO>>.Success(response, "Blogs Fetched for User");
            }

            var errors = new Dictionary<string, string>
            {
                {"Blogs", "No Blogs Available For User"}
            };
            return ApiResponse<IEnumerable<BlogsDTO>>.Failed(errors, "Blogs Not Found", HttpStatusCode.NoContent);
        }

        public async Task<ApiResponse<BlogsDTO>> CreateBlog(string userId, CreateBlogDTO dto)
        {
            string? uploadedImageUrl = null;

            try
            {
                #region Uploading Image
                if (dto.ImageUrl != null)
                {
                    uploadedImageUrl = await _cloudinary.UploadImage(dto.ImageUrl);
                    if (string.IsNullOrEmpty(uploadedImageUrl))
                    {
                        var imgError = new Dictionary<string, string>
                        {
                            { "ImageUploadFailed", "Image upload faiiled when creating blog" }
                        };
                        throw new ServiceException(imgError, HttpStatusCode.ServiceUnavailable);
                    }
                }
                #endregion

                #region request model mapping
                var request = new Blogs
                {
                    UserId = userId,
                    Title = dto.Title,
                    Description = dto.Description,
                    ImageUrl = uploadedImageUrl
                };
                #endregion
                var result = await _blogRepository.AddAsync(request);
                if (result == null)
                {
                    var errors = new Dictionary<string, string>
                    {
                        { "BlogCreationFailed", "Error occured when creating new blog" }
                    };
                    throw new ServiceException(errors, HttpStatusCode.ServiceUnavailable);
                }

                var userDetail = await _userRepository.GetByIdAsync(result.UserId); // create a separate method in repo for just getting username and userId using 'result.UserId' instead of fetching everything.
                #region response model mapping
                var response = new BlogsDTO
                {
                    BlogId = result.BlogId,
                    User = new BlogUser
                    {
                        UserId = result.UserId,
                        Name = userDetail.UserName ?? ""
                    },
                    Title = result.Title,
                    Description = result.Description,
                    ImageUrl = result.ImageUrl,
                    UpVoteCount = result.UpVoteCount,
                    DownVoteCount = result.DownVoteCount,
                    CommentCount = result.CommentCount
                };
                #endregion

                // Invalidate GetAllBlogs key
                await _redisCache.DeleteKeysByPrefix(CacheKeys.GetAllBlogsPrefix);

                return ApiResponse<BlogsDTO>.Success(response, "Blog created successfully.");
            }
            catch
            {
                if (!string.IsNullOrEmpty(uploadedImageUrl))
                {
                    await _cloudinary.DeleteImage(uploadedImageUrl);
                }

                throw; // throws the service exception for cloudinary if error occurs when uploading image
            }
        }

        public async Task<ApiResponse<BlogsDTO>> UpdateBlog(UpdateBlogDTO dto, string userId)
        {
            var existingBlog = await _blogRepository.GetByIdAsync(dto.BlogId);
            if (existingBlog == null)
            {
                var errors = new Dictionary<string, string>() { { "Blog", "Blog Not Found." } };
                return ApiResponse<BlogsDTO>.Failed(errors, "Blog Update Failed");
            }

            if (existingBlog.UserId != userId)
            {
                var authError = new Dictionary<string, string>() { { "Authorization", "You are not authorized to update this blog." } };
                return ApiResponse<BlogsDTO>.Failed(authError, "Unauthorized blog update attempt.", HttpStatusCode.Unauthorized);
            }

            string? newImageUrl = null;
            string? oldImageUrl = existingBlog.ImageUrl;

            try
            {
                #region Upload Image (Upload validation is in UploadImage method itself)
                // If the new image is uploaded check if there is image in old blog and delete it.
                if (dto.ImageUrl != null)
                {
                    newImageUrl = await _cloudinary.UploadImage(dto.ImageUrl);

                    if (string.IsNullOrEmpty(newImageUrl))
                    {
                        var imgUploadError = new Dictionary<string, string>
                    {
                        { "ImageUploadFailed", "Failed to upload new image" }
                    };

                        throw new ServiceException(imgUploadError, HttpStatusCode.ServiceUnavailable);
                    }
                }
                #endregion

                // Transaction ensure either both adding to blog history and updating blog passes or both fails.
                var result = await _tranService.ExecuteInTransactionAsync<Blogs>(async () =>
                {
                    #region Add to Blog History
                    var historyReq = _mapper.Map<BlogHistory>(existingBlog);
                    historyReq.CreatedAt = DateTime.Now;
                    var historyRes = await _blogHistoryRepo.AddAsync(historyReq);
                    if (historyRes == null)
                    {
                        var error = new Dictionary<string, string>() { { "Blog History", "Add Blog History response returned null." } };
                        throw new ServiceException(error, HttpStatusCode.Conflict);
                    }
                    #endregion

                    #region Update Blog
                    existingBlog.Title = !string.IsNullOrEmpty(dto.Title) ? dto.Title : existingBlog.Title;
                    existingBlog.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : existingBlog.Description;
                    existingBlog.ImageUrl = newImageUrl ?? existingBlog.ImageUrl;
                    existingBlog.UpdatedAt = DateTime.Now;
                    #endregion

                    await _blogRepository.Update(existingBlog);
                    return existingBlog;                    
                });

                #region Cache invalidation after transaction success
                var cacheKey = RedisCacheHelper.GenerateCacheKey("GetBlogById", new CacheRequestItems { Id = dto.BlogId.ToString() });
                await _redisCache.InvalidateKey(cacheKey);

                // Invalidate GetAllBlogs key
                await _redisCache.DeleteKeysByPrefix(CacheKeys.GetAllBlogsPrefix);
                #endregion

                // Delete OLD image AFTER SUCCESS
                if (newImageUrl != null && !string.IsNullOrEmpty(oldImageUrl))
                {
                    await _cloudinary.DeleteImage(oldImageUrl);
                }

                #region response model mapping
                var response = new BlogsDTO
                {
                    BlogId = result.BlogId,
                    User = new BlogUser
                    {
                        UserId = result.UserId,
                        Name = result.User.UserName ?? ""
                    },
                    Title = result.Title,
                    Description = result.Description,
                    ImageUrl = result.ImageUrl,
                    UpVoteCount = result.UpVoteCount,
                    DownVoteCount = result.DownVoteCount,
                    CommentCount = result.CommentCount
                };
                #endregion
                return ApiResponse<BlogsDTO>.Success(response, "Blog Update Successful");
            }
            catch
            {
                // COMPENSATION — delete NEW image if DB transaction failed
                if (!string.IsNullOrEmpty(newImageUrl))
                {
                    await _cloudinary.DeleteImage(newImageUrl);
                }

                throw;
            }
        }

        public async Task<ApiResponse<string>> DeleteBlog(int id, string userId)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog != null)
            {
                if (blog.UserId != userId)
                {
                    var authError = new Dictionary<string, string>() { { "Authorization", "You are not authorized to delete this blog." } };
                    return ApiResponse<string>.Failed(authError, "Unauthorized blog deletion attempt.");
                }
                try
                {
                    // Checking if the blog is already deleted or not
                    if (blog.IsDeleted == true)
                    {
                        var blogError = new Dictionary<string, string>() { { "Blog", "Blog not found." } };
                        return ApiResponse<string>.Failed(blogError, "Blog Deletion Failed");
                    }
                    blog.IsDeleted = true;
                    await _blogRepository.Update(blog); // Softdelete
                    await _blogRepository.SaveChangesAsync();
                    
                    // Invalidate Cache after delete
                    await _redisCache.DeleteKeysByPrefix(CacheKeys.GetAllBlogsPrefix);
                    // TODO: implement cache for blogById and UserSpecificBlog and when deleting, invalidate them too.
                    
                    return ApiResponse<string>.Success(null, "Blog Deleted Successfully");
                }
                catch (Exception ex)
                {
                    var exceptionErrors = new Dictionary<string, string>() { { "Exception", ex.Message } };
                    return ApiResponse<string>.Failed(exceptionErrors, "Blog Deletion Failed Due to an Error");
                }
            }
            var errors = new Dictionary<string, string>() { { "Blog", "Blog Not Found." } };
            return ApiResponse<string>.Failed(errors, "Blog Deletion Failed");
        }

        public async Task<bool> UpdateBlogVoteCount(AddOrUpdateBlogReactionDTO model, bool reactionExists, VoteType? previousVote)
        {
            var blog = await _blogRepository.GetByIdAsync(model.BlogId);
            if (blog == null)
            {
                return false;
            }

            if (!reactionExists) // New reaction
            {
                if (model.ReactionType == VoteType.UpVote)
                    blog.UpVoteCount++;
                else if (model.ReactionType == VoteType.DownVote)
                    blog.DownVoteCount++;
            }
            else // Reaction is being updated
            {
                // Remove previous vote
                if (previousVote == VoteType.UpVote)
                    blog.UpVoteCount--;
                else if (previousVote == VoteType.DownVote)
                    blog.DownVoteCount--;

                // Apply new vote
                if (model.ReactionType == VoteType.UpVote)
                    blog.UpVoteCount++;
                else if (model.ReactionType == VoteType.DownVote)
                    blog.DownVoteCount++;
            }

            // Ensure no negative votes
            blog.UpVoteCount = Math.Max(0, blog.UpVoteCount);
            blog.DownVoteCount = Math.Max(0, blog.DownVoteCount);

            // Update blog and save changes
            await _blogRepository.Update(blog);
            await _blogRepository.SaveChangesAsync();

            #region Invalidate Cache
            // Update the blog and invalidate the cache
            var cacheKey = RedisCacheHelper.GenerateCacheKey("GetBlogById", new CacheRequestItems { Id = model.BlogId.ToString() });
            await _redisCache.InvalidateKey(cacheKey);

            // Also invalidate the GetAllBlogs cache since vote counts have changed
            await _redisCache.DeleteKeysByPrefix(CacheKeys.GetAllBlogsPrefix);

            // This will match any key containing the specific filter
            //await DeleteKeysByPatternAsync("*Filter:asd*");

            // This will match any key containing "Filter:" segment
            //await DeleteKeysByPatternAsync("*Filter:*");

            // This will match keys with specific pagination
            //await DeleteKeysByPatternAsync("*Skip:0-Take:10*");
            #endregion

            return true;
        }

        public async Task UpdateBlogCommentCount(int blogId, bool isAdding)
        {
            var blog = await _blogRepository.GetByIdAsync(blogId);
            if (blog != null)
            {
                if (isAdding)
                    blog.CommentCount++;
                else
                    blog.CommentCount = Math.Max(0, blog.CommentCount - 1); // Prevent negative counts

                try
                {
                    var updatedBlog = await _blogRepository.Update(blog);
                    if (updatedBlog == null)
                    {
                        var errors = new Dictionary<string, string>
                        {
                            { "UpdateBlogFailed", "Error occured when updating blog" }
                        };
                        throw new ServiceException(errors, HttpStatusCode.InternalServerError);
                    }
                    await _blogRepository.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
