using System.IO;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.S3;
using FluidCdk.IAM.Grants;
using FluidCdk.Lambda;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface ITaggerFunctionBuilder : IFunctionBuilder {}

    public class TaggerFunctionBuilder : FunctionBuilder, ITaggerFunctionBuilder
    {
        readonly IConfiguration _config;
        readonly IImageBucketBuilder _bucketBuilder;
        
        public TaggerFunctionBuilder(IConfiguration config, IImageBucketBuilder bucketBuilder)
        {
            _config = config;
            _bucketBuilder = bucketBuilder;
        }

        protected override Function Build(Construct scope)
        {
            var config = _config.GetSection("Infrastructure");
            var functionName = config.GetValue<string>("ImageTaggerFunctionName");
            var assetFilename = _config.GetValue<string>("ASSET_FOLDER") + $"\\ImageTagger.Lambda.zip";

            this.SetName(functionName)
                .SetHandler("ImageTagger.Lambda::ImageTagger.Lambda.Function::Handler")
                .SourceFromAsset(assetFilename)
                .Grant(new RekognitionGrant().FullAccess())
                .Grant(new S3Grant()
                    .ReadWrite()
                    .On($"{_bucketBuilder.GetInstance(scope).BucketArn}*")
                )
                .AddS3EventSource(_bucketBuilder, EventType.OBJECT_CREATED);
            
            return base.Build(scope);
        }
    }
}
