using Amazon.CDK;

namespace FluidCdk.Core.Contracts
{
    public interface IConstructBuilder { }

    public interface IConstructBuilder<out T> : IConstructBuilder where T : Construct
    {

        T GetInstance(Construct scope);
    }
}