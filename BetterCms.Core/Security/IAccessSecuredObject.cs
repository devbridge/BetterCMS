using System;
using System.Collections.Generic;

namespace BetterCms.Core.Security
{
    public interface IAccessSecuredObject
    {
        Guid Id { get; }

        string Title { get; }

        IList<IAccessRule> AccessRules { get; }

        void AddRule(IAccessRule accessRule);

        void RemoveRule(IAccessRule accessRule);
    }
}