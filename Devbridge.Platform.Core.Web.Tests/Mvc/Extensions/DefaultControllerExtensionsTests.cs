using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Web.Mvc.Extensions;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Mvc.Extensions
{
    [TestFixture]
    public class DefaultControllerExtensionsTests : TestBase
    {
        [Test]
        public void ShouldReturn_ControllerTypesList_Correctly()
        {
            var controllerExt = GetControllerTExtensions();
            var controllers = controllerExt.GetControllerTypes(GetType().Assembly);

            Assert.IsNotNull(controllers);
            var controllersList = controllers.ToList();
            Assert.AreEqual(controllersList.Count, 1);
            Assert.AreEqual(controllersList[0], typeof(PublicTestController));
        }

        [Test]
        public void ShouldReturn_Correct_ControllerName()
        {
            var controllerExt = GetControllerTExtensions();
            var controllerName = controllerExt.GetControllerName(typeof(PublicNestedTestController));

            Assert.AreEqual(controllerName, "PublicNestedTest");
        }
        
        [Test]
        public void ShouldReturn_Correct_NonControllerName()
        {
            var controllerExt = GetControllerTExtensions();
            var controllerName = controllerExt.GetControllerName(typeof(int));

            Assert.IsNull(controllerName);
        }

        [Test]
        public void ShouldReturn_Correct_Controller_Actions()
        {
            var controllerExt = GetControllerTExtensions();
            var actions = controllerExt.GetControllerActions(typeof(WebTestController));

            Assert.IsNotNull(actions);
            Assert.AreEqual(actions.Count(), 1);
            Assert.AreEqual(actions.First().Name, "TestAction");
        }
        
        [Test]
        public void ShouldReturn_Correct_GenericController_Actions()
        {
            var controllerExt = GetControllerTExtensions();
            var actions = controllerExt.GetControllerActions<WebTestController>();

            Assert.IsNotNull(actions);
            Assert.AreEqual(actions.Count(), 1);
            Assert.AreEqual(actions.First().Name, "TestAction");
        }

        private DefaultControllerExtensions GetControllerTExtensions()
        {
            var types = new[]
            {
                typeof (AbstractTestController),
                typeof (PrivateTestController),
                typeof (PublicTestController),
                typeof (PublicNestedTestController),
                typeof (IController),
                typeof (DefaultControllerExtensionsTests)
            };

            var assemblyLoader = new Mock<IAssemblyLoader>();
            assemblyLoader.Setup(a => a.GetLoadableTypes(It.IsAny<Assembly>())).Returns<Assembly>(a => types);

            var controllerExt = new DefaultControllerExtensions(assemblyLoader.Object);

            return controllerExt;
        }

        private class PrivateTestController : IController
        {
            public void Execute(RequestContext requestContext)
            {
            }
        }

        public class PublicNestedTestController : IController
        {
            public void Execute(RequestContext requestContext)
            {
            }
        }
        
        public class WebTestController : ControllerBase
        {
            protected override void ExecuteCore()
            {
                throw new System.NotImplementedException();
            }

            public ActionResult TestAction()
            {
                throw new System.NotImplementedException();
            }
        }
    }

    public abstract class AbstractTestController : IController
    {
        public void Execute(RequestContext requestContext)
        {
        }
    }

    public class PublicTestController : IController
    {
        public void Execute(RequestContext requestContext)
        {
        }
    }
}
