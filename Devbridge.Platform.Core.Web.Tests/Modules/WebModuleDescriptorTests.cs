using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

using Autofac;

using Devbridge.Platform.Core.Web.Modules;
using Devbridge.Platform.Core.Web.Modules.Registration;
using Devbridge.Platform.Core.Web.Mvc.Commands;
using Devbridge.Platform.Core.Web.Mvc.Extensions;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Modules
{
    [TestFixture]
    public class WebModuleDescriptorTests : TestBase
    {
        [Test]
        public void Should_Return_Correct_AreaName()
        {
            var descriptor = new TestWebModuleDescriptor();

            Assert.AreEqual(descriptor.AreaName, "module-testwebmodule");
        }
        
        [Test]
        public void Should_Return_Correct_BaseModulePath()
        {
            var descriptor = new TestWebModuleDescriptor();

            Assert.AreEqual(descriptor.BaseModulePath, "/file/module-testwebmodule");
        }
        
        [Test]
        public void Should_Return_Correct_JsBasePath()
        {
            var descriptor = new TestWebModuleDescriptor();

            Assert.AreEqual(descriptor.JsBasePath, "/file/module-testwebmodule/scripts");
        }
        
        [Test]
        public void Should_Return_Correct_CssBasePath()
        {
            var descriptor = new TestWebModuleDescriptor();

            Assert.AreEqual(descriptor.CssBasePath, "/file/module-testwebmodule/content/styles");
        }
        
        [Test]
        public void Should_Return_Correct_MinifiedJsPath()
        {
            var descriptor = new TestWebModuleDescriptor();

            Assert.AreEqual(descriptor.MinifiedJsPath, "/file/module-testwebmodule/scripts/module.testwebmodule.min.js");
        }
        
        [Test]
        public void Should_Return_Correct_MinifiedCssPath()
        {
            var descriptor = new TestWebModuleDescriptor();

            Assert.AreEqual(descriptor.MinifiedCssPath, "/file/module-testwebmodule/content/styles/module.testwebmodule.min.css");
        }

        [Test]
        public void Should_Register_ModuleControllers_Correctly()
        {
            var descriptor = new TestWebModuleDescriptor();
            var context = new WebModuleRegistrationContext(descriptor);
            var containerBuilder = new ContainerBuilder();

            var controllerExtensions = new Mock<IControllerExtensions>();
            controllerExtensions
                .Setup(ce => ce.GetControllerTypes(It.IsAny<Assembly>()))
                .Returns<Assembly>(assembly => new[] { typeof(TestController1), typeof(TestController2) });

            descriptor.RegisterModuleControllers(context, containerBuilder, controllerExtensions.Object);

            // Routes registration
            Assert.AreEqual(context.Routes.Count, 1);
            var route = (Route)context.Routes[0];
            Assert.AreEqual(route.Url, "module-testwebmodule/{controller}/{action}");
            Assert.AreEqual(route.DataTokens["area"], "module-testwebmodule");

            // Types registration
            var container = containerBuilder.Build();

            var controller1 = container.Resolve(typeof(TestController1));
            var controller2 = container.Resolve(typeof(TestController2));

            TestController3 controller3 = null;
            try
            {
                controller3 = container.Resolve<TestController3>();
            }
            catch
            {
            }
            
            Assert.IsNotNull(controller1);
            Assert.IsNotNull(controller2);
            Assert.IsNull(controller3);
        }
        
        [Test]
        public void Should_Register_RegisterModuleCommands_Correctly()
        {
            var descriptor = new TestWebModuleDescriptor();
            var context = new WebModuleRegistrationContext(descriptor);
            var containerBuilder = new ContainerBuilder();

            descriptor.RegisterModuleCommands(context, containerBuilder);

            var container = containerBuilder.Build();
            
            var commandin = container.Resolve<TestCommandIn>();
            var commandout = container.Resolve<TestCommandOut>();
            var commandinout = container.Resolve<TestCommandInOut>();

            Assert.IsNotNull(commandin);
            Assert.IsNotNull(commandout);
            Assert.IsNotNull(commandinout);
        }

        [Test]
        public void Should_Return_Correct_RegistrationContext()
        {
            var descriptor = new TestWebModuleDescriptor();
            var context = descriptor.CreateRegistrationContext();

            Assert.IsNotNull(context);
            Assert.AreEqual(context.GetType(), typeof(WebModuleRegistrationContext));
        }

        #region CLASSES FOR TESTS

        private class TestWebModuleDescriptor : WebModuleDescriptor
        {
            public override string Name
            {
                get
                {
                    return "testWebModule";
                }
            }

            public override string Description
            {
                get
                {
                    return "Test web module";
                }
            }
        }

        private class TestController1 : IController
        {
            public void Execute(RequestContext requestContext)
            {
                throw new NotImplementedException();
            }
        }
        
        private class TestController2 : IController
        {
            public void Execute(RequestContext requestContext)
            {
                throw new NotImplementedException();
            }
        }
        
        private class TestController3 : IController
        {
            public void Execute(RequestContext requestContext)
            {
                throw new NotImplementedException();
            }
        }
        
        public class TestCommandIn : CoreCommandBase, ICommandIn<int>
        {
            public void Execute(int request)
            {
                throw new NotImplementedException();
            }
        }
        
        public class TestCommandOut : CoreCommandBase, ICommandOut<int>
        {
            public int Execute()
            {
                throw new NotImplementedException();
            }
        }
        
        public class TestCommandInOut : CoreCommandBase, ICommand<int, int>
        {
            public int Execute(int request)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
