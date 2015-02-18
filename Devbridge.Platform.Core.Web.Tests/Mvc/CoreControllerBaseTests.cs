using System.Linq;
using System.Web.Mvc;

using Autofac;

using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Web.Models;
using Devbridge.Platform.Core.Web.Mvc;
using Devbridge.Platform.Core.Web.Mvc.Commands;

using Devbridge.Platform.Sample.Web.Module.Controllers;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Mvc
{
    [TestFixture]
    public class CoreControllerBaseTests : TestBase
    {
        [Test]
        public void Should_Return_Correct_Controller_Messages()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var controller = container.Resolve<SampleWebController>() as ICommandContext;

                Assert.IsNotNull(controller);
                Assert.IsNotNull(controller.Messages);
            }
        }

        [Test]
        public void Should_Create_Success_Json_And_Append_Messages()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var controller = PrepareController(container);
                var messages = new[] { "Message 1" };
                var json = new WireJson { Success = true, Messages = messages.ToArray() };
                var jsonResult = controller.Json(json);

                Assert.AreEqual(jsonResult.Data, json);
                AssertJson(messages, jsonResult);
            }
        }
        
        [Test]
        public void Should_Create_Failed_Json_And_Append_Messages()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var controller = PrepareController(container);
                var messages = new[] { "Message 1" };
                var json = new WireJson { Success = false, Messages = messages.ToArray() };
                var jsonResult = controller.Json(json);

                Assert.AreEqual(jsonResult.Data, json);
                AssertJson(messages, jsonResult);
            }
        }
        
        [Test]
        public void Should_Create_Success_WireJson_And_Append_Messages()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var controller = PrepareController(container);
                var data = new { Test = "Test" };
                var jsonResult = controller.WireJson(true, data);

                var result = AssertJson(null, jsonResult);
                Assert.AreEqual(result.Data, data);
            }
        }
        
        [Test]
        public void Should_Create_Success_ComboWireJson_And_Append_Messages()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var controller = PrepareController(container);
                var data = new { Test = "Test" };
                const string html = "html";
                var jsonResult = controller.ComboWireJson(true, html, data);

                var result = AssertJson(null, jsonResult);
                Assert.AreEqual(result.Data, data);

                Assert.IsTrue(result is ComboWireJson);
                var comboResult = (ComboWireJson)result;

                Assert.AreEqual(comboResult.Data, data);
                Assert.AreEqual(comboResult.Html, html);
            }
        }

        private CoreControllerBase PrepareController(ILifetimeScope container)
        {
            var controller = container.Resolve<SampleWebController>();
            Assert.IsNotNull(controller);

            controller.Messages.AddError("Test Error");
            controller.Messages.AddSuccess("Test Success");

            return controller;
        }

        private WireJson AssertJson(string[] messages, JsonResult jsonResult)
        {
            Assert.IsNotNull(jsonResult);

            Assert.IsTrue(jsonResult.Data is WireJson);
            var returnedJson = (WireJson)jsonResult.Data;

            var count = messages != null ? messages.Count() + 1 : 1;
            Assert.AreEqual(returnedJson.Messages.Count(), count);
            if (returnedJson.Success)
            {
                Assert.IsTrue(returnedJson.Messages.Contains("Test Success"));
            }
            else
            {
                Assert.IsTrue(returnedJson.Messages.Contains("Test Error"));
            }

            if (messages != null)
            {
                foreach (var message in messages)
                {
                    Assert.IsTrue(returnedJson.Messages.Contains(message));
                }
            }

            return returnedJson;
        }
    }
}
