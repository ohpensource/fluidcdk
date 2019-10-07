using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageTagger.Domain.Contracts;
using ImageTagger.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageTagger.Web.Pages
{
    public class IndexModel : PageModel
    {
        readonly IImageService _imageService;

        [Required]
        [Display(Name = "Picture")]
        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public List<TaggedImage> ImagesInBucket { get; set; }

        public IndexModel(IImageService imageService)
        {
            _imageService = imageService;
            ImagesInBucket = new List<TaggedImage>();
        }

        public async Task OnGetAsync()
        {
            ImagesInBucket = (await _imageService.GetAllImageUrls()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FileUpload != null)
            {
                using (var stream = FileUpload.OpenReadStream())
                {

                    var fileName = FileUpload.FileName.Replace(" ", "-");
                    await _imageService.UploadImageAsync(stream, fileName);
                }
            }

            return Redirect(Url.Content("~/"));
        }
    }
}
