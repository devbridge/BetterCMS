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

        public virtual CustomOption CustomOption { get; set; }

        IAccessSecuredObject IAccessSecuredObjectDependency.SecuredObject
        {
            get
            {
                return Page;
            }
        }

        ICustomOption IOption.CustomOption
        {
            get
            {
                return CustomOption;
            }
            set
            {
                CustomOption = (CustomOption)value;
            }
        }
    }
}