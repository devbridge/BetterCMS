using BetterCms.Module.WebApi.Helpers;
using BetterCms.Module.WebApi.Models;
using BetterCms.Module.WebApi.Models.Enums;

using NUnit.Framework;

namespace BetterCms.WebApi.Tests.UnitTests
{
    [TestFixture]
    public class QueryCreatorTests
    {
        private const string TestValueDate = "2000-10-15 12:43:59";
        private const string TestValueString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

        [Test]
        public void SingleOrderBy()
        {
            var dataOptions = new DataOptions();
            dataOptions.Order.Add("CreatedOn");

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.IsNotNull(filterParameters);
            Assert.IsEmpty(filterParameters);
            Assert.AreEqual(filterQuery, string.Empty);
            
            Assert.AreEqual(orderQuery, "CreatedOn");
        }
        
        [Test]
        public void SingleOrderByDescending()
        {
            var dataOptions = new DataOptions();
            dataOptions.Order.Add("CreatedOn", OrderDirection.Desc);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.IsNotNull(filterParameters);
            Assert.IsEmpty(filterParameters);
            Assert.AreEqual(filterQuery, string.Empty);

            Assert.AreEqual(orderQuery, "CreatedOn desc");
        }
        
        [Test]
        public void MultipleOrderBy()
        {
            var dataOptions = new DataOptions();
            dataOptions.Order.Add("CreatedOn", OrderDirection.Desc);
            dataOptions.Order.Add("Title");
            dataOptions.Order.Add("Description", OrderDirection.Desc);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.IsNotNull(filterParameters);
            Assert.IsEmpty(filterParameters);
            Assert.AreEqual(filterQuery, string.Empty);

            Assert.AreEqual(orderQuery, "CreatedOn desc, Title, Description desc");
        }

        [Test]
        public void SingleFilterByEqualOptional()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Created", TestValueDate);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Created == @0");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueDate);
        }
        
        [Test]
        public void SingleFilterByEqual()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Created", TestValueDate, FilterOperation.Equal);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Created == @0");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueDate);
        }
        
        [Test]
        public void SingleFilterByNotEqual()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Created", TestValueDate, FilterOperation.NotEqual);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Created != @0");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueDate);
        }
        
        [Test]
        public void SingleFilterByGreater()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Created", TestValueDate, FilterOperation.Greater);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Created > @0");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueDate);
        }
        
        [Test]
        public void SingleFilterByGreaterOrEqual()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Created", TestValueDate, FilterOperation.GreaterOrEqual);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Created >= @0");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueDate);
        }
        
        [Test]
        public void SingleFilterByLess()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Created", TestValueDate, FilterOperation.Less);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Created < @0");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueDate);
        }
        
        [Test]
        public void SingleFilterByLessOrEqual()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Created", TestValueDate, FilterOperation.LessOrEqual);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Created <= @0");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueDate);
        }
        
        [Test]
        public void SingleFilterByContains()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Title", TestValueString, FilterOperation.Contains);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Title.Contains(@0)");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueString);
        }
        
        [Test]
        public void SingleFilterByNotContains()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Title", TestValueString, FilterOperation.NotContains);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "!Title.Contains(@0)");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueString);
        }
        
        [Test]
        public void SingleFilterByStartsWith()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Title", TestValueString, FilterOperation.StartsWith);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Title.StartsWith(@0)");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueString);
        }
        
        [Test]
        public void SingleFilterByEndsWith()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Title", TestValueString, FilterOperation.EndsWith);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "Title.EndsWith(@0)");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 1);
            Assert.AreEqual(filterParameters[0], TestValueString);
        }

        [Test]
        public void ComplexFilterWithAndConnector()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Add("Title", "Test1", FilterOperation.NotContains);
            dataOptions.Filter.Add("Title", "Test2", FilterOperation.NotContains);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "!Title.Contains(@0) and !Title.Contains(@1)");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 2);
            Assert.AreEqual(filterParameters[0], "Test1");
            Assert.AreEqual(filterParameters[1], "Test2");
        }
        
        [Test]
        public void MoreComplexFilterWithAndConnector()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Connector = FilterConnector.And;
            dataOptions.Filter.Add("Title", "Test1", FilterOperation.NotContains);
            dataOptions.Filter.Add("Title", "Test2", FilterOperation.Equal);
            dataOptions.Filter.Add("Title", "Test3", FilterOperation.NotEqual);
            dataOptions.Filter.Add("Title", "Test4", FilterOperation.StartsWith);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "!Title.Contains(@0) and Title == @1 and Title != @2 and Title.StartsWith(@3)");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 4);
            Assert.AreEqual(filterParameters[0], "Test1");
            Assert.AreEqual(filterParameters[1], "Test2");
            Assert.AreEqual(filterParameters[2], "Test3");
            Assert.AreEqual(filterParameters[3], "Test4");
        }

        [Test]
        public void ComplexFilterWithOrConnector()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Connector = FilterConnector.Or;
            dataOptions.Filter.Add("Title", "Test1", FilterOperation.NotContains);
            dataOptions.Filter.Add("Title", "Test2", FilterOperation.NotContains);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "!Title.Contains(@0) or !Title.Contains(@1)");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 2);
            Assert.AreEqual(filterParameters[0], "Test1");
            Assert.AreEqual(filterParameters[1], "Test2");
        }

        [Test]
        public void MoreComplexFilterWithOrConnector()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Connector = FilterConnector.Or;
            dataOptions.Filter.Add("Title", "Test1", FilterOperation.NotContains);
            dataOptions.Filter.Add("Title", "Test2", FilterOperation.Equal);
            dataOptions.Filter.Add("Title", "Test3", FilterOperation.NotEqual);
            dataOptions.Filter.Add("Title", "Test4", FilterOperation.StartsWith);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            Assert.AreEqual(filterQuery, "!Title.Contains(@0) or Title == @1 or Title != @2 or Title.StartsWith(@3)");
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 4);
            Assert.AreEqual(filterParameters[0], "Test1");
            Assert.AreEqual(filterParameters[1], "Test2");
            Assert.AreEqual(filterParameters[2], "Test3");
            Assert.AreEqual(filterParameters[3], "Test4");
        }

        [Test]
        public void ComplexFilterWithInnerFilters()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Connector = FilterConnector.And;
            dataOptions.Filter.Add("Title", "Test1", FilterOperation.NotContains);
            dataOptions.Filter.Add("Title", "Test2", FilterOperation.NotContains);

            var innerFilter1 = new DataFilter(FilterConnector.Or);
            innerFilter1.Add("CreatedOn", TestValueDate, FilterOperation.Greater);
            innerFilter1.Add("CreatedOn", TestValueDate, FilterOperation.Less);
            innerFilter1.Add("CreatedOn", TestValueDate);

            var innerFilter2 = new DataFilter(FilterConnector.Or);
            innerFilter2.Add("ModifiedOn", TestValueDate, FilterOperation.Greater);
            innerFilter2.Add("ModifiedOn", TestValueDate, FilterOperation.Less);
            innerFilter2.Add("ModifiedOn", TestValueDate);
            
            dataOptions.Filter.InnerFilters.Add(innerFilter1);
            dataOptions.Filter.InnerFilters.Add(innerFilter2);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, string.Empty);

            var result = "!Title.Contains(@0) and !Title.Contains(@1) and (CreatedOn > @2 or CreatedOn < @3 or CreatedOn == @4) and (ModifiedOn > @5 or ModifiedOn < @6 or ModifiedOn == @7)";

            Assert.AreEqual(filterQuery, result);
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 8);
            Assert.AreEqual(filterParameters[0], "Test1");
            Assert.AreEqual(filterParameters[1], "Test2");
            Assert.AreEqual(filterParameters[2], TestValueDate);
            Assert.AreEqual(filterParameters[3], TestValueDate);
            Assert.AreEqual(filterParameters[4], TestValueDate);
            Assert.AreEqual(filterParameters[5], TestValueDate);
            Assert.AreEqual(filterParameters[6], TestValueDate);
            Assert.AreEqual(filterParameters[7], TestValueDate);
        }
        
        [Test]
        public void ComplexFilterWithInnerFiltersAndOrderBy()
        {
            var dataOptions = new DataOptions();
            dataOptions.Filter.Connector = FilterConnector.And;
            dataOptions.Filter.Add("Title", "Test1", FilterOperation.NotContains);
            dataOptions.Filter.Add("Title", "Test2", FilterOperation.NotContains);

            var innerFilter1 = new DataFilter(FilterConnector.Or);
            innerFilter1.Add("CreatedOn", TestValueDate, FilterOperation.Greater);
            innerFilter1.Add("CreatedOn", TestValueDate, FilterOperation.Less);
            innerFilter1.Add("CreatedOn", TestValueDate);

            var innerFilter2 = new DataFilter(FilterConnector.Or);
            innerFilter2.Add("ModifiedOn", TestValueDate, FilterOperation.Greater);
            innerFilter2.Add("ModifiedOn", TestValueDate, FilterOperation.Less);
            innerFilter2.Add("ModifiedOn", TestValueDate);
            
            dataOptions.Filter.InnerFilters.Add(innerFilter1);
            dataOptions.Filter.InnerFilters.Add(innerFilter2);

            dataOptions.Order.Add("CreatedOn", OrderDirection.Desc);
            dataOptions.Order.Add("Title");
            dataOptions.Order.Add("Description", OrderDirection.Desc);

            var queryCreator = new DataOptionsQueryCreator(dataOptions);
            var orderQuery = queryCreator.GetOrderQuery();
            var filterQuery = queryCreator.GetFilterQuery();
            var filterParameters = queryCreator.GetFilterParameters();

            Assert.AreEqual(orderQuery, "CreatedOn desc, Title, Description desc");

            var result = "!Title.Contains(@0) and !Title.Contains(@1) and (CreatedOn > @2 or CreatedOn < @3 or CreatedOn == @4) and (ModifiedOn > @5 or ModifiedOn < @6 or ModifiedOn == @7)";

            Assert.AreEqual(filterQuery, result);
            Assert.IsNotNull(filterParameters);
            Assert.AreEqual(filterParameters.Length, 8);
            Assert.AreEqual(filterParameters[0], "Test1");
            Assert.AreEqual(filterParameters[1], "Test2");
            Assert.AreEqual(filterParameters[2], TestValueDate);
            Assert.AreEqual(filterParameters[3], TestValueDate);
            Assert.AreEqual(filterParameters[4], TestValueDate);
            Assert.AreEqual(filterParameters[5], TestValueDate);
            Assert.AreEqual(filterParameters[6], TestValueDate);
            Assert.AreEqual(filterParameters[7], TestValueDate);
        }
    }
}