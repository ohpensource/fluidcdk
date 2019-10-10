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
        private readonly IWebAppRestApiBuilder _webAppRestApiBuilder;

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
                .GrantS3ReadWrite();

            
            var result = base.Build(scope);
            result.AddEnvironment("IMGTAGGER_BUCKETNAME", _imageBucket.GetInstance(scope).BucketName);

            return result;
        }
    }
}
