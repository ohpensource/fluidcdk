using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using FluidCdk.Core.Contracts;

namespace FluidCdk.S3.Entities
{
    public class BucketEventEntity
    {
        public EventType EventType { get; set; }
        public IConstructBuilder<Function> Destination { get; set; }
        public INotificationKeyFilter[] Filter { get; set; }
    }
}
