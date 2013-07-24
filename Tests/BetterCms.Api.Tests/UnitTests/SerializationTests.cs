using System;
using System.Text;
using System.Web.Script.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

using NUnit.Framework;

namespace BetterCms.Api.Tests.UnitTests
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void SerializeOptions()
        {
            var options = CreateTestDataOptions();
            var serializer = new JavaScriptSerializer();

            string serialized = serializer.Serialize(options);
            Console.WriteLine(serialized);

            var deserialized = serializer.Deserialize<DataOptions>(serialized);

            Assert.IsNotNull(options);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(options.Take, deserialized.Take);
            Assert.AreEqual(options.Skip, deserialized.Skip);

            AreFiltersEqual(options.Filter, deserialized.Filter);
            AreOrdersEqual(options.Order, deserialized.Order);
        }

        [Test]
        public void DeserializeOptions()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("    \"filter\": {");
            sb.AppendLine("        \"where\": [");
            sb.AppendLine("            { \"field\": \"CreatedOn\", \"operation\": \"greater\", \"value\": \"\\/Date(1224043200000)\\/\" }");
            sb.AppendLine("            , { \"field\": \"Title\", \"operation\": \"notEqual\", \"value\": \"Africa\" }");
            sb.AppendLine("        ],");
            sb.AppendLine("        \"inner\": [");
            sb.AppendLine("            {");
            sb.AppendLine("                \"connector\": \"or\",");
            sb.AppendLine("                \"where\": [");
            sb.AppendLine("                    { \"field\": \"Title\", \"operation\": \"startsWith\", \"value\": \"It\" }");
            sb.AppendLine("                    , { \"field\": \"Title\", \"operation\": \"endsWith\", \"value\": \"na\" }");
            sb.AppendLine("                ]");
            sb.AppendLine("            }");
            sb.AppendLine("       ]");
            sb.AppendLine("    },");
            sb.AppendLine("   \"order\": {");
            sb.AppendLine("        \"by\": [");
            sb.AppendLine("            { \"field\": \"CreatedOn\" }");
            sb.AppendLine("            , { \"field\": \"Title\", \"direction\": \"desc\" }");
            sb.AppendLine("        ]");
            sb.AppendLine("    },");
            sb.AppendLine("    \"skip\": 3,");
            sb.AppendLine("    \"take\": 5");
            sb.AppendLine("}");

            var serializer = new JavaScriptSerializer();
            var json = sb.ToString();

            Console.WriteLine(json);

            var options = serializer.Deserialize<DataOptions>(json);

            Assert.IsNotNull(options);
            Assert.AreEqual(options.Filter.Where.Count, 2);
            Assert.AreEqual(options.Filter.Where[1].Field, "Title");
            Assert.AreEqual(options.Filter.Where[1].Value, "Africa");
            Assert.AreEqual(options.Filter.Where[1].Operation, FilterOperation.NotEqual);

            Assert.AreEqual(options.Filter.Inner.Count, 1);
            Assert.AreEqual(options.Filter.Inner[0].Connector, FilterConnector.Or);

            Assert.AreEqual(options.Filter.Inner[0].Where.Count, 2);
            Assert.AreEqual(options.Filter.Inner[0].Where[0].Field, "Title");
            Assert.AreEqual(options.Filter.Inner[0].Where[0].Value, "It");
            Assert.AreEqual(options.Filter.Inner[0].Where[0].Operation, FilterOperation.StartsWith);
            
            Assert.AreEqual(options.Take, 5);
            Assert.AreEqual(options.Skip, 3);
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

            options.Filter.Inner.Add(subFilter);

            options.Order.Add("CreatedOn");
            options.Order.Add("Title", OrderDirection.Desc);

            return options;
        }

        private void AreFiltersEqual(DataFilter filter1, DataFilter filter2)
        {
            Assert.AreEqual(filter1.Connector, filter2.Connector);

            Assert.AreEqual(filter1.Where.Count, filter2.Where.Count);
            Assert.AreEqual(filter1.Inner.Count, filter2.Inner.Count);

            for (var i = 0; i < filter1.Where.Count; i++)
            {
                var item1 = filter1.Where[i];
                var item2 = filter1.Where[i];

                Assert.AreEqual(item1.Field, item2.Field);
                Assert.AreEqual(item1.Operation, item2.Operation);
                Assert.AreEqual(item1.Value, item2.Value);
            }

            for (var i = 0; i < filter1.Inner.Count; i++)
            {
                AreFiltersEqual(filter1.Inner[i], filter2.Inner[i]);
            }
        }

        private void AreOrdersEqual(DataOrder order1, DataOrder order2)
        {
            Assert.AreEqual(order1.By.Count, order2.By.Count);

            for (var i = 0; i < order1.By.Count; i++)
            {
                var item1 = order1.By[0];
                var item2 = order2.By[0];

                Assert.AreEqual(item1.Field, item2.Field);
                Assert.AreEqual(item1.Direction, item2.Direction);
                Assert.AreEqual(item1.OrderByDescending, item2.OrderByDescending);
            }
        }

        /*
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
        */
    }
}