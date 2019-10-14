using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;

namespace ImageTagger.Infra.UnitTest.Classes
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        public AutoNSubstituteDataAttribute()
            : base(() =>
            {
                var myfixture = new Fixture().Customize(new AutoNSubstituteCustomization());
                myfixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => myfixture.Behaviors.Remove(b));
                myfixture.Behaviors.Add(new OmitOnRecursionBehavior());
                return myfixture;
            })
        {
        }
    }
}
