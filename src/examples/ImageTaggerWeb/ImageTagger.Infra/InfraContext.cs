using System;
using System.Collections.Generic;
using System.Text;

namespace ImageTagger.Infra
{

    public interface IInfraContext
    {
        string Account { get; set; }
        string Region { get; set; }
        string StackName { get; set; }
        string ImageBucketName { get; set; }
        string ImageTaggerFunctionName { get; set; }
        string AssetFileFolder { get; set; }
        string WebApiRestApiName { get; set; }
        string RestApiFunctionName { get; set; }

    }

    public class InfraContext : IInfraContext
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
