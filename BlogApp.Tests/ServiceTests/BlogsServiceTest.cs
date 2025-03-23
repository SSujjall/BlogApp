using AutoMapper;
using BlogApp.Application.Helpers.CloudinaryService.Service;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
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

    private BlogService _blogService;

    [SetUp]
    public void Setup()
    {
        _mockBlogRepo = new Mock<IBlogRepository>();
        _mockCloudinary = new Mock<ICloudinaryService>();
        _mockBlogHistoryRepo = new Mock<IBaseRepository<BlogHistory>>();
        _mockMapper = new Mock<IMapper>();
        _mockUserRepo = new Mock<IUserRepository>();

        _blogService = new BlogService(
            _mockBlogRepo.Object,
            _mockCloudinary.Object,
            _mockBlogHistoryRepo.Object,
            _mockMapper.Object,
            _mockUserRepo.Object
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

        _mockBlogRepo
           .Setup(r => r.GetFilteredBlogs(It.IsAny<GetRequest<Blogs>>()))
           .ReturnsAsync((blogs, 1));

        var result = await _blogService.GetAllBlogs(new GetRequest<Blogs>());

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(1, result.Data.Count());
        Assert.AreEqual("Test Blog", result.Data.First().Title);
    }

    [Test]
    public async Task GetAllBlogs_ReturnsFailedResponse_WhenRepositoryReturnsNull()
    {
        // Arrange
        _mockBlogRepo
            .Setup(r => r.GetFilteredBlogs(It.IsAny<GetRequest<Blogs>>()))
            .ReturnsAsync((null, 0));

        // Act
        var result = await _blogService.GetAllBlogs(new GetRequest<Blogs>());

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.IsNull(result.Data);
    }
}
