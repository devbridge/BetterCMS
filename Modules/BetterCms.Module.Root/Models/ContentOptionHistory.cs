using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentOptionHistory : EquatableEntity<ContentOptionHistory>, IContentOption
    {
        public virtual ContentHistory ContentHistory { get; set; }

        public virtual string Key { get; set; }

        public virtual ContentOptionType Type { get; set; }

        public virtual string DefaultValue { get; set; }

        public IContent Content
        {
            get
            {
                return ContentHistory;
            }
        }
    }
}