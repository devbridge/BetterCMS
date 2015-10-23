/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
    /// Used as viewdata for partials rendered for RowStart/RowEnd
    /// </summary>
    public class GridRowViewData<T>
    {
        /// <summary>
        /// The current item for this row in the data source.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Whether this is an alternating row
        /// </summary>
        public bool IsAlternate { get; private set; }

        /// <summary>
        /// Creates a new instance of the GridRowViewData class.
        /// </summary>
        public GridRowViewData(T item, bool isAlternate)
        {
            Item = item;
            IsAlternate = isAlternate;
        }
    }
}