using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.WebApi.Helpers;
using BetterCms.Module.WebApi.Models;

using NUnit.Framework;

namespace BetterCms.WebApi.Tests.UnitTests
{
    /// <summary>
    /// TODO: Create tests for fake lists for order by, filtering, paging
    /// </summary>

    [TestFixture]
    public class DataOptionsExtensionsTests
    {
        [Test]
        public void SingleOrderBy()
        {
            var dataOptions = new DataOptions();
            dataOptions.Order.Add("IsDeleted");

            var request = new TestRequest();
            dataOptions.ApplyOrder(request);

            Assert.IsNotNull(request.OrderBy);
            Assert.AreEqual(request.OrderBy.Count, 1);
            Assert.IsNull(request.Filter);
            Assert.AreEqual(request.ItemsCount, null);
            Assert.AreEqual(request.StartItemNumber, 1);
        }

        private class TestModel : ModelBase
        {
        }

        private class TestRequest : GetDataRequest<TestModel>
        {
            public TestRequest(Expression<Func<TestModel, bool>> filter = null,
                Expression<Func<TestModel, dynamic>> order = null,
                bool orderDescending = false,
                int? itemsCount = null,
                int startItemNumber = 1)
                : base(filter, order, orderDescending, itemsCount, startItemNumber)
            {
            }
        }
    }
}