using System;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Blog.ViewModels.Setting
{
    public class SettingItemViewModel : IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }
    }
}