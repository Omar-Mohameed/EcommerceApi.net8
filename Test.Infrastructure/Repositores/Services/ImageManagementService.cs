using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Services;

namespace Test.Infrastructure.Repositores.Services
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider fileprovider;// A way to access and manage files

        public ImageManagementService(IFileProvider fileprovider)
        {
            this.fileprovider = fileprovider;
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string folder)
        {
            List<string> SaveImageSrc = new List<string>();// List to hold the URLs of saved images
            var directory = Path.Combine("wwwroot", "Images", folder);

            // Ensure the directory exists , if not create it
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            foreach (IFormFile file in files) // files is collection of images that uploaded from client
            {
                if(file.Length > 0)  // ensure the file is not empty
                {
                    // get image name
                    var ImageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var ImageSrc = $"/Images/{folder}/{ImageName}"; // URL to access the image -> <img src="/Images/Products/pic1.jpg" /> to store in db
                    var root = Path.Combine(directory, ImageName); // Full path to save the image ex wwwroot/Images/Products/img1.png لحفظ الصوره فعليا على السيرفر

                    // save the image to the specified path
                    using (FileStream stream = new FileStream(root, FileMode.Create)) // open file stream on path(root) to write the image
                    {
                        await file.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(ImageSrc);
                }
            }
            return SaveImageSrc;
        }

        public async Task DeleteImageAsync(string src)
        {
            src = src.TrimStart('/');   // remove leading slash
            var filePath = Path.Combine("wwwroot", src);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await Task.CompletedTask;
        }
    }
}
