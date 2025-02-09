using Microsoft.AspNetCore.Http;

namespace BlogApp.Application.Helpers.CloudinaryService.Service
{
    public interface ICloudinaryService
    {
        Task<string> UploadImage(IFormFile file);
        Task<(List<string> SuccessfulUploads, List<string> FailedUploads)> UploadMultipleImage(List<IFormFile> files);
        Task DeleteImage(string imageUrl);
    }
}
