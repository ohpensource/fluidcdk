using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using AutoFixture.Xunit2;
using ImageTagger.Infra.Constructs;
using ImageTagger.Infra.UnitTest.Classes;
using NSubstitute;
using Xunit;

namespace ImageTagger.Infra.UnitTest
{
    public class ImageTaggerStackBuilderTests
    {

        readonly Stack _testStack;
        InfraContext _infraContext;

        public ImageTaggerStackBuilderTests()
        {
            var testApp = new App();
            _testStack = new Stack(testApp, "teststack");
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

        [Theory]
        [AutoNSubstituteData]
        public void When_GetInstance_Is_Called(
            [Frozen] ITaggerFunctionBuilder functionBuilderMock,
            [Frozen] IWebAppFunctionBuilder webAppFunctionBuilderMock,
            [Frozen] IWebAppRestApiBuilder webAppRestApiBuilderMock)
        {
            var testLambda = new Function(_testStack, "test1", new FunctionProps
            {
                FunctionName = "testlambda",
                Runtime = Runtime.DOTNET_CORE_2_1,
                Handler = "aws::wwww::wwww",
                Code = Code.FromBucket(new Bucket(_testStack, "testbucket", null), "test", null)
            });


            functionBuilderMock.GetInstance(_testStack).Returns(testLambda);
            webAppFunctionBuilderMock.GetInstance(_testStack).Returns(testLambda);
            webAppRestApiBuilderMock.GetInstance(_testStack).Returns(new RestApi(_testStack, "test3"));

            var sut = new ImageTaggerStackBuilder(_infraContext, functionBuilderMock, webAppFunctionBuilderMock, webAppRestApiBuilderMock);

            var result = sut.GetInstance(_testStack);

            Assert.IsType<Stack>(result);
            Assert.NotNull(result);

            Assert.Equal(_infraContext.StackName, result.StackName);
            Assert.Equal(_infraContext.Account, result.Account);
            Assert.Equal(_infraContext.Region, result.Region);

            functionBuilderMock.ReceivedWithAnyArgs(1).GetInstance(_testStack);
            webAppFunctionBuilderMock.ReceivedWithAnyArgs(1).GetInstance(_testStack);
            webAppRestApiBuilderMock.ReceivedWithAnyArgs(1).GetInstance(_testStack);

        }
    }
}
