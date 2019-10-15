using Amazon.CDK;
using FluidCdk.Core;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IImageTaggerStackBuilder : IStackBuilder {}

    public class ImageTaggerStackBuilder : StackBuilder, IImageTaggerStackBuilder
    {
        readonly IInfraContext _infraContext;
        readonly ITaggerFunctionBuilder _taggerFunctionBuilder;
        readonly IWebAppFunctionBuilder _apiFunctionBuilder;
        readonly IWebAppRestApiBuilder _restApiBuilder;

        public ImageTaggerStackBuilder(
            IInfraContext infraContext, 
            ITaggerFunctionBuilder taggerFunctionBuilder, 
            IWebAppFunctionBuilder apiFunctionBuilder, 
            IWebAppRestApiBuilder restApiBuilder)
        {
            _infraContext = infraContext;
            _taggerFunctionBuilder = taggerFunctionBuilder;
            _apiFunctionBuilder = apiFunctionBuilder;
            _restApiBuilder = restApiBuilder;
        }

        protected override Stack Build(Construct scope)
        {
            this.SetName(_infraContext.StackName)
                .SetEnv(_infraContext.Account, _infraContext.Region)
                .Add(_taggerFunctionBuilder)
                .Add(_restApiBuilder)
                .Add(_apiFunctionBuilder);

            return base.Build(scope);
        }
    }
}
