using System;
using System.Collections.Generic;

namespace BetterCms.Core.Security
{
    public interface IAccessSecuredObject
    {
        Guid Id { get; set; }

        IList<IAccessRule> AccessRules { get; }

        void AddRule(IAccessRule accessRule);

        void RemoveRule(IAccessRule accessRule);
    }
}