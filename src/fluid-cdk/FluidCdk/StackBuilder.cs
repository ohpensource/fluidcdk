using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK;
using FluidCdk.Core.Contracts;

namespace FluidCdk.Core
{
    public interface IStackBuilder : IConstructBuilder<Stack> {}

    public class StackBuilder : ConstructBuilderBase<Stack>, IStackBuilder
    {
        readonly StackProps _props = new StackProps();
        readonly List<IConstructBuilder<Construct>> _constructs= new List<IConstructBuilder<Construct>>();

        public StackBuilder() {}

        public StackBuilder(string name)
        {
            _props.StackName = name;
        }

        public StackBuilder SetName(string name)
        {
            _props.StackName = name;
            return this;
        }

        public StackBuilder SetEnv(string account, string region)
        {
            _props.Env = new Amazon.CDK.Environment
            {
                Account = account,
                Region = region
            };
            return this;
        }

        public StackBuilder AddTags(IDictionary<string, string> tags)
        {
            foreach (var keyValuePair in tags)
                AddTag(keyValuePair.Key, keyValuePair.Value);

            return this;
        }

        public StackBuilder AddTag(string key, string value)
        {
            if (_props.Tags == null) _props.Tags = new Dictionary<string, string>();
            _props.Tags.Add(key,value);
            return this;
        }

        public StackBuilder Add(IConstructBuilder<Construct> construct)
        {
            _constructs.Add(construct);
            return this;
        }

        protected override Stack Build(Construct scope)
        {
            var stack = new Stack(scope, _props.StackName, _props);
            _constructs.ForEach(c => c.GetInstance(stack));
            return stack;
        }
    }
}
