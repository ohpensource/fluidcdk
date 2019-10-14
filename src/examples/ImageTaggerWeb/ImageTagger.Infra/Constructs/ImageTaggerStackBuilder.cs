using Amazon.CDK;
using FluidCdk.Core;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IImageTaggerStackBuilder : IStackBuilder {}

    public class ImageTaggerStackBuilder : StackBuilder, IImageTaggerStackBuilder
    {
        readonly InfraContext _infraContext;
        readonly ITaggerFunctionBuilder _functionBuilder;
        readonly IWebAppFunctionBuilder _apiBuilder;
        private readonly IWebAppRestApiBuilder _restApi;

        public ImageTaggerStackBuilder(
            InfraContext infraContext, 
            ITaggerFunctionBuilder functionBuilder, 
            IWebAppFunctionBuilder apiBuilder, 
            IWebAppRestApiBuilder restApi)
        {
            _infraContext = infraContext;
            _functionBuilder = functionBuilder;
            _apiBuilder = apiBuilder;
            _restApi = restApi;
        }

        protected override Stack Build(Construct scope)
        {
            this.SetName(_infraContext.StackName)
                .SetEnv(_infraContext.Account, _infraContext.Region)
                .Add(_functionBuilder)
                .Add(_restApi)
                .Add(_apiBuilder);

            return base.Build(scope);
        }
    }
}
