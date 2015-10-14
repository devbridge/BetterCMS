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

        public override string ToString()
        {
            return string.Format("{0}, Key: {1}, Name: {2}, Value: {3}, Id: {4}", 
                base.ToString(), Key, Name, Value, Id);
        }
    }
}