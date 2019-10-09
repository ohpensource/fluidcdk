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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImageTagger.Web.Pages
{
    public class IndexModel : PageModel
    {
        readonly IImageService _imageService;
        private readonly ILogger<IndexModel> _logger;

        [Required]
        [Display(Name = "Picture")]
        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public List<TaggedImage> ImagesInBucket { get; set; }

        public IndexModel(IImageService imageService, ILogger<IndexModel> logger)
        {
            _imageService = imageService;
            _logger = logger;
            ImagesInBucket = new List<TaggedImage>();
        }

        public async Task OnGetAsync()
        {
            try
            {
                ImagesInBucket = (await _imageService.GetAllImageUrls()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnGetAsync: \n"+JsonConvert.SerializeObject(ex));
                throw ;
            }
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
