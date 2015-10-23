/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.Mvc.Rendering;

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
    /// A renderer responsible for rendering a grid.
    /// </summary>
	public interface IGridRenderer<T> where T : class
    {
        /// <summary>
        /// Renders a grid
        /// </summary>
        /// <param name="gridModel">The grid model to render</param>
        /// <param name="dataSource">Data source for the grid</param>
        /// <param name="output">The TextWriter to which the grid should be rendered/</param>
        /// <param name="viewContext">View context</param>
        void Render(IGridModel<T> gridModel, IEnumerable<T> dataSource, TextWriter output, ViewContext viewContext);
    }
}