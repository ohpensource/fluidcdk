using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;

namespace FluidCdk.Lambda
{
    public interface IApiFunctionBuilder : IFunctionBuilder {}

    public class ApiFunctionBuilder : FunctionBuilder, IFunctionBuilder
    {

        protected override Function Build(Construct scope)
        {
            var result = base.Build(scope);

            var api = new LambdaRestApi(scope, _props.FunctionName + "RestApi", new LambdaRestApiProps
            {
                Handler = result, 
                BinaryMediaTypes = new [] {"multipart/form-data"}
            });

            return result;
        }
    }
}
