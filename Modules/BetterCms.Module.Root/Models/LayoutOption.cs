using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class LayoutOption : EquatableEntity<LayoutOption>, IDeletableOption<Layout>
    {
        public LayoutOption()
        {
            IsDeletable = true;
        }

        public virtual Layout Layout { get; set; }

        public virtual string Key { get; set; }

        public virtual OptionType Type { get; set; }

        public virtual string DefaultValue { get; set; }
        
        public virtual bool IsDeletable { get; set; }

        string IOption.Value
        {
            get
            {
                return DefaultValue;
            }
            set
            {
                DefaultValue = value;
            }
        }
        
        Layout IDeletableOption<Layout>.Entity
        {
            get
            {
                return Layout;
            }
            set
            {
                Layout = value;
            }
        }
    }
}