using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.CDK;
using FluidCdk.Core.Contracts;

namespace FluidCdk.Core
{
    public abstract class ConstructBuilderBase<T> : IConstructBuilder<T> where T : Construct
    {

        protected T Instance { get; set; }

        protected abstract T Build(Construct scope);

        public T GetInstance(Construct scope)
        {
            if (Instance != null)
                return Instance;

            return Instance = Build(scope);

        }
    }
}
