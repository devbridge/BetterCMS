using System;

using BetterCms.Api.Interfaces.Models;
using BetterCms.Api.Interfaces.Models.Enums;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageContentOption : EquatableEntity<PageContentOption>, IPageContentOption
    {
        public virtual PageContent PageContent { get; set; }

        public virtual string Value { get; set; }

        public virtual string Key { get; set; }

        public virtual OptionType Type { get; set; }

        IPageContent IPageContentOption.PageContent
        {
            get
            {
                return PageContent;
            }
        }
    }
}