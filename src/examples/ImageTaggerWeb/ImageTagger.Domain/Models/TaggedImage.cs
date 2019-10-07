using System;
using System.Collections.Generic;
using System.Text;

namespace ImageTagger.Domain.Models
{
    public class TaggedImage
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public long SizeInKb { get; set; }
        public string PresignedUrl { get; set; }
        public Dictionary<string,string> Tags { get; set; }
    }
}
