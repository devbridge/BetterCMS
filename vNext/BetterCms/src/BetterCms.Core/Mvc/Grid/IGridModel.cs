/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System.Collections.Generic;

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
    /// Defines a grid model
    /// </summary>
    public interface IGridModel<T> where T : class
    {
        IGridRenderer<T> Renderer { get; set; }
        IList<GridColumn<T>> Columns { get; }
        IGridSections<T> Sections { get; }
        string EmptyText { get; set; }
        IDictionary<string, object> Attributes { get; set; }
        GridSortOptions SortOptions { get; set; }
        string SortPrefix { get; set; }
    }
}