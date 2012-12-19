using System;

namespace BetterCms.Core.Modules.Projections
{
    public class DropDownListProjectionItem
    {
        public bool IsSelected { get; set; }

        public string Value { get; set; }

        public Func<string> Text { get; set; }

        public Func<string> Tooltip { get; set; } 

        public int Order { get; set; }
    }
}
