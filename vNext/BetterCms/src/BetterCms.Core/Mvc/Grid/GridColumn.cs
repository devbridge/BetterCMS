/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BetterCms.Core.Mvc.Sorting;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.Framework.WebEncoders;

namespace BetterCms.Core.Mvc.Grid
{
        /// <summary>
        /// Column for the grid
        /// </summary>
        public class GridColumn<T> : IGridColumn<T> where T : class
        {
            private readonly string _name;
            private string _displayName;
            private bool _doNotSplit;
            private readonly Func<T, object> _columnValueFunc;
            private readonly Type _dataType;
            private Func<T, bool> _cellCondition = x => true;
            private string _format;
            private bool _visible = true;
            private bool _htmlEncode = true;
            private readonly IDictionary<string, object> _headerAttributes = new Dictionary<string, object>();
            private List<Func<GridRowViewData<T>, IDictionary<string, object>>> _attributes = new List<Func<GridRowViewData<T>, IDictionary<string, object>>>();
            private bool _sortable = true;
            private string _sortColumnName = null;
            private SortDirection? _initialDirection;
            private int? _position;
            private Func<object, object> _headerRenderer = x => null;

            /// <summary>
            /// Creates a new instance of the GridColumn class
            /// </summary>
            public GridColumn(Func<T, object> columnValueFunc, string name, Type type)
            {
                _name = name;
                _displayName = name;
                _dataType = type;
                _columnValueFunc = columnValueFunc;
            }

            public bool Sortable
            {
                get { return _sortable; }
            }

            public bool Visible
            {
                get { return _visible; }
            }

            public string SortColumnName
            {
                get { return _sortColumnName; }
            }

            public SortDirection? InitialDirection
            {
                get { return _initialDirection; }
            }

            /// <summary>
            /// Name of the column
            /// </summary>
            public string Name
            {
                get { return _name; }
            }

            /// <summary>
            /// Display name for the column
            /// </summary>
            public string DisplayName
            {
                get
                {
                    if (_doNotSplit)
                    {
                        return _displayName;
                    }
                    return SplitPascalCase(_displayName);
                }
            }

            /// <summary>
            /// The type of the object being rendered for thsi column. 
            /// Note: this will return null if the type cannot be inferred.
            /// </summary>
            public Type ColumnType
            {
                get { return _dataType; }
            }

            public int? Position
            {
                get { return _position; }
            }

            IGridColumn<T> IGridColumn<T>.Attributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
            {
                _attributes.Add(attributes);
                return this;
            }

            IGridColumn<T> IGridColumn<T>.Sortable(bool isColumnSortable)
            {
                _sortable = isColumnSortable;
                return this;
            }

            IGridColumn<T> IGridColumn<T>.SortColumnName(string name)
            {
                _sortColumnName = name;
                return this;
            }

            IGridColumn<T> IGridColumn<T>.SortInitialDirection(SortDirection initialDirection)
            {
                _initialDirection = initialDirection;
                return this;
            }

            /// <summary>
            /// Custom header renderer
            /// </summary>
            [Obsolete("CustomHeaderRenderer has been deprecated. Please use Header instead.")]
            public Action<RenderingContext> CustomHeaderRenderer { get; set; }

            /// <summary>
            /// Custom item renderer
            /// </summary>
            [Obsolete("CustomItemRenderer has been deprecated. Please use a custom column instead.")]
            public Action<RenderingContext, T> CustomItemRenderer { get; set; }

            IGridColumn<T> IGridColumn<T>.InsertAt(int index)
            {
                _position = index;
                return this;
            }

            /// <summary>
            /// Additional attributes for the column header
            /// </summary>
            public IDictionary<string, object> HeaderAttributes
            {
                get { return _headerAttributes; }
            }

            /// <summary>
            /// Additional attributes for the cell
            /// </summary>
            public Func<GridRowViewData<T>, IDictionary<string, object>> Attributes
            {
                get { return GetAttributesFromRow; }
            }

            private IDictionary<string, object> GetAttributesFromRow(GridRowViewData<T> row)
            {
                var dictionary = new Dictionary<string, object>();
                var pairs = _attributes.SelectMany(attributeFunc => attributeFunc(row));

                foreach (var pair in pairs)
                {
                    dictionary[pair.Key] = pair.Value;
                }

                return dictionary;
            }

            public IGridColumn<T> Named(string name)
            {
                _displayName = name;
                _doNotSplit = true;
                return this;
            }

            public IGridColumn<T> DoNotSplit()
            {
                _doNotSplit = true;
                return this;
            }

            public IGridColumn<T> Format(string format)
            {
                _format = format;
                return this;
            }

            public IGridColumn<T> CellCondition(Func<T, bool> func)
            {
                _cellCondition = func;
                return this;
            }

            IGridColumn<T> IGridColumn<T>.Visible(bool isVisible)
            {
                _visible = isVisible;
                return this;
            }

            public IGridColumn<T> Header(Func<object, object> headerRenderer)
            {
                _headerRenderer = headerRenderer;
                return this;
            }

            public IGridColumn<T> Encode(bool shouldEncode)
            {
                _htmlEncode = shouldEncode;
                return this;
            }

            //TODO: Jeremy to remove after next release
            [Obsolete("Use Encode(false) instead.")]
            public IGridColumn<T> DoNotEncode()
            {
                return Encode(false);
            }

            IGridColumn<T> IGridColumn<T>.HeaderAttributes(IDictionary<string, object> attributes)
            {
                foreach (var attribute in attributes)
                {
                    _headerAttributes.Add(attribute);
                }

                return this;
            }

            private string SplitPascalCase(string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return input;
                }
                return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            }

            /// <summary>
            /// Gets the value for a particular cell in this column
            /// </summary>
            /// <param name="instance">Instance from which the value should be obtained</param>
            /// <returns>Item to be rendered</returns>
            public object GetValue(T instance)
            {
                if (!_cellCondition(instance))
                {
                    return null;
                }

                var value = _columnValueFunc(instance);

                if (!string.IsNullOrEmpty(_format))
                {
                    value = string.Format(_format, value);
                }

                if (_htmlEncode && value != null && !(value is IHtmlContent))
                {
                    value = HtmlEncoder.Default.HtmlEncode(value.ToString());
                    //value = HttpUtility.HtmlEncode(value.ToString());
                }

                return value;
            }

            public string GetHeader()
            {
                var header = _headerRenderer(null);
                return header == null ? null : header.ToString();
            }
        }
}