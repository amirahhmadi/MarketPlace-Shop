using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.ExtenstionMethods
{
    public static class ImageExtension
    {
        public static string UploadImage(this IFormFile? image, string path)
        {
            if (image == null)
                return "";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var extension = Path.GetExtension(image.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowedExtensions.Contains(extension.ToLower()))
                return "";

            string imageName = Guid.NewGuid().ToString("N").Substring(0, 10) + extension;
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), path, imageName);

            using var stream = new FileStream(savePath, FileMode.Create);
            image.CopyTo(stream);

            return imageName;
        }

        public static void RemoveImage(string imageName, string path)
        {
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), path, imageName);

            if (File.Exists(imgPath))
                File.Delete(imgPath);
        }
    }
}
