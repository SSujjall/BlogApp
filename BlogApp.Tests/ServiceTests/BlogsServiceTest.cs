using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.CloudinaryService.Service;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Redis_Cache.Service;
using BlogApp.Infrastructure.Services;
using Moq;
using System.Net;

namespace BlogApp.Tests.ServiceTests;

[TestFixture]
public class BlogsServiceTest
{
    private Mock<IBlogRepository> _mockBlogRepo;
    private Mock<ICloudinaryService> _mockCloudinary;
    private Mock<IBaseRepository<BlogHistory>> _mockBlogHistoryRepo;
    private Mock<IMapper> _mockMapper;
    private Mock<IUserRepository> _mockUserRepo;
    private Mock<IRedisCache> _mockRedisCache;

    private BlogService _blogService;

    [SetUp]
    public void Setup()
    {
        _mockBlogRepo = new Mock<IBlogRepository>();
        _mockCloudinary = new Mock<ICloudinaryService>();
        _mockBlogHistoryRepo = new Mock<IBaseRepository<BlogHistory>>();
        _mockMapper = new Mock<IMapper>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockRedisCache = new Mock<IRedisCache>();

        _blogService = new BlogService(
            _mockBlogRepo.Object,
            _mockCloudinary.Object,
            _mockBlogHistoryRepo.Object,
            _mockMapper.Object,
            _mockUserRepo.Object,
            _mockRedisCache.Object
        );
    }

    [Test]
    public async Task GetAllBlogs_ReturnsMappedDTOs_WhenRepositoryReturnsData()
    {
        // Arrange
        var blogs = new List<Blogs>
        {
            new Blogs
            {
                BlogId = 1,
                Title = "Test Blog",
                Description = "Description",
                UserId = "user1",
                User = new Users { UserName = "John" },
                ImageUrl = "image.jpg",
                UpVoteCount = 5,
                DownVoteCount = 1,
                CommentCount = 3
            }
        };

        var totalCount = 1;

        var getBlogsResult = new GetFilteredBlogsDTO
        {
            Blogs = blogs,
            Count = totalCount
        };

        _mockBlogRepo
           .Setup(r => r.GetFilteredBlogs(It.IsAny<GetRequest<Blogs>>()))
           .ReturnsAsync(getBlogsResult);

        var result = await _blogService.GetAllBlogs(new GetRequest<Blogs>(), new Domain.Shared.CacheRequestItems { });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Data.Count(), Is.EqualTo(1));
            Assert.That(result.Data.First().Title, Is.EqualTo("Test Blog"));
        });
    }

    [Test]
    public async Task GetAllBlogs_ReturnsFailedResponse_WhenRepositoryReturnsNull()
    {
        // Arrange
        _mockBlogRepo
            .Setup(r => r.GetFilteredBlogs(It.IsAny<GetRequest<Blogs>>()))
            .ReturnsAsync(new Application.DTOs.GetFilteredBlogsDTO());

        // Act
        var result = await _blogService.GetAllBlogs(new GetRequest<Blogs>(), new Domain.Shared.CacheRequestItems { });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(result.Data, Is.Null);
        });
    }
}
