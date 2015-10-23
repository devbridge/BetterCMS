/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.ApiExplorer;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace BetterCms.Core.Mvc.Grid
{
    public interface IGrid<T> : IGridWithOptions<T> where T : class
    {
        /// <summary>
        /// Specifies a custom GridModel to use.
        /// </summary>
        /// <param name="model">The GridModel storing information about this grid</param>
        /// <returns></returns>
        IGrid<T> WithModel(IGridModel<T> model);
    }

    public interface IGridWithOptions<T> : IHtmlContent where T : class
    {
        IModelMetadataProvider MetadataProvider { get; set; }

        /// <summary>
        /// The GridModel that holds the internal representation of this grid.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)] //hide from fluent interface
        IGridModel<T> Model { get; }

        /// <summary>
        /// Specifies that the grid should be rendered using a specified renderer.
        /// </summary>
        /// <param name="renderer">Renderer to use</param>
        /// <returns></returns>
        IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer);

        /// <summary>
        /// Specifies the columns to use. 
        /// </summary>
        /// <param name="columnBuilder"></param>
        /// <returns></returns>
        IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder);

        /// <summary>
        /// Text to render when grid is empty.
        /// </summary>
        /// <param name="emptyText">Empty Text</param>
        /// <returns></returns>
        IGridWithOptions<T> Empty(string emptyText);

        /// <summary>
        /// Additional custom attributes
        /// </summary>
        /// <returns></returns>
        IGridWithOptions<T> Attributes(IDictionary<string, object> attributes);

        /// <summary>
        /// Additional custom attributes for each row
        /// </summary>
        /// <param name="attributes">Lambda expression that returns custom attributes for each row</param>
        /// <returns></returns>
        IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes);

        /// <summary>
        /// Additional custom attributes for the header row.
        /// </summary>
        /// <param name="attributes">Attributes for the header row</param>
        /// <returns></returns>
        IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes);


        /// <summary>
        /// Specifies that the grid is currently sorted
        /// </summary>
        IGridWithOptions<T> Sort(GridSortOptions sortOptions);

        /// <summary>
        /// Specifies that the grid is sorted. Column links will have the specified prefix prepended.
        /// </summary>
        IGridWithOptions<T> Sort(GridSortOptions sortOptions, string prefix);

        /// <summary>
        /// Renders the grid to the TextWriter specified at creation
        /// </summary>
        /// <returns></returns>
        void Render();

    }
}