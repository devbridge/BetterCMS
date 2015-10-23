/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
    /// Sections for a grid.
    /// </summary>
    public class GridSections<T> : IGridSections<T> where T : class
    {
        private GridRow<T> _row = new GridRow<T>();
        private GridRow<T> _headerRow = new GridRow<T>();

        GridRow<T> IGridSections<T>.Row
        {
            get { return _row; }
        }

        GridRow<T> IGridSections<T>.HeaderRow
        {
            get { return _headerRow; }
        }
    }

    public interface IGridSections<T> where T : class
    {
        GridRow<T> Row { get; }
        GridRow<T> HeaderRow { get; }
    }
}