using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Modules.Registration;
using Devbridge.Platform.Core.Tests.TestHelpers;
using Devbridge.Platform.Sample.Module;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Modules.Registration
{
    [TestFixture]
    public class DefaultModulesRegistrationTests : TestBase
    {
        [Test]
        public void ShouldAddDescriptor_FromAssembly_Correctly()
        {
            AddAssembly<SampleModuleDescriptor>(1);
        }

        [Test]
        public void ShouldNotAddDescriptor_FromAssembly_Correctly()
        {
            AddAssembly<DefaultModulesRegistration>(0);
        }

        private void AddAssembly<TType>(int expectedResult)
        {
            var assemblyLoaderMock = new Mock<IAssemblyLoader>();
            assemblyLoaderMock
                .Setup(a => a.GetLoadableTypes(It.IsAny<Assembly>()))
                .Returns<Assembly>(a => new List<Type> { typeof(TType) });

            var registration = new DefaultModulesRegistration(assemblyLoaderMock.Object);
            registration.AddModuleDescriptorTypeFromAssembly(typeof(TType).Assembly);

            registration.InitializeModules();
            var modules = registration.GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(modules.Count(), expectedResult);
        }
    }
}
