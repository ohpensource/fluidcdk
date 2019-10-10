using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using FluidCdk.Lambda;
using Microsoft.Extensions.Configuration;

namespace ImageTagger.Infra.Constructs
{
    public interface IWebAppRestApiBuilder : ILambdaRestApiBuilder { }
    
    public class WebAppRestApiBuilder : LambdaRestApiBuilder, IWebAppRestApiBuilder
    {
        private readonly IWebAppFunctionBuilder _appFunctionBuilder;
        private readonly IConfiguration _config;


        public WebAppRestApiBuilder(IWebAppFunctionBuilder appFunctionBuilder, IConfiguration config)
        {
            _appFunctionBuilder = appFunctionBuilder;
            _config = config;
        }

        protected override RestApi Build(Construct scope)
        {
            this.SetHandler(_appFunctionBuilder)
                .AddBinaryMediaType("multipart/form-data")
                .SetName(_config.GetSection("Infrastructure").GetValue<string>("WebApiRestApiName"));

            return base.Build(scope);
        }
    }
}
