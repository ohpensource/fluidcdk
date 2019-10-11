using Amazon.CDK;
using FluidCdk.Lambda;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IWebAppFunctionBuilder : IFunctionBuilder { }

    public class WebAppFunctionBuilder : FunctionBuilder, IWebAppFunctionBuilder
    {
        readonly IConfiguration _config;
        readonly IImageBucketBuilder _imageBucket;

        public WebAppFunctionBuilder(IConfiguration config, IImageBucketBuilder imageBucket)
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
                .AddEnvVariables("IMGTAGGER_BUCKETNAME", _imageBucket.GetInstance(scope).BucketName)
                .GrantS3ReadWrite();

            var result = base.Build(scope);

            return result;
        }
    }
}
