using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Helpers
{
    /// <summary>
    /// Helper class for creating filtering and ordering queries from DataOptions instance
    /// </summary>
    public class DataOptionsQueryCreator<TModel>
    {
        /// <summary>
        /// Order constants for generating a query 
        /// </summary>
        private static class OrderConstants
        {
            public const string Descending = " desc";
        }

        /// <summary>
        /// Filter constants for generating a query 
        /// </summary>
        private static class FilterConstants
        {
            public const string And = " and ";
            public const string Or = " or ";
            public const string Contains = "{0}.Contains(@{1})";
            public const string NotContains = "(!{0}.Contains(@{1}) or {0} == null)";
            public const string StartsWith = "{0}.StartsWith(@{1})";
            public const string EndsWith = "{0}.EndsWith(@{1})";
            public const string Equal = "{0} == @{1}";
            public const string EqualNull = "{0} == null";
            public const string NotEqual = "({0} != @{1} or {0} == null)";
            public const string NotEqualNull = "{0} != null";
            public const string Greater = "{0} > @{1}";
            public const string GreaterOrEqual = "{0} >= @{1}";
            public const string Less = "{0} < @{1}";
            public const string LessOrEqual = "{0} <= @{1}";
        }

        private readonly DataOptions dataOptions;

        private readonly IList<object> filterParameters;

        private readonly Type modelType = typeof(TModel);

        private string orderQuery;

        private string filterQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataOptionsQueryCreator{TModel}" /> class.
        /// </summary>
        /// <param name="dataOptions">The data options.</param>
        public DataOptionsQueryCreator(DataOptions dataOptions)
        {
            this.dataOptions = dataOptions;

            filterParameters = new List<object>();
        }

        /// <summary>
        /// Gets the order query.
        /// </summary>
        /// <returns>Filter query, generated from data options</returns>
        public string GetOrderQuery()
        {
            if (orderQuery == null)
            {
                CreateOrderQuery();
            }

            return orderQuery;
        }

        /// <summary>
        /// Gets the filter query.
        /// </summary>
        /// <returns>Order query, generated from data options</returns>
        public string GetFilterQuery()
        {
            if (filterQuery == null)
            {
                CreateFilterQuery();
            }

            return filterQuery;
        }

        /// <summary>
        /// Gets the filter parameters.
        /// </summary>
        /// <returns>Array with parameters for filtering.</returns>
        public object[] GetFilterParameters()
        {
            return filterParameters.ToArray();
        }

        /// <summary>
        /// Creates the order query data options.
        /// </summary>
        private void CreateOrderQuery()
        {
            var sb = new StringBuilder(string.Empty);

            if (dataOptions.Order != null && dataOptions.Order.By != null && dataOptions.Order.By.Count > 0)
            {
                for (var i = 0; i < dataOptions.Order.By.Count; i++)
                {
                    var item = dataOptions.Order.By[i];
                    var fieldName = item.Field;

                    ValidateProperty(fieldName);

                    if (i > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(fieldName);

                    if (item.OrderByDescending)
                    {
                        sb.Append(OrderConstants.Descending);
                    }
                }
            }

            orderQuery = sb.ToString();
        }

        /// <summary>
        /// Creates the filter query from data options.
        /// </summary>
        private void CreateFilterQuery()
        {
            var sb = new StringBuilder(string.Empty);
            filterParameters.Clear();

            if (dataOptions.Filter != null &&
                ((dataOptions.Filter.Where != null && dataOptions.Filter.Where.Count > 0)
                    || (dataOptions.Filter.Inner != null && dataOptions.Filter.Inner.Count > 0)))
            {
                var query = CreateQueryJunction(dataOptions.Filter);
                sb.Append(query);
            }

            filterQuery = sb.ToString();
        }

        /// <summary>
        /// Appends the filter connector to query.
        /// </summary>
        /// <param name="sb">The string builder instance.</param>
        /// <param name="filterConnector">The filter connector.</param>
        private void AppendFilterConnector(StringBuilder sb, FilterConnector filterConnector)
        {
            if (string.IsNullOrWhiteSpace(sb.ToString()))
            {
                return;
            }

            string connector = filterConnector == FilterConnector.And
                ? FilterConstants.And
                : FilterConstants.Or;

            sb.Append(connector);
        }

        /// <summary>
        /// Creates the query expression.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        /// <param name="filterConnector">The filter connector.</param>
        /// <returns>Generated query expression</returns>
        private string CreateQueryExpression(FilterItem filterItem, FilterConnector filterConnector)
        {
            var sb = new StringBuilder();
            var propertyName = filterItem.Field;

            var property = GetAndValidateProperty(propertyName);

            var parameterNr = filterParameters.Count;
            var value = GetValue(filterItem, property);
            filterParameters.Add(value);
            AppendFilterConnector(sb, filterConnector);

            string query;
            switch (filterItem.Operation)
            {
                case FilterOperation.Contains:
                    query = string.Format(FilterConstants.Contains, propertyName, parameterNr);
                    break;
                case FilterOperation.NotContains:
                    query = string.Format(FilterConstants.NotContains, propertyName, parameterNr);
                    break;
                case FilterOperation.StartsWith:
                    query = string.Format(FilterConstants.StartsWith, propertyName, parameterNr);
                    break;
                case FilterOperation.EndsWith:
                    query = string.Format(FilterConstants.EndsWith, propertyName, parameterNr);
                    break;
                case FilterOperation.Equal:
                    if (value == null)
                    {
                        query = string.Format(FilterConstants.EqualNull, propertyName);
                        break;
                    }
                    query = string.Format(FilterConstants.Equal, propertyName, parameterNr);
                    break;
                case FilterOperation.NotEqual:
                    if (value == null)
                    {
                        query = string.Format(FilterConstants.NotEqualNull, propertyName);
                        break;
                    }
                    query = string.Format(FilterConstants.NotEqual, propertyName, parameterNr);
                    break;
                case FilterOperation.Greater:
                    query = string.Format(FilterConstants.Greater, propertyName, parameterNr);
                    break;
                case FilterOperation.GreaterOrEqual:
                    query = string.Format(FilterConstants.GreaterOrEqual, propertyName, parameterNr);
                    break;
                case FilterOperation.Less:
                    query = string.Format(FilterConstants.Less, propertyName, parameterNr);
                    break;
                case FilterOperation.LessOrEqual:
                    query = string.Format(FilterConstants.LessOrEqual, propertyName, parameterNr);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown filter operation: {0}!", filterItem.Operation));
            }

            sb.Append(query);
            return sb.ToString();
        }

        /// <summary>
        /// Creates the query conjunction or disjunction.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        private string CreateQueryJunction(DataFilter filter)
        {
            var sb = new StringBuilder(string.Empty);

            foreach (var filterItem in filter.Where)
            {
                var subQuery = CreateQueryExpression(filterItem, filter.Connector);
                AppendFilterConnector(sb, filter.Connector);
                sb.Append(subQuery);
            }

            if (filter.Inner != null && filter.Inner.Any())
            {
                foreach (var innerFilter in filter.Inner)
                {
                    var subQuery = CreateQueryJunction(innerFilter);
                    AppendFilterConnector(sb, filter.Connector);
                    sb.Append("(").Append(subQuery).Append(")");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Validates the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void ValidateProperty(string propertyName)
        {
            GetAndValidateProperty(propertyName);
        }

        /// <summary>
        /// Gets the and validates property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Reflection property info object</returns>
        /// <exception cref="System.InvalidOperationException">Model has no such property</exception>
        private System.Reflection.PropertyInfo GetAndValidateProperty(string propertyName)
        {
            var property = modelType.GetProperty(propertyName);

            if (property == null)
            {
                property = modelType.GetProperty(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(propertyName));

                if (property == null)
                {
                    throw new InvalidOperationException(string.Format("Property {0} doesn't exist in object {1}", propertyName, modelType));
                }
            }

            return property;
        }

        /// <summary>
        /// Gets the value, converted to property's type.
        /// </summary>
        /// <param name="filterItem">The filter item.</param>
        /// <param name="property">The property.</param>
        /// <returns>Value, converted to property's type</returns>
        private object GetValue(FilterItem filterItem, System.Reflection.PropertyInfo property)
        {
            // Return value if object is not string (no need casting)
            if (!(filterItem.Value is string))
            {
                return filterItem.Value;
            }

            // Try to cast to different type of objects
            var value = filterItem.Value.ToString();

            Type type = property.PropertyType;

            if (typeof(string).IsAssignableFrom(type))
            {
                return value;
            }

            if (typeof(Enum).IsAssignableFrom(type))
            {
                return ConvertToEnum(type, value);
            }

            if (typeof(int?).IsAssignableFrom(type)
                || typeof(short?).IsAssignableFrom(type))
            {
                return Convert.ToInt32(value);
            }

            if (typeof(long?).IsAssignableFrom(type))
            {
                return Convert.ToInt64(value);
            }

            if (typeof(DateTime?).IsAssignableFrom(type))
            {
                return Convert.ToDateTime(value);
            }

            if (typeof(Guid?).IsAssignableFrom(type))
            {
                return new Guid(value);
            }

            if (typeof(bool?).IsAssignableFrom(type))
            {
                return Convert.ToBoolean(value);
            }

            return value;
        }

        /// <summary>
        /// Converts value to enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private object ConvertToEnum(Type enumType, object value)
        {
            if (!typeof(Enum).IsAssignableFrom(enumType))
            {
                throw new NotSupportedException(string.Format("Failed to convert value {0} to specied enum type {1}.", enumType, value));
            }

            if (value is string)
            {
                var parsedValue = Enum.Parse(enumType, value as string);
                return parsedValue;
            }

            if (value is short? || value is int? || value is long?)
            {
                var parsedValue = Enum.ToObject(enumType, value);
                return parsedValue;
            }

            throw new NotSupportedException(string.Format("Failed to convert value {0} to specied enum type {1}.", enumType, value));
        }
    }
}