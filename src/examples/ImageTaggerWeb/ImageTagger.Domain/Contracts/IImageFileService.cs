using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ImageTagger.Domain.Models;

namespace ImageTagger.Domain.Contracts
{
    public interface IImageFileService
    {
        Task UploadAsync(Stream file, string filename);

        Task<IEnumerable<TaggedImage>> GetAllImages();

    }
}