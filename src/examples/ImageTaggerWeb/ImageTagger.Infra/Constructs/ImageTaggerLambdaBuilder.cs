using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.S3;
using FluidCdk.Lambda;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IImageTaggerLambdaBuilder : IFunctionBuilder {}

    public class ImageTaggerLambdaBuilder : FunctionBuilder, IImageTaggerLambdaBuilder
    {
        readonly IConfiguration _config;
        readonly IImageBucketBuilder _bucketBuilder;
        
        public ImageTaggerLambdaBuilder(IConfiguration config, IImageBucketBuilder bucketBuilder)
        {
            _config = config;
            _bucketBuilder = bucketBuilder;
        }

        protected override Function Build(Construct scope)
        {
            var config = _config.GetSection("Infrastructure");

            var functionName = config
                .GetValue<string>("ImageTaggerFunctionName");

            this.SetName(functionName)
                .SetHandler(typeof(ImageTagger.Lambda.Function), nameof(ImageTagger.Lambda.Function.Handler))
                .SourceFromBucket(config.GetValue<string>("CodeBucketName"), config.GetValue<string>("ImageTaggerFunctionBucketKey"))
                .GrantRecognitionReadOnly()
                .GrantS3ReadWrite()
                .AddS3EventSource(_bucketBuilder, new S3EventSourceProps
                {
                    Events = new EventType[] { EventType.OBJECT_CREATED }
                });
            
            return base.Build(scope);
        }
    }
}
