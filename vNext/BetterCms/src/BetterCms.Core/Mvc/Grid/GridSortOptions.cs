/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using BetterCms.Core.Mvc.Sorting;

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
    /// Sorting information for use with the grid.
    /// </summary>
    public class GridSortOptions
    {
        public string Column { get; set; }
        public SortDirection Direction { get; set; }
    }
}