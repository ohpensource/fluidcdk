using Amazon.CDK;
using FluidCdk.IAM.Grants;
using FluidCdk.Lambda;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IWebAppFunctionBuilder : IFunctionBuilder { }

    public class WebAppFunctionBuilder : FunctionBuilder, IWebAppFunctionBuilder
    {
        readonly InfraContext _infraContext;
        readonly IImageBucketBuilder _imageBucket;

        public WebAppFunctionBuilder(InfraContext infraContext, IImageBucketBuilder imageBucket)
        {
            _infraContext = infraContext;
            _imageBucket = imageBucket;
        }

        protected override Amazon.CDK.AWS.Lambda.Function Build(Construct scope)
        {

            this
                .SourceFromAsset(_infraContext.AssetFileFolder + "\\ImageTagger.Web.zip")
                .SetHandler("ImageTagger.Web::ImageTagger.Web.LambdaEntryPoint::FunctionHandlerAsync")
                .SetName(_infraContext.RestApiFunctionName)
                .AddEnvVariables("IMGTAGGER_BUCKETNAME", _imageBucket.GetInstance(scope).BucketName)
                .Grant(new S3Grant()
                    .ReadWrite()
                    .On($"{_imageBucket.GetInstance(scope).BucketArn}*")
                );
                

            var result = base.Build(scope);

            return result;
        }
    }
}
