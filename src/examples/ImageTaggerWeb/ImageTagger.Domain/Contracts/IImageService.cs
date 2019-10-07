using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageTagger.Domain.Models;

namespace ImageTagger.Domain.Contracts
{
    public interface IImageService
    {
        Task UploadImageAsync(Stream image, string filename);
        Task<IEnumerable<TaggedImage>> GetAllImageUrls();
    }
}