using BlogApp.API.Controllers;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using BlogApp.Domain.Shared;

namespace BlogApp.Tests.ControllerTests
{
    [TestFixture]
    public class BlogControllerTests
    {
        private Mock<IBlogService> _mockBlogService;
        private BlogController _controller;
        [SetUp]
        public void Setup()
        {
            _mockBlogService = new Mock<IBlogService>();
            _controller = new BlogController(_mockBlogService.Object);
        }

        [Test]
        public async Task GetBlogs_ReturnsOk_WithValidData()
        {
            var mockBlogs = new List<BlogsDTO>
            {
                new BlogsDTO
                {
                    BlogId = 1,
                    Title = "Test Blog",
                    Description = "Test Desc",
                    User = new BlogUser { UserId = "1", Name = "User1" },
                    UpVoteCount = 10,
                    DownVoteCount = 2,
                    CommentCount = 5,
                    ImageUrl = "image.jpg"
                }
            };

            var resp = ApiResponse<IEnumerable<BlogsDTO>>.Success(mockBlogs, "All Blogs Listed", HttpStatusCode.OK, 1);
            _mockBlogService.Setup(s => s.GetAllBlogs(It.IsAny<GetRequest<Blogs>>(), It.IsAny<CacheRequestItems>())).ReturnsAsync(resp);

            // Act
            var result = await _controller.GetBlogs("popularity", 0, 10, "Test");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnedRes = okResult.Value as ApiResponse<IEnumerable<BlogsDTO>>;
            Assert.Multiple(() =>
            {
                Assert.That(returnedRes, Is.Not.Null);
                Assert.That(returnedRes!.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(returnedRes.Data.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public async Task GetBlogs_ReturnsError_WhenServiceFails()
        {
            // Arrange
            var response = ApiResponse<IEnumerable<BlogsDTO>>.Failed(null, "Service error", HttpStatusCode.InternalServerError);
            _mockBlogService.Setup(s => s.GetAllBlogs(It.IsAny<GetRequest<Blogs>>(), It.IsAny<CacheRequestItems>())).ReturnsAsync(response);

            // Act
            var result = await _controller.GetBlogs(null, null, null, null);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult, Is.Not.Null);
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
    }
}