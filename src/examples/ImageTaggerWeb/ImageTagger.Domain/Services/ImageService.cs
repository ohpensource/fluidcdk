using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageTagger.Domain.Contracts;
using ImageTagger.Domain.Models;
using Microsoft.Extensions.Logging;

namespace ImageTagger.Domain.Services
{
    public class ImageService : IImageService
    {
        readonly ILogger<ImageService> _logger;
        readonly IImageFileService _imageFileService;

        public ImageService(ILogger<ImageService> logger, IImageFileService imageFileService)
        {
            _logger = logger;
            _imageFileService = imageFileService;
        }

        public async Task UploadImageAsync(Stream image, string filename)
        {
            try
            {
                await _imageFileService.UploadAsync(image, filename);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error uploading file: ${filename}");
                throw;
            }
        }

        public async Task<IEnumerable<TaggedImage>> GetAllImageUrls()
        {
            var objects = await _imageFileService.GetAllImages();
            return objects;
        }

    }
}
