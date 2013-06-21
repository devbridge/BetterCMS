using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using Autofac;

using BetterCms.Api;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Services;

using NUnit.Framework;

using System.Linq.Dynamic;

using DynamicExpression = System.Linq.Dynamic.DynamicExpression;
using LinqExpression = System.Linq.Expressions.Expression;

namespace BetterCms.Test.Module.Blog.ModelTests.MapTests
{
    [TestFixture]
    public class AuthorMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Author_Successfully()
        {
            var content = TestDataProvider.CreateNewAuthor();
            RunEntityMapTestsInTransaction(content);  
        }
        
        private DataOptions CreateTestDataOptions()
        {
            var options = new DataOptions(5, 3);
            options.Filter.Add("CreatedOn", new DateTime(2013, 06, 01), FilterOperation.Greater);
            options.Filter.Add("Title", "Africa", FilterOperation.NotEqual);

            var subFilter = new DataFilter(FilterConnector.Or);
            subFilter.Add("Title", "It", FilterOperation.StartsWith);
            subFilter.Add("Title", "Af", FilterOperation.StartsWith);
            subFilter.Add("Title", "na", FilterOperation.EndsWith);
            subFilter.Add("Title", "Spain");

            options.Filter.InnerFilters.Add(subFilter);

            options.Order.Add("CreatedOn");
            options.Order.Add("Title", OrderDirection.Desc);

            return options;
        }

        private void AreFiltersEqual(DataFilter filter1, DataFilter filter2)
        {
            Assert.AreEqual(filter1.Connector, filter2.Connector);

            Assert.AreEqual(filter1.FilterItems.Count, filter2.FilterItems.Count);
            Assert.AreEqual(filter1.InnerFilters.Count, filter2.InnerFilters.Count);

            for (var i = 0; i < filter1.FilterItems.Count; i++)
            {
                var item1 = filter1.FilterItems[i];
                var item2 = filter1.FilterItems[i];

                Assert.AreEqual(item1.Field, item2.Field);
                Assert.AreEqual(item1.Operation, item2.Operation);
                Assert.AreEqual(item1.Value, item2.Value);
            }

            for (var i = 0; i < filter1.InnerFilters.Count; i++)
            {
                AreFiltersEqual(filter1.InnerFilters[i], filter2.InnerFilters[i]);
            }
        }

        private void AreOrdersEqual(DataOrder order1, DataOrder order2)
        {
            Assert.AreEqual(order1.OrderItems.Count, order2.OrderItems.Count);

            for (var i = 0; i < order1.OrderItems.Count; i++)
            {
                var item1 = order1.OrderItems[0];
                var item2 = order2.OrderItems[0];

                Assert.AreEqual(item1.Field, item2.Field);
                Assert.AreEqual(item1.Direction, item2.Direction);
                Assert.AreEqual(item1.OrderByDescending, item2.OrderByDescending);
            }
        }

        [Test]
        public void SerializeOptions()
        {
            var options = CreateTestDataOptions();
            var serializer = new JavaScriptSerializer();
            
            string serialized =  serializer.Serialize(options);
            Console.WriteLine(serialized);

            var deserialized = serializer.Deserialize<DataOptions>(serialized);

            Assert.IsNotNull(options);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(options.ItemsCount, deserialized.ItemsCount);
            Assert.AreEqual(options.StartItemNumber, deserialized.StartItemNumber);

            AreFiltersEqual(options.Filter, deserialized.Filter);
            AreOrdersEqual(options.Order, deserialized.Order);
        }
        
        [Test]
        public void DeserializeOptions()
        {
            RunActionInTransaction(
                session =>
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("{");
                        sb.AppendLine("    \"Filter\": {");
                        sb.AppendLine("        \"FilterItems\": [");
                        sb.AppendLine("            { \"Field\": \"CreatedOn\", \"Value\": \"\\/Date(1224043200000)\\/\", \"Operation\": \"Greater\" }");
                        sb.AppendLine("            , { \"Field\": \"Title\", \"Value\": \"Africa\", \"Operation\": \"Equal\" }");
                        sb.AppendLine("        ],");
                        sb.AppendLine("        \"InnerFilters\": [");
                        sb.AppendLine("            {");
                        sb.AppendLine("                \"Connector\": \"Or\",");
                        sb.AppendLine("                \"FilterItems\": [");
                        sb.AppendLine("                    { \"Field\": \"Title\", \"Value\": \"It\", \"Operation\": \"StartsWith\" }");
                        sb.AppendLine("                    , { \"Field\": \"Title\", \"Value\": \"na\", \"Operation\": \"EndsWith\" }");
                        sb.AppendLine("                ]");
                        sb.AppendLine("            }");
                        sb.AppendLine("       ]");
                        sb.AppendLine("    },");
                        sb.AppendLine("   \"Order\": {");
                        sb.AppendLine("        \"OrderItems\": [");
                        sb.AppendLine("            { \"Field\": \"CreatedOn\" }");
                        sb.AppendLine("            , { \"Field\": \"Title\", \"Operation\": \"Desc\" }");
                        sb.AppendLine("        ]");
                        sb.AppendLine("    },");
                        sb.AppendLine("    \"StartItemNumber\": 3,");
                        sb.AppendLine("    \"ItemsCount\": 5");
                        sb.AppendLine("}");

                        var serializer = new JavaScriptSerializer();
                        var json = sb.ToString();

                        Console.WriteLine(json);

                        var options = serializer.Deserialize<DataOptions>(json);

                        Assert.IsNotNull(options);
                        Assert.AreEqual(options.Filter.FilterItems.Count, 2);
                        Assert.AreEqual(options.Filter.FilterItems[1].Field, "Title");
                        Assert.AreEqual(options.Filter.FilterItems[1].Value, "Africa");
                        Assert.AreEqual(options.Filter.FilterItems[1].Operation, FilterOperation.Equal);

                        Assert.AreEqual(options.Filter.InnerFilters.Count, 1);
                        Assert.AreEqual(options.Filter.InnerFilters[0].Connector, FilterConnector.Or);

                        Assert.AreEqual(options.Filter.InnerFilters[0].FilterItems.Count, 2);
                        Assert.AreEqual(options.Filter.InnerFilters[0].FilterItems[0].Field, "Title");
                        Assert.AreEqual(options.Filter.InnerFilters[0].FilterItems[0].Value, "It");
                        Assert.AreEqual(options.Filter.InnerFilters[0].FilterItems[0].Operation, FilterOperation.StartsWith);
                    });
        }

        [Test]
        public void DynamicLINQTest1()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var results = repository
                    .AsQueryable<BlogPost>()
                    .Where("CreatedOn > @0 and Title != @1 and (Title.StartsWith(@2) or Title.StartsWith(@3) or Title.EndsWith(@4))", 
                        new DateTime(2013, 06, 01), 
                        "Africa", 
                        "Af", 
                        "It", 
                        "na")
                    .Where(b => b.Version == 1)
                    .OrderBy("CreatedOn desc, Title")
                    .Skip(2)
                    .Take(3)
                    .ToList();

                Console.WriteLine("Count: {0}", results.Count);
                Console.WriteLine(results);
            });
        }

        [Test]
        public void TestLambdaWhereClause()
        {
            var whereClause = "CreatedOn > @0 and Title != @1 and (Title.StartsWith(@2) or Title.StartsWith(@3) or Title.EndsWith(@4))";

            RunActionInTransaction(
                session =>
                    {
                        var unitOfWork = new DefaultUnitOfWork(session);
                        var repository = new DefaultRepository(unitOfWork);

                        var values = new object[] { new DateTime(2013, 06, 01), "Africa", "Af", "It", "na" };
                        var filter = DynamicExpression.ParseLambda<BlogPostModel, bool>(whereClause, values);

                        var request = new GetBlogPostsRequest(filter);

                        var blogService = new DefaultBlogService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IUrlService>(), repository);
                        using (var api = new BlogsApiContext(Container.BeginLifetimeScope(), null, blogService, null, repository))
                        {
                            var results = api.GetBlogPosts(request);

                            Assert.NotNull(results);
                            Assert.AreEqual(results.TotalCount, 10);
                            Assert.AreEqual(results.Items.Any(b => b.Title == "Italy"), true);
                            Assert.AreEqual(results.Items.Any(b => b.Title == "Argentina"), true);
                            Assert.AreEqual(results.Items.Any(b => b.Title.StartsWith("Af")), true);
                            
                            Assert.AreEqual(results.Items.Any(b => b.Title == "Africa"), false);
                            Assert.AreEqual(results.Items.Any(b => b.Title == "Spain"), false);
                            Assert.AreEqual(results.Items.Any(b => b.Title == "Latvia"), false);
                        }
                    });
        }
        
        [Test]
        public void TestLambdaOrderClause()
        {
            var orderByClause = "AuthorName, CreatedOn desc, Title";

            RunActionInTransaction(
                session =>
                {
                    var unitOfWork = new DefaultUnitOfWork(session);
                    var repository = new DefaultRepository(unitOfWork);

                    var request = new GetBlogPostsRequest(itemsCount:5);

                    var parameters = new[] { LinqExpression.Parameter(typeof(BlogPostModel), "") };
                    ExpressionParser parser = new ExpressionParser(parameters, orderByClause, new object[0]);
                    var orderings = parser.ParseOrdering();
                    foreach (var order in orderings)
                    {
                        var expression = order.Selector;
                        var conversion = LinqExpression.Convert(expression, typeof(object));

                        var lambda = System.Linq.Expressions.Expression.Lambda<Func<BlogPostModel, object>>(conversion, parameters);
                        request.AddOrder(lambda, !order.Ascending);
                    }

                    var blogService = new DefaultBlogService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IUrlService>(), repository);
                    using (var api = new BlogsApiContext(Container.BeginLifetimeScope(), null, blogService, null, repository))
                    {
                        var results = api.GetBlogPosts(request);

                        Assert.NotNull(results);
                        Assert.AreEqual(results.Items.Count, 5);
                        Assert.AreEqual(results.Items.Any(b => b.Title == "Yemen"), true);
                        Assert.AreEqual(results.Items.Any(b => b.Title == "Zambia"), true);
                        Assert.AreEqual(results.Items.Any(b => b.Title == "Africa"), false);
                    }
                });
        }
    }

    /// <summary>
    /// Represents container for data filter, order and paging information
    /// </summary>
    [Serializable]
    public class DataOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataOptions" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public DataOptions()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataOptions" /> class.
        /// </summary>
        /// <param name="itemsCount">The maximum count of returning items.</param>
        /// <param name="startItemNumber">The starting item number.</param>
        public DataOptions(int? itemsCount,
            int startItemNumber = 1)
        {
            Filter = new DataFilter();
            Order = new DataOrder();

            StartItemNumber = startItemNumber;
            ItemsCount = itemsCount;
        }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public DataFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public DataOrder Order { get; set; }

        /// <summary>
        /// Gets or sets the starting item number.
        /// </summary>
        /// <value>
        /// The starting item number.
        /// </value>
        public int StartItemNumber { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of returning items.
        /// </summary>
        /// <value>
        /// The maximum count of returning items.
        /// </value>
        public int? ItemsCount { get; set; }
    }

    /// <summary>
    /// Ordering directions enum
    /// </summary>
    public enum OrderDirection
    {
        Asc,
        Desc
    }

    /// <summary>
    /// Filtering operations enum
    /// </summary>
    public enum FilterOperation
    {
        Equal,
        NotEqual,
        
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,

        Contains,
        NotContains,
        StartsWith,
        EndsWith
    }

    /// <summary>
    /// Filtering connectors enum
    /// </summary>
    public enum FilterConnector
    {
        And,
        Or
    }

    /// <summary>
    /// Represents container for filtering items list
    /// </summary>
    [Serializable]
    public class DataFilter
    {
        /// <summary>
        /// Gets or sets the filter connector.
        /// </summary>
        /// <value>
        /// The filter connector.
        /// </value>
        public FilterConnector Connector { get; set; }

        /// <summary>
        /// Gets or sets the list filter items.
        /// </summary>
        /// <value>
        /// The list of filter items.
        /// </value>
        public IList<FilterItem> FilterItems { get; set; }

        /// <summary>
        /// Gets or sets the list of inner filters.
        /// </summary>
        /// <value>
        /// The list of inner filters.
        /// </value>
        public IList<DataFilter> InnerFilters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFilter" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public DataFilter()
            : this(FilterConnector.And)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFilter" /> class.
        /// </summary>
        /// <param name="connector">The filter connector.</param>
        public DataFilter(FilterConnector connector)
        {
            FilterItems = new List<FilterItem>();
            InnerFilters = new List<DataFilter>();
            Connector = connector;
        }

        /// <summary>
        /// Adds the specified filtering field.
        /// </summary>
        /// <param name="field">The filtering field.</param>
        /// <param name="value">The filtering value.</param>
        /// <param name="operation">The filtering operation.</param>
        public void Add(string field, object value, FilterOperation operation = FilterOperation.Equal)
        {
            var filterItem = new FilterItem(field, value, operation);

            FilterItems.Add(filterItem);
        }
    }

    /// <summary>
    /// Represents class for filtering items
    /// </summary>
    [Serializable]
    public class FilterItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterItem" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public FilterItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterItem" /> class.
        /// </summary>
        /// <param name="field">The filtering field.</param>
        /// <param name="value">The filtering value.</param>
        /// <param name="operation">The filtering operation.</param>
        public FilterItem(string field, object value, FilterOperation operation = FilterOperation.Equal)
        {
            Field = field;
            Value = value;
            Operation = operation;
        }

        /// <summary>
        /// Gets or sets the filtering field.
        /// </summary>
        /// <value>
        /// The filtering field.
        /// </value>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the filtering value.
        /// </summary>
        /// <value>
        /// The filtering value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the filtering operation.
        /// </summary>
        /// <value>
        /// The filtering operation.
        /// </value>
        public FilterOperation Operation { get; set; }
    }

    /// <summary>
    /// Represents container for ordering items list
    /// </summary>
    [Serializable]
    public class DataOrder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataOrder" /> class.
        /// </summary>
        public DataOrder()
        {
            OrderItems = new List<OrderItem>();
        }

        /// <summary>
        /// Gets or sets the list of order items.
        /// </summary>
        /// <value>
        /// The list of order items.
        /// </value>
        public IList<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Adds the order item to orderings list.
        /// </summary>
        /// <param name="field">The ordering field.</param>
        /// <param name="direction">The order direction.</param>
        public void Add(string field, OrderDirection direction = OrderDirection.Asc)
        {
            var filterItem = new OrderItem(field, direction);

            OrderItems.Add(filterItem);
        }
    }

    /// <summary>
    /// Represents class for ordering items
    /// </summary>
    [Serializable]
    public class OrderItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public OrderItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="direction">The order direction.</param>
        public OrderItem(string field, OrderDirection direction = OrderDirection.Asc)
        {
            Direction = direction;
            Field = field;
        }

        /// <summary>
        /// Gets or sets the ordering field.
        /// </summary>
        /// <value>
        /// The ordering field.
        /// </value>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the order direction.
        /// </summary>
        /// <value>
        /// The order direction.
        /// </value>
        public OrderDirection Direction { get; set; }

        /// <summary>
        /// Gets a value indicating whether query must be ordered by descending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if query must be ordered by descending; otherwise, <c>false</c>.
        /// </value>
        public bool OrderByDescending
        {
            get
            {
                return Direction == OrderDirection.Desc;
            }
        }
    }

    /// <summary>
    /// DataOptions class extensions
    /// </summary>
    public static class DataOptionsExtensions
    {
        /// <summary>
        /// Applies the filter, the order and the paging to get request.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="options">The filtering / sorting options.</param>
        /// <param name="request">The request.</param>
        public static void ApplyTo<TModel>(this DataOptions options, GetDataRequest<TModel> request)
        {
            var creator = new DataOptionsQueryCreator(options);

            options.ApplyFilter(request, creator);
            options.ApplyOrder(request, creator);
            options.ApplyPaging(request);
        }

        /// <summary>
        /// Applies the filter to get request.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="options">The filtering / sorting options.</param>
        /// <param name="request">The request.</param>
        /// <param name="creator">The query creator.</param>
        public static void ApplyFilter<TModel>(this DataOptions options, GetDataRequest<TModel> request, DataOptionsQueryCreator creator = null)
        {
            if (options != null 
                && options.Filter != null 
                && ((options.Filter.FilterItems != null && options.Filter.FilterItems.Count > 0)
                    || (options.Filter.InnerFilters != null && options.Filter.InnerFilters.Count > 0)))
            {
                if (creator == null)
                {
                    creator = new DataOptionsQueryCreator(options);
                }

                var query = creator.GetFilterQuery();
                var parameters = creator.GetFilterParameters();
                var filter = DynamicExpression.ParseLambda<TModel, bool>(query, parameters);

                request.Filter = filter;
            }
        }

        /// <summary>
        /// Applies the order to get request.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="options">The filtering / sorting options.</param>
        /// <param name="request">The request.</param>
        /// <param name="creator">The query creator.</param>
        public static void ApplyOrder<TModel>(this DataOptions options, GetDataRequest<TModel> request, DataOptionsQueryCreator creator = null)
        {
            if (options != null && options.Order != null && options.Order.OrderItems != null)
            {
                if (creator == null)
                {
                    creator = new DataOptionsQueryCreator(options);
                }

                var query = creator.GetOrderQuery();

                var parameters = new[] { LinqExpression.Parameter(typeof(TModel), "") };
                ExpressionParser parser = new ExpressionParser(parameters, query, new object[0]);
                var orderings = parser.ParseOrdering();
                foreach (var order in orderings)
                {
                    var expression = order.Selector;
                    var conversion = LinqExpression.Convert(expression, typeof(object));

                    var lambda = System.Linq.Expressions.Expression.Lambda<Func<TModel, object>>(conversion, parameters);
                    request.AddOrder(lambda, !order.Ascending);
                }
            }
        }

        /// <summary>
        /// Applies the paging to get request.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="options">The filtering / sorting options.</param>
        /// <param name="request">The request.</param>
        public static void ApplyPaging<TModel>(this DataOptions options, GetDataRequest<TModel> request)
        {
            if (options != null && options.ItemsCount > 0)
            {
                if (options.StartItemNumber > 1)
                {
                    request.StartItemNumber = options.StartItemNumber;
                }
                request.ItemsCount = options.ItemsCount;
            }
        }
    }

    /// <summary>
    /// Helper class for creating filtering and ordering queries from DataOptions instance
    /// </summary>
    public class DataOptionsQueryCreator
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
            public const string Contains = "{0}.Contains({1})";
            public const string NotContains = "!{0}.Contains({1})";
            public const string StartsWith = "{0}.StartsWith({1})";
            public const string EndsWith = "{0}.EndsWith({1})";
            public const string Equal = "{0} == {1}";
            public const string NotEqual = "{0} != {1}";
            public const string Greater = "{0} > {1}";
            public const string GreaterOrEqual = "{0} >= {1}";
            public const string Less = "{0} < {1}";
            public const string LessOrEqual = "{0} <= {1}";
        }

        private readonly DataOptions dataOptions;
        
        private readonly IList<object> filterParameters;

        private string orderQuery;

        private string filterQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataOptionsQueryCreator" /> class.
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

            if (dataOptions.Order != null && dataOptions.Order.OrderItems != null && dataOptions.Order.OrderItems.Count > 0)
            {
                for (var i = 0; i < dataOptions.Order.OrderItems.Count; i++)
                {
                    var item = dataOptions.Order.OrderItems[i];
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(item.Field);

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
                ((dataOptions.Filter.FilterItems != null && dataOptions.Filter.FilterItems.Count > 0)
                    || (dataOptions.Filter.InnerFilters != null && dataOptions.Filter.InnerFilters.Count > 0)))
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

            var parameterNr = filterParameters.Count;
            filterParameters.Add(filterItem.Value);
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
                    query = string.Format(FilterConstants.Equal, propertyName, parameterNr);
                    break;
                case FilterOperation.NotEqual:
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

            foreach (var filterItem in filter.FilterItems)
            {
                var subQuery = CreateQueryExpression(filterItem, filter.Connector);
                AppendFilterConnector(sb, filter.Connector);
                sb.Append(subQuery);
            }

            if (filter.InnerFilters != null && filter.InnerFilters.Any())
            {
                foreach (var innerFilter in filter.InnerFilters)
                {
                    var subQuery = CreateQueryJunction(innerFilter);
                    AppendFilterConnector(sb, filter.Connector);
                    sb.Append(subQuery);
                }
            }

            return sb.ToString();
        }
    }
}
