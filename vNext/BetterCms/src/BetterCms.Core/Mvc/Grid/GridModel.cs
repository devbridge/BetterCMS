/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ViewEngines;

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
	/// Default model for grid
	/// </summary>
	public class GridModel<T> : IGridModel<T> where T : class
    {
        private readonly ColumnBuilder<T> _columnBuilder;
        private readonly GridSections<T> _sections = new GridSections<T>();
        private IGridRenderer<T> _renderer;
        private string _emptyText;
        private IDictionary<string, object> _attributes = new Dictionary<string, object>();
        private GridSortOptions _sortOptions;
        private string _sortPrefix;

        GridSortOptions IGridModel<T>.SortOptions
        {
            get { return _sortOptions; }
            set { _sortOptions = value; }
        }

        IList<GridColumn<T>> IGridModel<T>.Columns
        {
            get { return _columnBuilder; }
        }

        IGridRenderer<T> IGridModel<T>.Renderer
        {
            get { return _renderer; }
            set { _renderer = value; }
        }

        string IGridModel<T>.EmptyText
        {
            get { return _emptyText; }
            set { _emptyText = value; }
        }

        IDictionary<string, object> IGridModel<T>.Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        string IGridModel<T>.SortPrefix
        {
            get { return _sortPrefix; }
            set { _sortPrefix = value; }
        }

        /// <summary>
        /// Creates a new instance of the GridModel class
        /// </summary>
        public GridModel(ICompositeViewEngine compositeViewEngine, IUrlHelper urlHelper, IModelMetadataProvider metadataProvider)
        {
            _emptyText = "There is no data available.";
            _columnBuilder = CreateColumnBuilder(metadataProvider);
            _renderer = new HtmlTableGridRenderer<T>(compositeViewEngine, urlHelper);
        }

        /// <summary>
        /// Column builder for this grid model
        /// </summary>
        public ColumnBuilder<T> Column
        {
            get { return _columnBuilder; }
        }

        /// <summary>
        /// Section overrides for this grid model.
        /// </summary>
        IGridSections<T> IGridModel<T>.Sections
        {
            get { return _sections; }
        }

        /// <summary>
        /// Section overrides for this grid model.
        /// </summary>
        public GridSections<T> Sections
        {
            get { return _sections; }
        }

        /// <summary>
        /// Text that will be displayed when the grid has no data.
        /// </summary>
        /// <param name="emptyText">Text to display</param>
        public void Empty(string emptyText)
        {
            _emptyText = emptyText;
        }

        /// <summary>
        /// Defines additional attributes for the grid.
        /// </summary>
        /// <param name="hash"></param>
        public void Attributes(params Func<object, object>[] hash)
        {
            Attributes(new Hash(hash));
        }

        /// <summary>
        /// Defines additional attributes for the grid
        /// </summary>
        /// <param name="attributes"></param>
        public void Attributes(IDictionary<string, object> attributes)
        {
            _attributes = attributes;
        }

        /// <summary>
        /// Specifies the Renderer to use with this grid. If omitted, the HtmlTableGridRenderer will be used. 
        /// </summary>
        /// <param name="renderer">The Renderer to use</param>
        public void RenderUsing(IGridRenderer<T> renderer)
        {
            _renderer = renderer;
        }

        /// <summary>
        /// Secifies that the grid is currently being sorted by the specified column in a particular direction.
        /// </summary>
        public void Sort(GridSortOptions sortOptions)
        {
            _sortOptions = sortOptions;
        }

        /// <summary>
        /// Specifies that the grid is currently being sorted by the specified column in a particular direction.
        /// This overload allows you to specify a prefix.
        /// </summary>
        public void Sort(GridSortOptions sortOptions, string prefix)
        {
            _sortOptions = sortOptions;
            _sortPrefix = prefix;
        }

        protected virtual ColumnBuilder<T> CreateColumnBuilder(IModelMetadataProvider metadataProvider)
        {
            return new ColumnBuilder<T>(metadataProvider);
        }
    }
}