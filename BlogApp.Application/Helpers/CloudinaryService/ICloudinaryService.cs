using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Application.Helpers.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<string> UploadImage(IFormFile file);
        Task<string> UploadMultipleImage(List<IFormFile> files);
        Task DeleteImage(string publicId);
    }
}
