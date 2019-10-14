using System;
using System.Collections.Generic;
using System.Text;

namespace ImageTagger.Infra
{
    public class InfraContext
    {
        public string Account { get; set; }
        public string Region { get; set; }
        public string StackName { get; set; }
        public string ImageBucketName { get; set; }
        public string ImageTaggerFunctionName { get; set; }
        public string AssetFileFolder { get; set; }
        public string WebApiRestApiName { get; set; }
        public string RestApiFunctionName { get; set; }
    }
}
