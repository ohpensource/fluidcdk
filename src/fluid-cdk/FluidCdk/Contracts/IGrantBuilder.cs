using Amazon.CDK.AWS.IAM;

namespace FluidCdk.Core.Contracts
{
    public interface IGrantBuilder
    {
        PolicyStatement Build();
    }
}
