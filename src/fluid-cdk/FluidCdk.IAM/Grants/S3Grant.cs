using System.Collections.Generic;
using System.Linq;
using Amazon.CDK.AWS.IAM;
using FluidCdk.Core;
using FluidCdk.Core.Contracts;

namespace FluidCdk.IAM.Grants
{
    public class S3Grant : GrantBase
    {
        public S3Grant FullAccess()
        {
            _ = Actions("s3:*");
            return this;
        }

        public S3Grant Readonly()
        {
            var readonlyActions =
                new S3Permissions[]
                {
                    S3Permissions.GetObject,
                    S3Permissions.GetObjectAcl,
                    S3Permissions.GetObjectLegalHold,
                    S3Permissions.GetObjectRetention,
                    S3Permissions.GetObjectTagging,
                    S3Permissions.GetObjectTorrent,
                    S3Permissions.GetObjectVersion,
                    S3Permissions.GetObjectVersionAcl,
                    S3Permissions.GetObjectVersionTagging,
                    S3Permissions.GetObjectVersionTorrent,
                    S3Permissions.ListMultipartUploadParts,
                    S3Permissions.ListBucket,
                    S3Permissions.ListBucketVersions,
                    S3Permissions.ListAllMyBuckets,
                    S3Permissions.ListBucketMultipartUploads,
                }.Select(a => $"s3:{a}");
            _ = this.Actions(readonlyActions.ToArray());
            return this;
        }

        public S3Grant ReadWrite()
        {

            var readWriteActions =
                new S3Permissions[]
                {
                    S3Permissions.GetObject,
                    S3Permissions.GetObjectAcl,
                    S3Permissions.GetObjectLegalHold,
                    S3Permissions.GetObjectRetention,
                    S3Permissions.GetObjectTagging,
                    S3Permissions.GetObjectTorrent,
                    S3Permissions.GetObjectVersion,
                    S3Permissions.GetObjectVersionAcl,
                    S3Permissions.GetObjectVersionTagging,
                    S3Permissions.GetObjectVersionTorrent,
                    S3Permissions.ListMultipartUploadParts,
                    S3Permissions.ListBucket,
                    S3Permissions.ListBucketVersions,
                    S3Permissions.ListAllMyBuckets,
                    S3Permissions.ListBucketMultipartUploads,
                    S3Permissions.AbortMultipartUpload,
                    S3Permissions.BypassGovernanceRetention,
                    S3Permissions.DeleteObject,
                    S3Permissions.DeleteObjectTagging,
                    S3Permissions.DeleteObjectVersion,
                    S3Permissions.DeleteObjectVersionTagging,
                    S3Permissions.PutObject,
                    S3Permissions.PutObjectAcl,
                    S3Permissions.PutObjectLegalHold,
                    S3Permissions.PutObjectRetention,
                    S3Permissions.PutObjectTagging,
                    S3Permissions.PutObjectVersionAcl,
                    S3Permissions.PutObjectVersionTagging,
                    S3Permissions.RestoreObject,
                    S3Permissions.CreateBucket,
                    S3Permissions.DeleteBucket,
                }.Select(a => $"s3:{a}");

            _ = Actions(readWriteActions.ToArray());

            return this;
        }

        public enum S3Permissions
        {
            GetObject,
            GetObjectAcl,
            GetObjectLegalHold,
            GetObjectRetention,
            GetObjectTagging,
            GetObjectTorrent,
            GetObjectVersion,
            GetObjectVersionAcl,
            GetObjectVersionTagging,
            GetObjectVersionTorrent,
            ListMultipartUploadParts,
            ListBucket,
            ListBucketVersions,
            ListAllMyBuckets,
            ListBucketMultipartUploads,

            AbortMultipartUpload,
            BypassGovernanceRetention,
            DeleteObject,
            DeleteObjectTagging,
            DeleteObjectVersion,
            DeleteObjectVersionTagging,
            PutObject,
            PutObjectAcl,
            PutObjectLegalHold,
            PutObjectRetention,
            PutObjectTagging,
            PutObjectVersionAcl,
            PutObjectVersionTagging,
            RestoreObject,
            CreateBucket,
            DeleteBucket,

            // Special (finegrain)
            DeleteBucketPolicy,
            DeleteBucketWebsite,
            GetAccelerateConfiguration,
            GetAnalyticsConfiguration,
            GetBucketAcl,
            GetBucketCORS,
            GetBucketLocation,
            GetBucketLogging,
            GetBucketNotification,
            GetBucketObjectLockConfiguration,
            GetBucketPolicy,
            GetBucketPolicyStatus,
            GetBucketPublicAccessBlock,
            GetBucketRequestPayment,
            GetBucketTagging,
            GetBucketVersioning,
            GetBucketWebsite,
            GetEncryptionConfiguration,
            GetInventoryConfiguration,
            GetLifecycleConfiguration,
            GetMetricsConfiguration,
            GetReplicationConfiguration,
            PutAccelerateConfiguration,
            PutAnalyticsConfiguration,
            PutBucketAcl,
            PutBucketCORS,
            PutBucketLogging,
            PutBucketNotification,
            PutBucketObjectLockConfiguration,
            PutBucketPolicy,
            PutBucketPublicAccessBlock,
            PutBucketRequestPayment,
            PutBucketTagging,
            PutBucketVersioning,
            PutBucketWebsite,
            PutEncryptionConfiguration,
            PutInventoryConfiguration,
            PutLifecycleConfiguration,
            PutMetricsConfiguration,
            PutReplicationConfiguration,
        }
    }
}
