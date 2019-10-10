using Amazon.CDK;
using FluidCdk.Core;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IImageTaggerStackBuilder : IStackBuilder {}

    public class ImageTaggerStackBuilder : StackBuilder, IImageTaggerStackBuilder
    {
        readonly ITaggerFunctionBuilder _functionBuilder;
        readonly IWebAppFunctionBuilder _apiBuilder;
        readonly IConfiguration _config;
        private readonly IWebAppRestApiBuilder _restApi;

        public ImageTaggerStackBuilder(ITaggerFunctionBuilder functionBuilder, IWebAppFunctionBuilder apiBuilder, IConfiguration config, IWebAppRestApiBuilder restApi)
        {
            _functionBuilder = functionBuilder;
            _apiBuilder = apiBuilder;
            _config = config;
            _restApi = restApi;
        }

        protected override Stack Build(Construct scope)
        {
            var config = _config.GetSection("Infrastructure");

            this.SetName(config.GetValue<string>("ImageTaggerStackName"))
                .SetEnv(config.GetValue<string>("account"), config.GetValue<string>("region"))
                .Add(_functionBuilder)
                .Add(_restApi)
                .Add(_apiBuilder);

            return base.Build(scope);
        }
    }
}
