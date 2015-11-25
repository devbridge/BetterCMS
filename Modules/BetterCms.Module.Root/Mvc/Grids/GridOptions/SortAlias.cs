using BetterCms.Module.Root.Models.Enums;

namespace BetterCms.Module.Root.Mvc.Grids.GridOptions
{
    public class SortAlias
    {
        public SortAlias(string title, string column, SortDirection direction)
        {
            Title = title;
            Column = column;
            Direction = direction;
        }
        public string Title { get; set; }

        public string Column { get; set; }

        public SortDirection Direction { get; set; }
    }
}