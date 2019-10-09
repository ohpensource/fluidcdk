using Amazon.CDK;
using FluidCdk.Lambda;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IImageTaggerApiBuilder : IApiFunctionBuilder { }

    public class ImageTaggerApiBuilder : ApiFunctionBuilder, IImageTaggerApiBuilder
    {
        readonly IConfiguration _config;
        readonly IImageBucketBuilder _imageBucket;

        public ImageTaggerApiBuilder(IConfiguration config, IImageBucketBuilder imageBucket)
        {
            _config = config;
            _imageBucket = imageBucket;
        }

        protected override Amazon.CDK.AWS.Lambda.Function Build(Construct scope)
        {
            var infraConfig = _config.GetSection("Infrastructure");
            var name = infraConfig.GetValue<string>("RestApiFunctionName");

            this
                .SourceFromAsset(_config.GetValue<string>("ASSET_FOLDER") + $"\\ImageTagger.Web.zip")
                .SetHandler("ImageTagger.Web::ImageTagger.Web.LambdaEntryPoint::FunctionHandlerAsync")
                .SetName(name)
                .GrantS3ReadWrite();

            
            var result = base.Build(scope);
            result.AddEnvironment("IMGTAGGER_BUCKETNAME", _imageBucket.GetInstance(scope).BucketName);

            return result;
        }
    }
}
