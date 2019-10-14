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
                new S3Helper.S3Permissions[]
                {
                    S3Helper.S3Permissions.GetObject,
                    S3Helper.S3Permissions.GetObjectAcl,
                    S3Helper.S3Permissions.GetObjectLegalHold,
                    S3Helper.S3Permissions.GetObjectRetention,
                    S3Helper.S3Permissions.GetObjectTagging,
                    S3Helper.S3Permissions.GetObjectTorrent,
                    S3Helper.S3Permissions.GetObjectVersion,
                    S3Helper.S3Permissions.GetObjectVersionAcl,
                    S3Helper.S3Permissions.GetObjectVersionTagging,
                    S3Helper.S3Permissions.GetObjectVersionTorrent,
                    S3Helper.S3Permissions.ListMultipartUploadParts,
                    S3Helper.S3Permissions.ListBucket,
                    S3Helper.S3Permissions.ListBucketVersions,
                    S3Helper.S3Permissions.ListAllMyBuckets,
                    S3Helper.S3Permissions.ListBucketMultipartUploads,
                }.Select(a => $"s3:{a}");
            _ = this.Actions(readonlyActions.ToArray());
            return this;
        }

        public S3Grant ReadWrite()
        {

            var readWriteActions =
                new S3Helper.S3Permissions[]
                {
                    S3Helper.S3Permissions.GetObject,
                    S3Helper.S3Permissions.GetObjectAcl,
                    S3Helper.S3Permissions.GetObjectLegalHold,
                    S3Helper.S3Permissions.GetObjectRetention,
                    S3Helper.S3Permissions.GetObjectTagging,
                    S3Helper.S3Permissions.GetObjectTorrent,
                    S3Helper.S3Permissions.GetObjectVersion,
                    S3Helper.S3Permissions.GetObjectVersionAcl,
                    S3Helper.S3Permissions.GetObjectVersionTagging,
                    S3Helper.S3Permissions.GetObjectVersionTorrent,
                    S3Helper.S3Permissions.ListMultipartUploadParts,
                    S3Helper.S3Permissions.ListBucket,
                    S3Helper.S3Permissions.ListBucketVersions,
                    S3Helper.S3Permissions.ListAllMyBuckets,
                    S3Helper.S3Permissions.ListBucketMultipartUploads,
                    S3Helper.S3Permissions.AbortMultipartUpload,
                    S3Helper.S3Permissions.BypassGovernanceRetention,
                    S3Helper.S3Permissions.DeleteObject,
                    S3Helper.S3Permissions.DeleteObjectTagging,
                    S3Helper.S3Permissions.DeleteObjectVersion,
                    S3Helper.S3Permissions.DeleteObjectVersionTagging,
                    S3Helper.S3Permissions.PutObject,
                    S3Helper.S3Permissions.PutObjectAcl,
                    S3Helper.S3Permissions.PutObjectLegalHold,
                    S3Helper.S3Permissions.PutObjectRetention,
                    S3Helper.S3Permissions.PutObjectTagging,
                    S3Helper.S3Permissions.PutObjectVersionAcl,
                    S3Helper.S3Permissions.PutObjectVersionTagging,
                    S3Helper.S3Permissions.RestoreObject,
                    S3Helper.S3Permissions.CreateBucket,
                    S3Helper.S3Permissions.DeleteBucket,
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
