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

        /// <summary>
        /// Gets a value indicating whether entity should be saved without checking object security.
        /// </summary>
        /// <value>
        ///   <c>true</c> if entity can be saved unsecured; otherwise, <c>false</c>.
        /// </value>
        bool SaveUnsecured { get; }
    }
}