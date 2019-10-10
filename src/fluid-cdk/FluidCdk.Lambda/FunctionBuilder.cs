using System;
using System.Collections.Generic;
using System.IO;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.S3;
using FluidCdk.Core;
using FluidCdk.Core.Contracts;
using FluidCdk.Lambda.Entities;

namespace FluidCdk.Lambda
{

    public interface IFunctionBuilder : IConstructBuilder<Function>
    {
        IFunctionBuilder SetName(string name);
        IFunctionBuilder SetDescription(string description);
        IFunctionBuilder SetTimeout(Duration timeout);
        IFunctionBuilder SetMemorySize(int memorySize);
        IFunctionBuilder SetHandler(string handler);
        IFunctionBuilder SetHandler(Type type, string handler);
        IFunctionBuilder SetLogRetentionDays(RetentionDays retentionDays);
        IFunctionBuilder SourceFromBucket(string bucketName, string key);
        IFunctionBuilder SourceFromAsset(string assetLocation);
        IFunctionBuilder WithInlineCode(Runtime runtime, string code);
        IFunctionBuilder AddS3EventSource(IConstructBuilder<Bucket> bucketBuilder, S3EventSourceProps props);
        IFunctionBuilder AddPolicyStatement(PolicyStatement policyStatement);
        IFunctionBuilder GrantS3ReadWrite();
        IFunctionBuilder GrantS3ReadOnly();
        IFunctionBuilder GrantRecognitionReadOnly();
    }

    public class FunctionBuilder : ConstructBuilderBase<Function>, IFunctionBuilder
    {

        protected readonly FunctionProps _props = new FunctionProps
        {
            Runtime = Runtime.DOTNET_CORE_2_1,
            Timeout = Duration.Seconds(30),
            MemorySize = 128
        };

        string _name = "";
        readonly List<S3EventEntity> _s3Events = new List<S3EventEntity>();
        string _codeBucketKey = null;
        string _codeBucket = null;
        string _assetPath = null;

        readonly List<PolicyStatement> _policyStatements = new List<PolicyStatement>();

        public FunctionBuilder() { }

        public FunctionBuilder(string name)
        {
            _name = name;
        }

        protected override Function Build(Construct scope)
        {
            if (!string.IsNullOrWhiteSpace(_codeBucket))
            {
                var bucket = Bucket.FromBucketName(scope, _codeBucket+_name, _codeBucket);
                _props.Code = Code.FromBucket(bucket, _codeBucketKey);
            }

            if (!string.IsNullOrWhiteSpace(_assetPath))
            {
                _props.Code = Code.FromAsset(_assetPath);
            }

            var lambda = new Function(scope, _name, _props);

            _s3Events.ForEach(e => lambda.AddEventSource(new S3EventSource(e.BucketBuilder.GetInstance(scope), e.Props)));
            _policyStatements.ForEach(p => lambda.AddToRolePolicy(p));

            return lambda;
        }

        public IFunctionBuilder SetName(string functionName)
        {
            _name = functionName;
            return this;
        }

        public IFunctionBuilder SetDescription(string description)
        {
            _props.Description = description;
            return this;
        }

        public IFunctionBuilder SetTimeout(Duration timeout)
        {
            _props.Timeout = timeout;
            return this;
        }

        public IFunctionBuilder SetMemorySize(int memorySize)
        {
            _props.MemorySize = memorySize;
            return this;
        }

        public IFunctionBuilder SetHandler(string handler)
        {
            _props.Handler = handler;
            return this;
        }

        public IFunctionBuilder SetHandler(Type type, string handler)
        {
            _props.Handler = $"{type.Namespace}::{type.FullName}::{handler}";
            return this; 
        }

        public IFunctionBuilder SetLogRetentionDays(RetentionDays retentionDays)
        {
            _props.LogRetention = retentionDays;
            return this;
        }

        public IFunctionBuilder SourceFromBucket(string bucketName, string key)
        {
            _codeBucket = bucketName;
            _codeBucketKey = key;

            return this;
        }

        public IFunctionBuilder SourceFromAsset(string assetLocation)
        {
            _assetPath = assetLocation;
            return this;
        }

        public IFunctionBuilder WithInlineCode(Runtime runtime, string code)
        {
            _props.Runtime = runtime;
            _props.Code = Code.FromInline(code);
            return this;
        }

        public IFunctionBuilder AddS3EventSource(IConstructBuilder<Bucket> bucketBuilder, S3EventSourceProps props )
        {
            _s3Events.Add(new S3EventEntity { BucketBuilder = bucketBuilder, Props = props});
            return this;
        }

        public IFunctionBuilder AddPolicyStatement(PolicyStatement policyStatement)
        {
            _policyStatements.Add(policyStatement);
            return this;
        }

        public IFunctionBuilder GrantS3ReadWrite()
        {
            return AddPolicyStatement(S3Helper.GetFullAccess());
        }

        public IFunctionBuilder GrantS3ReadOnly()
        {
            return AddPolicyStatement(S3Helper.GetReadonlyAccess());
        }

        public IFunctionBuilder GrantRecognitionReadOnly()
        {
            var imgRecPolicyStatement = new PolicyStatement();
            imgRecPolicyStatement.AddActions("rekognition:*");
            imgRecPolicyStatement.AddResources("*");
            return AddPolicyStatement(imgRecPolicyStatement);
        }

    }
}
