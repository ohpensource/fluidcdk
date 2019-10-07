using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using FluidCdk.Lambda;
using FluidCdk.S3;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{

    public interface IImageBucketBuilder : IBucketBuilder
    {
    }

    public class ImageBucketBuilder : BucketBuilder, IImageBucketBuilder
    {
        readonly IConfiguration _config;

        public ImageBucketBuilder(IConfiguration config)
        {
            _config = config;
        }

        protected override Bucket Build(Construct scope)
        {
            this.SetName(_config.GetSection("Infrastructure").GetValue<string>("ImageBucketName"));
            return base.Build(scope);
        }
    }
}
