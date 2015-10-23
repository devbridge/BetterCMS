/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System;
using System.Collections.Generic;

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
	/// Represents a Grid Row
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GridRow<T>
    {
        private Func<GridRowViewData<T>, IDictionary<string, object>> _attributes = x => new Dictionary<string, object>();
        private Func<GridRowViewData<T>, RenderingContext, bool> _startSectionRenderer = (x, y) => false;
        private Func<GridRowViewData<T>, RenderingContext, bool> _endSectionRenderer = (x, y) => false;

        /// <summary>
        /// Invokes the custom renderer defined (if any) for the start of the row. 
        /// Returns TRUE if custom rendering occurred (indicating that further rendering should stop) otherwise FALSE.
        /// </summary>
        public Func<GridRowViewData<T>, RenderingContext, bool> StartSectionRenderer
        {
            get { return _startSectionRenderer; }
            set { _startSectionRenderer = value; }
        }

        /// <summary>
        /// Invokes the custom renderer defined (if any) for the start of the row.
        /// Returns TRUE if custom rendering occurred (indicating that further rendering should stop) otherwise FALSE.
        /// </summary>
        public Func<GridRowViewData<T>, RenderingContext, bool> EndSectionRenderer
        {
            get { return _endSectionRenderer; }
            set { _endSectionRenderer = value; }
        }

        /// <summary>
        /// Returns custom attributes for the row.
        /// </summary>
        public Func<GridRowViewData<T>, IDictionary<string, object>> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

    }
}