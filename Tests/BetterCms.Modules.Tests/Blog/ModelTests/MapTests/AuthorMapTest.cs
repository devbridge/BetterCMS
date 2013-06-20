using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Blog.Models;

using NHibernate;
using NHibernate.Criterion;

using NUnit.Framework;

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
        
        private GetBlogPostsRequest CreateTestRequest()
        {
            // Creating request
            var request = new GetBlogPostsRequest(5, 3);
            request.Filter.Add(BlogPostData.CreatedOn, new DateTime(2013, 06, 01), FilterOperation.Greater);
            request.Filter.Add(BlogPostData.Title, "Africa", FilterOperation.NotEqual);
            request.Filter.Add(BlogPostData.AuthorName, "Af", FilterOperation.StartsWith);

            var subFilter = new DataFilter<BlogPostData>(FilterConnector.Or);
            subFilter.Add(BlogPostData.Title, "It", FilterOperation.StartsWith);
            subFilter.Add(BlogPostData.Title, "Af", FilterOperation.StartsWith);
            subFilter.Add(BlogPostData.Title, "na", FilterOperation.EndsWith);
            subFilter.Add(BlogPostData.Title, "Spain");

            request.Filter.InnerFilters.Add(subFilter);

            request.Order.Add(BlogPostData.CreatedOn);
            request.Order.Add(BlogPostData.Title, OrderOperation.Desc);

            return request;
        }

        [Test]
        public void SerializeRequest()
        {
            var request = CreateTestRequest();
            var serializer = new JavaScriptSerializer();
            
            string serialized =  serializer.Serialize(request);
            Console.WriteLine(serialized);

            request = serializer.Deserialize<GetBlogPostsRequest>(serialized);
        }
        
        [Test]
        public void DeserializeRequest()
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

                        var request = serializer.Deserialize<GetBlogPostsRequest>(json);

                        Console.WriteLine(request.Filter.FilterItems[0].Value);
                        Console.WriteLine(request.Filter.FilterItems[0].Value.GetType());

                        // Construct query
                        //var criteria = session.CreateCriteria<BlogPost>();

                        //var result = criteria.ToDataListResponse<BlogPost, BlogPostData>(request);
                        //var result2 = result;
                    });
        }

        [Test]
        public void QOver_3()
        {
            RunActionInTransaction(session =>
            {
                var request = CreateTestRequest();

                // Construct query
                var criteria = session.CreateCriteria<BlogPost>();

                var result = criteria.ToDataListResponse<BlogPost, BlogPostData>(request);
                var result2 = result;
            });
        }
    }

    public class RequestPropertyNameExtractor<T>
        where T : struct, IComparable, IConvertible, IFormattable
    {
        public virtual string GetPropertyName(T property)
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Property type must be enum!");
            }

            return (property as Enum).ToString();
        }
    }

    [Serializable]
    public class GetBlogPostsRequest : GetDataRequest<BlogPostData>
    {
        public GetBlogPostsRequest()
        {
        }

        public GetBlogPostsRequest(int? itemsCount, int startItemNumber)
            : base(itemsCount, startItemNumber, new BlogPostPropertyNameExtractor())
        {
        }
    }

    public class BlogPostPropertyNameExtractor : RequestPropertyNameExtractor<BlogPostData>
    {
        public override string GetPropertyName(BlogPostData property)
        {
            if (property == BlogPostData.AuthorName)
            {
                return "Author.Name";
            }
            return base.GetPropertyName(property);
        }
    }

    [Serializable]
    public class GetDataRequest<T>
        where T : struct, IComparable, IConvertible, IFormattable
    {
        public GetDataRequest(int? itemsCount = null,
            int startItemNumber = 1,
            RequestPropertyNameExtractor<T> nameExtractor = null)
        {
            if (nameExtractor == null)
            {
                nameExtractor = new RequestPropertyNameExtractor<T>();
            }

            Filter = new DataFilter<T>(nameExtractor);
            Order = new DataOrder<T>(nameExtractor);

            StartItemNumber = startItemNumber;
            ItemsCount = itemsCount;
        }

        public DataFilter<T> Filter { get; set; }
        
        public DataOrder<T> Order { get; set; }

        public int StartItemNumber { get; set; }

        public int? ItemsCount { get; set; }
    }

    public enum BlogPostData
    {
        Title,
        CreatedOn,
        ModifiedByUser,
        AuthorName,
        Tag
    }

    public enum OrderOperation
    {
        Asc,
        Desc
    }

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

    public enum FilterConnector
    {
        And,
        Or
    }

    [Serializable]
    public class DataFilter<T>
        where T : struct, IComparable, IConvertible, IFormattable
    {
        public FilterConnector Connector { get; set; }

        public IList<FilterItem> FilterItems { get; set; }

        public IList<DataFilter<T>> InnerFilters { get; set; }

        private RequestPropertyNameExtractor<T> nameExtractor;

        public DataFilter()
        {
        }

        public DataFilter(RequestPropertyNameExtractor<T> nameExtractor, FilterConnector connector = FilterConnector.And)
            : this(connector, nameExtractor)
        {
        }

        public DataFilter(FilterConnector connector, RequestPropertyNameExtractor<T> nameExtractor = null)
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Filter data type must be enum!");
            }

            this.nameExtractor = nameExtractor ?? new RequestPropertyNameExtractor<T>();

            FilterItems = new List<FilterItem>();
            InnerFilters = new List<DataFilter<T>>();
            Connector = connector;
        }

        public void Add(T field, object value, FilterOperation operation = FilterOperation.Equal)
        {
            var filterItem = new FilterItem(field, value, operation, nameExtractor);

            FilterItems.Add(filterItem);
        }

        [Serializable]
        public class FilterItem
        {
            private T field;
            private string fieldName;
            private readonly RequestPropertyNameExtractor<T> nameExtractor;

            public FilterItem()
            {
                nameExtractor = new RequestPropertyNameExtractor<T>();
            }

            public FilterItem(T field, object value, FilterOperation operation = FilterOperation.Equal, RequestPropertyNameExtractor<T> nameExtractor = null)
            {
                this.nameExtractor = nameExtractor ?? new RequestPropertyNameExtractor<T>();

                Field = field;
                Value = value;
                Operation = operation;
            }

            public string FieldName
            {
                get
                {
                    return fieldName;
                }
            }

            public T Field
            {
                get
                {
                    return field;
                }
                set
                {
                    if (!typeof(T).IsEnum)
                    {
                        throw new InvalidOperationException("Filter item data type must be enum!");
                    }

                    field = value;
                    fieldName = nameExtractor.GetPropertyName(field);
                }
            }

            public object Value { get; set; }

            public FilterOperation Operation { get; set; }
        }
    }

    public class DataOrder<T> 
        where T : struct, IComparable, IConvertible, IFormattable
    {
        private RequestPropertyNameExtractor<T> nameExtractor;

        public DataOrder()
            : this(null)
        {
        }

        public DataOrder(RequestPropertyNameExtractor<T> nameExtractor)
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Order data type must be enum!");
            }

            this.nameExtractor = nameExtractor ?? new RequestPropertyNameExtractor<T>();

            OrderItems = new List<OrderItem>();
        }

        public IList<OrderItem> OrderItems { get; set; }

        public void Add(T field, OrderOperation operation = OrderOperation.Asc)
        {
            var filterItem = new OrderItem(field, operation, nameExtractor);

            OrderItems.Add(filterItem);
        }

        public class OrderItem
        {
            private T field;
            private string fieldName;
            private RequestPropertyNameExtractor<T> nameExtractor;

            public OrderItem()
            {
                nameExtractor = new RequestPropertyNameExtractor<T>();
            }

            public OrderItem(T field, OrderOperation operation = OrderOperation.Asc, RequestPropertyNameExtractor<T> nameExtractor = null)
            {
                this.nameExtractor = nameExtractor ?? new RequestPropertyNameExtractor<T>();

                Operation = operation;
                Field = field;
            }

            public T Field
            {
                get
                {
                    return field;
                }
                set
                {
                    if (!typeof(T).IsEnum)
                    {
                        throw new InvalidOperationException("Order item data type must be enum!");
                    }

                    field = value;
                    fieldName = nameExtractor.GetPropertyName(field);
                }
            }

            public string FieldName
            {
                get
                {
                    return fieldName;
                }
            }

            public OrderOperation Operation { get; set; }

            public bool OrderByDescending
            {
                get
                {
                    return Operation == OrderOperation.Desc;
                }
            }
        }
    }

    public static class GetDataRequestExtensions
    {
        public static void ApplyFilter<T>(this GetDataRequest<T> request, ICriteria criteria)
             where T : struct, IComparable, IConvertible, IFormattable
        {
            if (request != null 
                && request.Filter != null 
                && ((request.Filter.FilterItems != null && request.Filter.FilterItems.Count > 0)
                    || (request.Filter.InnerFilters != null && request.Filter.InnerFilters.Count > 0)))
            {
                var junction = CreateQueryJunction(request.Filter);
                criteria.Add(junction);
            }
        }

        private static Junction CreateQueryJunction<T>(DataFilter<T> filter)
             where T : struct, IComparable, IConvertible, IFormattable
        {
            Junction junction;
            if (filter.Connector == FilterConnector.And)
            {
                junction = Restrictions.Conjunction();
            }
            else
            {
                junction = Restrictions.Disjunction();
            }

            foreach (var filterItem in filter.FilterItems)
            {
                var queryFilterItem = CreateCriterion(filterItem);
                junction.Add(queryFilterItem);
            }

            if (filter.InnerFilters != null && filter.InnerFilters.Any())
            {
                foreach (var innerFilter in filter.InnerFilters)
                {
                    var innerFilterQuery = CreateQueryJunction(innerFilter);
                    junction.Add(innerFilterQuery);
                }
            }

            return junction;
        }

        private static ICriterion CreateCriterion<T>(DataFilter<T>.FilterItem filterItem)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            var propertyName = filterItem.FieldName;

            switch (filterItem.Operation)
            {
                case FilterOperation.Contains:
                    return Restrictions.Like(propertyName, Convert.ToString(filterItem.Value), MatchMode.Anywhere);
                case FilterOperation.NotContains:
                    return Restrictions.Not(Restrictions.Like(propertyName, Convert.ToString(filterItem.Value), MatchMode.Anywhere));
                case FilterOperation.StartsWith:
                    return Restrictions.Like(propertyName, Convert.ToString(filterItem.Value), MatchMode.Start);
                case FilterOperation.EndsWith:
                    return Restrictions.Like(propertyName, Convert.ToString(filterItem.Value), MatchMode.End);

                case FilterOperation.Equal:
                    return Restrictions.Eq(propertyName, filterItem.Value);
                case FilterOperation.NotEqual:
                    return Restrictions.Not(Restrictions.Eq(propertyName, filterItem.Value));

                case FilterOperation.Greater:
                    return Restrictions.Gt(propertyName, filterItem.Value);
                case FilterOperation.GreaterOrEqual:
                    return Restrictions.Ge(propertyName, filterItem.Value);
                case FilterOperation.Less:
                    return Restrictions.Lt(propertyName, filterItem.Value);
                case FilterOperation.LessOrEqual:
                    return Restrictions.Le(propertyName, filterItem.Value);

                default:
                    throw new InvalidOperationException(string.Format("Unknown filter operation: {0}!", filterItem.Operation));
            }
        }

        public static void ApplyOrder<T>(this GetDataRequest<T> request, ICriteria criteria)
             where T : struct, IComparable, IConvertible, IFormattable
        {
            if (request != null && request.Order != null && request.Order.OrderItems != null)
            {
                foreach (var orderItem in request.Order.OrderItems)
                {
                    var propertyName = orderItem.FieldName;

                    criteria.AddOrder(new Order(propertyName, orderItem.OrderByDescending));
                }
            }
        }

        public static void ApplyPaging<T>(this GetDataRequest<T> request, ICriteria criteria)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            if (request != null && request.ItemsCount > 0)
            {
                if (request.StartItemNumber > 1)
                {
                    criteria.SetFirstResult(request.StartItemNumber - 1);
                }
                criteria.SetMaxResults(request.ItemsCount.Value);
            }
        }
    }

    public static class CriteriaExtensions
    {
        public static DataListResponse<TModel> ToDataListResponse<TModel, T>(this ICriteria criteria, GetDataRequest<T> request)
             where T : struct, IComparable, IConvertible, IFormattable
        {
            // Apply filter
            request.ApplyFilter(criteria);

            // Count future value
            var futureValue = ((ICriteria)criteria.Clone())
                .SetProjection(Projections.Count(Projections.Id()))
                .FutureValue<int>();

            // Apply order
            request.ApplyOrder(criteria);

            // Apply paging
            request.ApplyPaging(criteria);

            // Items future value
            var items = criteria
                .Future<TModel>();

            return new DataListResponse<TModel>(items.ToList(), futureValue.Value);
        }
    }
}
