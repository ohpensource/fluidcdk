using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.CDK.AWS.IAM;
using FluidCdk.Core.Contracts;

namespace FluidCdk.IAM.Grants
{
    public class GrantBase : IGrantBuilder
    {

        protected Effect GrantEffect { get; set; } 
        protected List<string> ActionList { get; set; }
        protected List<string> ResourceList { get; set; }

        public GrantBase()
        {
            ActionList = new List<string>();
            ResourceList = new List<string>();
        }

        public virtual GrantBase Actions(params string[] actions)
        {
            ActionList.AddRange(actions);
            return this;
        }

        public virtual GrantBase On(params string[] resources)
        {
            ResourceList.AddRange(resources);
            return this;
        }

        public virtual GrantBase Deny()
        {
            GrantEffect = Effect.DENY;
            return this;
        }

        public virtual PolicyStatement Build()
        {
            var props = new PolicyStatementProps
            {
                Effect = GrantEffect, 
                Actions = (ActionList.Any() ? ActionList.ToArray() : null),
                Resources = (ResourceList.Any() ? ResourceList.ToArray() : new string[] { "*" })
            };
            return new PolicyStatement(props);
        }

    }
}
