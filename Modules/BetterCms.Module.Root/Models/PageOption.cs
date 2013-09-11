using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Core.Security;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageOption : EquatableEntity<PageOption>, IOption, IAccessSecuredObjectDependency
    {
        public virtual Page Page { get; set; }

        public virtual string Value { get; set; }

        public virtual string Key { get; set; }

        public virtual OptionType Type { get; set; }

        IAccessSecuredObject IAccessSecuredObjectDependency.SecuredObject
        {
            get
            {
                return Page;
            }
        }
    }
}