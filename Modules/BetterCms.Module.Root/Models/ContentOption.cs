using System;
using System.Collections.Generic;

using BetterCms.Api.Interfaces.Models;
using BetterCms.Api.Interfaces.Models.Enums;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentOption : EquatableEntity<ContentOption>, IContentOption
    {
        public virtual Content Content { get; set; }

        public virtual string Key { get; set; }

        public virtual OptionType Type { get; set; }

        public virtual string DefaultValue { get; set; }        

        IContent IContentOption.Content
        {
            get
            {
                return Content;
            }
        }

        string IOption.Value
        {
            get
            {
                return DefaultValue;
            }
        }
    }
}