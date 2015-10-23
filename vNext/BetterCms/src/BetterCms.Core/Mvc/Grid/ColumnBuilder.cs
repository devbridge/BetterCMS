/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
	/// Builds grid columns
	/// </summary>
	public class ColumnBuilder<T> : IList<GridColumn<T>> where T : class
    {
        private readonly IModelMetadataProvider _metadataProvider;
        private readonly List<GridColumn<T>> _columns = new List<GridColumn<T>>();

        public ColumnBuilder(IModelMetadataProvider metadataProvider)
        {
            _metadataProvider = metadataProvider;
        }


        /// <summary>
        /// Creates a column for custom markup.
        /// </summary>
        public IGridColumn<T> Custom(Func<T, object> customRenderer)
        {
            var column = new GridColumn<T>(customRenderer, "", typeof(object));
            column.Encode(false);
            Add(column);
            return column;
        }

        /// <summary>
        /// Specifies a column should be constructed for the specified property.
        /// </summary>
        /// <param name="propertySpecifier">Lambda that specifies the property for which a column should be constructed</param>
        public IGridColumn<T> For(Expression<Func<T, object>> propertySpecifier)
        {
            var memberExpression = GetMemberExpression(propertySpecifier);
            var propertyType = GetTypeFromMemberExpression(memberExpression);
            var declaringType = memberExpression?.Expression.Type;
            var inferredName = memberExpression?.Member.Name;
            var column = new GridColumn<T>(propertySpecifier.Compile(), inferredName, propertyType);

            if (declaringType != null)
            {
                var metadata = _metadataProvider.GetMetadataForProperty(declaringType, inferredName);

                if (!string.IsNullOrEmpty(metadata.DisplayName))
                {
                    column.Named(metadata.DisplayName);
                }

                if (!string.IsNullOrEmpty(metadata.DisplayFormatString))
                {
                    column.Format(metadata.DisplayFormatString);
                }
            }

            Add(column);

            return column;
        }

        protected IList<GridColumn<T>> Columns
        {
            get { return _columns; }
        }

        public IEnumerator<GridColumn<T>> GetEnumerator()
        {
            return _columns
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static MemberExpression GetMemberExpression(LambdaExpression expression)
        {
            return RemoveUnary(expression.Body) as MemberExpression;
        }

        private static Type GetTypeFromMemberExpression(MemberExpression memberExpression)
        {
            if (memberExpression == null) return null;

            var dataType = GetTypeFromMemberInfo(memberExpression.Member, (PropertyInfo p) => p.PropertyType);
            if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (MethodInfo m) => m.ReturnType);
            if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (FieldInfo f) => f.FieldType);

            return dataType;
        }

        private static Type GetTypeFromMemberInfo<TMember>(MemberInfo member, Func<TMember, Type> func) where TMember : MemberInfo
        {
            if (member is TMember)
            {
                return func((TMember)member);
            }
            return null;
        }

        private static Expression RemoveUnary(Expression body)
        {
            var unary = body as UnaryExpression;
            if (unary != null)
            {
                return unary.Operand;
            }
            return body;
        }

        protected virtual void Add(GridColumn<T> column)
        {
            _columns.Add(column);
        }

        void ICollection<GridColumn<T>>.Add(GridColumn<T> column)
        {
            Add(column);
        }

        void ICollection<GridColumn<T>>.Clear()
        {
            _columns.Clear();
        }

        bool ICollection<GridColumn<T>>.Contains(GridColumn<T> column)
        {
            return _columns.Contains(column);
        }

        void ICollection<GridColumn<T>>.CopyTo(GridColumn<T>[] array, int arrayIndex)
        {
            _columns.CopyTo(array, arrayIndex);
        }

        bool ICollection<GridColumn<T>>.Remove(GridColumn<T> column)
        {
            return _columns.Remove(column);
        }

        int ICollection<GridColumn<T>>.Count
        {
            get { return _columns.Count; }
        }

        bool ICollection<GridColumn<T>>.IsReadOnly
        {
            get { return false; }
        }

        int IList<GridColumn<T>>.IndexOf(GridColumn<T> item)
        {
            return _columns.IndexOf(item);
        }

        void IList<GridColumn<T>>.Insert(int index, GridColumn<T> item)
        {
            _columns.Insert(index, item);
        }

        void IList<GridColumn<T>>.RemoveAt(int index)
        {
            _columns.RemoveAt(index);
        }

        GridColumn<T> IList<GridColumn<T>>.this[int index]
        {
            get { return _columns[index]; }
            set { _columns[index] = value; }
        }
    }
}