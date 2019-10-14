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
using Newtonsoft.Json;

namespace ImageTagger.Domain.Services
{
    public class S3ImageFileService : IImageFileService
    {
        readonly ILogger<S3ImageFileService> _logger;
        readonly IAmazonS3 _s3Client;
        readonly string _s3BucketName;

        public S3ImageFileService(ILogger<S3ImageFileService> logger, IAmazonS3 s3Client)
        {
            _logger = logger;
            _s3Client = s3Client;
            _s3BucketName = Environment.GetEnvironmentVariable("IMGTAGGER_BUCKETNAME");
            _logger.LogDebug($"S3ImageFileServce bucketname: {_s3BucketName}");
        }

        public async Task UploadAsync(Stream file, string filename)
        {
            using(var scope = _logger.BeginScope($"{nameof(UploadAsync)}(file,{filename})"))
            {
                try
                {
                    await _s3Client.UploadObjectFromStreamAsync(_s3BucketName, filename, file, null);

                    _logger.LogDebug("Uploaded file");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error uploading {filename}. {ex.Message} - {ex.StackTrace}");
                    throw;
                }
            }
        }

        public async Task<IEnumerable<TaggedImage>> GetAllImages()
        {
            using (var scope = _logger.BeginScope(nameof(GetAllImages)))
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

                    _logger.LogDebug($"Response: {JsonConvert.SerializeObject(response)}");

                    var batch = response.S3Objects
                        .Where(o => IsSupportedImageFormat(o.Key))
                        .Select(o => GetTaggedImage(o).Result);

                    taggedFiles.AddRange(batch);

                } while (response.HttpStatusCode == HttpStatusCode.OK && response.IsTruncated);

                return taggedFiles;
            }
        }

        private bool IsSupportedImageFormat(string fileName)
        {
            return new[] {".jpg", ".png", ".jpeg", ".bmp"}.Contains(Path.GetExtension(fileName)?.ToLower());
        }

        private async Task<Dictionary<string, string>> GetFileTags(string key)
        {
            using (var scope = _logger.BeginScope(nameof(GetFileTags)))
            {
                var request = new GetObjectTaggingRequest
                {
                    BucketName = _s3BucketName,
                    Key = key
                };
                
                var response = await _s3Client.GetObjectTaggingAsync(request);

                _logger.LogDebug($"Returned: {JsonConvert.SerializeObject(response)} ");

                if (response.HttpStatusCode != HttpStatusCode.OK)
                    return new Dictionary<string, string>();

                var result = response.Tagging.OrderByDescending(t => t.Value).ToDictionary(t => t.Key, t => t.Value);

                return result;
            }
        }

        private string GeneratePresignedUrl(string key)
        {
            using (var scope = _logger.BeginScope($"{nameof(GeneratePresignedUrl)}({key})"))
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _s3BucketName,
                    Key = key,
                    Expires = DateTime.Now.AddMinutes(5)
                };

                var result = _s3Client.GetPreSignedURL(request);
                return result;
            }
        }

        private async Task<TaggedImage> GetTaggedImage(S3Object s3Object)
        {
            using (var scope = _logger.BeginScope($"{nameof(GetTaggedImage)}({JsonConvert.SerializeObject(s3Object)})"))
            {
                var tags = await GetFileTags(s3Object.Key);

                var result = new TaggedImage
                {
                    FileName = s3Object.Key,
                    Path = Path.GetDirectoryName(s3Object.Key),
                    Tags = tags,
                    SizeInKb = s3Object.Size / 1024,
                    PresignedUrl = GeneratePresignedUrl(s3Object.Key)
                };

                return result;
            }
        }
    }
}
