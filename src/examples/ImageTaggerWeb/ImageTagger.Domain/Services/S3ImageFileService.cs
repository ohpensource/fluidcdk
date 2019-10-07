using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using ImageTagger.Domain.Contracts;
using ImageTagger.Domain.Models;
using Microsoft.Extensions.Logging;

namespace ImageTagger.Domain.Services
{
    public class S3ImageFileService : IImageFileService
    {
        readonly ILogger<S3ImageFileService> _logger;
        readonly IAmazonS3 _s3Client;
        readonly string _s3BucketName;

        public S3ImageFileService(ILogger<S3ImageFileService> logger, IAmazonS3 s3Client)
        {
            _s3BucketName = Environment.GetEnvironmentVariable("IMGTAGGER_BUCKETNAME");
            _logger = logger;
            _s3Client = s3Client;
        }

        public async Task UploadAsync(Stream file, string filename)
        {
            try
            {

                await _s3Client.PutObjectAsync(new PutObjectRequest
                {
                    Key = filename,
                    BucketName = _s3BucketName,
                    InputStream = file,
                    ContentType = "image/jpeg"
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Error uploading {filename}. {ex.Message} - {ex.StackTrace}");
                throw;
            }

            // await _s3Client.UploadObjectFromStreamAsync(_s3BucketName, filename, file, null);
        }

        public async Task<IEnumerable<TaggedImage>> GetAllImages()
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _s3BucketName
            };
            ListObjectsV2Response response = null;
            var taggedFiles = new List<TaggedImage>();
            do
            {
                response = await _s3Client.ListObjectsV2Async(request);
                var batch = response.S3Objects
                    .Where(o => IsSupportedImageFormat(o.Key))
                    .Select(o => GetTaggedImage(o).Result);

                taggedFiles.AddRange(batch);

            } while (response.HttpStatusCode == HttpStatusCode.OK && response.IsTruncated);

            return taggedFiles;
        }

        private bool IsSupportedImageFormat(string fileName)
        {
            return new[] {".jpg", ".png", ".bmp"}.Contains(Path.GetExtension(fileName)?.ToLower());
        }

        private async Task<Dictionary<string, string>> GetFileTags(string key)
        {
            var request = new GetObjectTaggingRequest
            {
                BucketName = _s3BucketName,
                Key = key
            };
            var response = await _s3Client.GetObjectTaggingAsync(request);

            if (response.HttpStatusCode != HttpStatusCode.OK)
                return new Dictionary<string, string>();

            return response.Tagging.OrderByDescending(t => t.Value).ToDictionary(t => t.Key, t => t.Value);
        }

        private string GeneratePresignedUrl(string key)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _s3BucketName,
                Key = key,
                Expires = DateTime.Now.AddMinutes(5)
            };

            return _s3Client.GetPreSignedURL(request);

        }

        private async Task<TaggedImage> GetTaggedImage(S3Object s3Object)
        {
            var tags = await GetFileTags(s3Object.Key);

            return new TaggedImage
            {
                FileName = s3Object.Key,
                Path = Path.GetDirectoryName(s3Object.Key),
                Tags = tags,
                SizeInKb = s3Object.Size / 1024,
                PresignedUrl = GeneratePresignedUrl(s3Object.Key)
            };
        }
    }
}
