using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.S3;
using FluidCdk.Core.Contracts;

namespace FluidCdk.Lambda.Entities
{
    public class S3EventEntity
    {
        public IConstructBuilder<Bucket> BucketBuilder { get; set; }
        public S3EventSourceProps Props { get; set; }
    }
}
