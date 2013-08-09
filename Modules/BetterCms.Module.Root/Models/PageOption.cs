using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageOption : EquatableEntity<PageOption>, IOption
    {
        public virtual Page Page { get; set; }

        public virtual string Value { get; set; }

        public virtual string Key { get; set; }

        public virtual OptionType Type { get; set; }
    }
}