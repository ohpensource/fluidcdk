using Amazon.CDK;
using FluidCdk.Core;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IImageTaggerStackBuilder : IStackBuilder {}

    public class ImageTaggerStackBuilder : StackBuilder, IImageTaggerStackBuilder
    {
        readonly IImageTaggerLambdaBuilder _lambdaBuilder;
        readonly IImageTaggerApiBuilder _apiBuilder;
        readonly IConfiguration _config;

        public ImageTaggerStackBuilder(IImageTaggerLambdaBuilder lambdaBuilder, IImageTaggerApiBuilder apiBuilder, IConfiguration config)
        {
            _lambdaBuilder = lambdaBuilder;
            _apiBuilder = apiBuilder;
            _config = config;
        }

        protected override Stack Build(Construct scope)
        {
            var config = _config.GetSection("Infrastructure");

            this.SetName(config.GetValue<string>("ImageTaggerStackName"))
                .SetEnv(config.GetValue<string>("account"), config.GetValue<string>("region"))
                .Add(_lambdaBuilder)
                .Add(_apiBuilder);

            return base.Build(scope);
        }
    }
}
