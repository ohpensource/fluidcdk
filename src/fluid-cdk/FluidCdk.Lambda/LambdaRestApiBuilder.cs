using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using FluidCdk.Core;
using FluidCdk.Core.Contracts;

namespace FluidCdk.Lambda
{

    public interface ILambdaRestApiBuilder : IConstructBuilder<RestApi>
    {
        ILambdaRestApiBuilder SetName(string name);

        ILambdaRestApiBuilder SetHandler(IFunctionBuilder function);
        ILambdaRestApiBuilder AddBinaryMediaType(string mediaType);
    }

    public class LambdaRestApiBuilder : ConstructBuilderBase<RestApi>, ILambdaRestApiBuilder
    {

        private LambdaRestApiProps _props = new LambdaRestApiProps();
        private string _name = Guid.NewGuid().ToString();
        private List<string> _binaryMediaTypes = new List<string>();
        private IFunctionBuilder _handler = null;

        protected override RestApi Build(Construct scope)
        {

            if (_binaryMediaTypes.Any())
                _props.BinaryMediaTypes = _binaryMediaTypes.ToArray();

            if (_handler != null)
                _props.Handler = _handler.GetInstance(scope);

            var result = new LambdaRestApi(scope, _name, _props);
            return result;
        }

        public ILambdaRestApiBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public ILambdaRestApiBuilder SetHandler(IFunctionBuilder functionBuilder)
        {
            _handler = functionBuilder;
            return this;
        }

        public ILambdaRestApiBuilder AddBinaryMediaType(string mediaType)
        {
            _binaryMediaTypes.Add(mediaType);
            return this;
        }
    }
}
