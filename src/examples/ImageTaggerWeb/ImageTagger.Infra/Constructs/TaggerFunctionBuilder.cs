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
        readonly InfraContext _infraContext;
        readonly IImageBucketBuilder _bucketBuilder;
        
        public TaggerFunctionBuilder(InfraContext infraContext, IImageBucketBuilder bucketBuilder)
        {
            _infraContext = infraContext;
            _bucketBuilder = bucketBuilder;
        }

        protected override Function Build(Construct scope)
        {
            var functionName = _infraContext.ImageTaggerFunctionName;
            var assetFilename = _infraContext.AssetFileFolder + $"\\ImageTagger.Lambda.zip";

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
