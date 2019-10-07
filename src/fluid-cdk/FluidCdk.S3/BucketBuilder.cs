using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Notifications;
using FluidCdk.Core;
using FluidCdk.Core.Contracts;
using FluidCdk.S3.Entities;

namespace FluidCdk.S3
{
    public interface IBucketBuilder : IConstructBuilder<Bucket> {}

    public class BucketBuilder : ConstructBuilderBase<Bucket>, IBucketBuilder
    {
        string _name;
        readonly List<BucketEventEntity> _events = new List<BucketEventEntity>();

        readonly BucketProps _props = new BucketProps();

        public BucketBuilder() { }

        public BucketBuilder(string bucketName) : this()
        {
            _name = bucketName;
        }

        public BucketBuilder SetName(string bucketName)
        {
            _name = bucketName;
            return this;
        }

        public BucketBuilder SetAccessControl(BucketAccessControl accessControl)
        {
            _props.AccessControl = accessControl;
            return this;
        }

        public BucketBuilder OnNewObject(IConstructBuilder<Function> function, INotificationKeyFilter[] filters)
        {
            _events.Add(new BucketEventEntity {
                EventType = EventType.OBJECT_CREATED,
                Destination = function,
                Filter = filters
            });

            return this;
        }

        protected override Bucket Build(Construct scope)
        {
            var bucket = new Bucket(scope, _name, _props);
            _events.ForEach(e => bucket.AddEventNotification(e.EventType, new LambdaDestination(e.Destination.GetInstance(scope)), e.Filter));
            return bucket;
        }
    }
}
