using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.SiteSettings
{
    public class SiteSettingTemplateItemViewModel : IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string TemplateName { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, TemplateName: {2}", Id, Version, TemplateName);
        }
    }
}