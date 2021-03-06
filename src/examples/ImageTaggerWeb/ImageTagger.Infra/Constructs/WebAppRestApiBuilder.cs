﻿using System;
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
        readonly IInfraContext _infraContext;
        readonly IWebAppFunctionBuilder _webAppFunctionBuilder;


        public WebAppRestApiBuilder(IInfraContext infraContext, IWebAppFunctionBuilder webAppFunctionBuilder)
        {
            _infraContext = infraContext;
            _webAppFunctionBuilder = webAppFunctionBuilder;
        }

        protected override RestApi Build(Construct scope)
        {
            this.SetHandler(_webAppFunctionBuilder)
                .AddBinaryMediaType("multipart/form-data")
                .SetName(_infraContext.WebApiRestApiName);

            return base.Build(scope);
        }
    }
}
