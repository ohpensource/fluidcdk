using System;
using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using AutoFixture.Xunit2;
using ImageTagger.Infra.Constructs;
using ImageTagger.Infra.UnitTest.Classes;
using Microsoft.Extensions.Configuration;
using Xunit;
using NSubstitute;

namespace ImageTagger.Infra.UnitTest
{
    public class ImageBucketBuilderTests
    {
        readonly InfraContext _infraContext;

        public ImageBucketBuilderTests()
        {
            _infraContext = new InfraContext
            {
                StackName = "stackname",
                WebApiRestApiName = "webapirestapiname",
                Region = "region",
                Account = "account",
                ImageTaggerFunctionName = "imagetaggerfunctionname",
                AssetFileFolder = "assetfilefolder",
                ImageBucketName = "imagebucketname",
                RestApiFunctionName = "restapifunctionname"
            };
        }


        [Fact]
        public void When_GetInstance_is_called()
        {
            var testStack =  new Stack(null, "testStackbucket", null);

            var sut = new ImageBucketBuilder(_infraContext);

            var result = sut.GetInstance(testStack);

            Assert.IsType<Bucket>(result);
            Assert.Equal(_infraContext.ImageBucketName, result.Node.Id );
            Assert.Equal(2, result.Node.Children.Length);
            Assert.IsType<CfnBucket>(result.Node.Children[0]);
            
        }
    }
}
