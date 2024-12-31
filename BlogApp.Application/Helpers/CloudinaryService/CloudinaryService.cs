using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace BlogApp.Application.Helpers.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;
        private readonly string folderName = "BlogAppPhotos";

        public CloudinaryService(Cloudinary cloudinary, ILogger<CloudinaryService> logger)
        {
            _cloudinary = cloudinary;
            _logger = logger;
        }

        #region helper methods
        public bool GetFolders()
        {
            var res = _cloudinary.SearchFolders().Expression($"name={folderName}").Execute();
            var resultJson = JsonConvert.SerializeObject(res);
            JToken token = JToken.Parse(resultJson);

            bool folderExists = token["folders"]?.Any(x => x["path"]?.ToString() == folderName) ?? false;

            if (folderExists) return true;

            else return false;
        }
        #endregion

        public async Task<string> UploadImage(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return null;
            }
            var folderExists = GetFolders();
            if (!folderExists)
            {
                // create folder if it does not exists.
                _cloudinary.CreateFolder(folderName);
            }

            using var stream = file.OpenReadStream();
            var uploadParam = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
            };

            var uploadRes = await _cloudinary.UploadAsync(uploadParam);
            if (uploadRes.StatusCode == HttpStatusCode.OK)
            {
                return uploadRes.SecureUrl.ToString();
            }
            throw new Exception("Failed to upload image to Cloudinary");
        }

        public async Task<(List<string> SuccessfulUploads, List<string> FailedUploads)> UploadMultipleImage(List<IFormFile> files)
        {
            var failedUploads = new List<string>();
            var successfulUploads = new List<string>();

            if (files == null || files.Count == 0)
                return (successfulUploads, failedUploads);

            var folderExists = GetFolders();
            if (!folderExists)
            {
                // create folder if it does not exists.
                _cloudinary.CreateFolder(folderName);
            }

            // uploading each image from the list of images
            foreach (var item in files)
            {
                try
                {
                    using var stream = item.OpenReadStream();
                    var uploadParam = new ImageUploadParams
                    {
                        File = new FileDescription(item.FileName, stream),
                        Folder = folderName,
                    };
                    var uploadRes = await _cloudinary.UploadAsync(uploadParam);
                    if (uploadRes.StatusCode == HttpStatusCode.OK)
                    {
                        successfulUploads.Add(uploadRes.SecureUrl.ToString());
                    }
                    failedUploads.Add($"failed to upload file: {item.FileName}");
                }
                catch (Exception)
                {
                    failedUploads.Add(item.FileName);
                }
            }

            return (successfulUploads, failedUploads);
        }

        public async Task DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            string publicId = imageUrl.Split('/').Last().Split('.').First();
            string cloudImageUrl = $"{folderName}/{publicId}";
            var deleteResult = await _cloudinary.DeleteResourcesAsync(ResourceType.Image, cloudImageUrl);

            if (deleteResult.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Failed to delete image with PublicId: {PublicId}. StatusCode: {StatusCode}",
                       publicId, deleteResult.StatusCode);
            }
        }
    }
}
