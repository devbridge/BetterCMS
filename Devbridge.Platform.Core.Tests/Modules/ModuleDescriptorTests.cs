using Devbridge.Platform.Core.Modules;
using Devbridge.Platform.Core.Modules.Registration;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Modules
{
    [TestFixture]
    public class ModuleDescriptorTests : TestBase
    {
        [Test]
        public void Should_Return_Correct_Assembly_Name()
        {
            var descriptor = new TestModuleDescriptor();

            Assert.AreEqual(descriptor.AssemblyName.Name, GetType().Assembly.GetName().Name);
        }
        
        [Test]
        public void Should_Return_Correct_RegistrationContext()
        {
            var descriptor = new TestModuleDescriptor();
            var context = descriptor.CreateRegistrationContext();

            Assert.IsNotNull(context);
            Assert.AreEqual(context.GetType(), typeof(ModuleRegistrationContext));
        }

        private class TestModuleDescriptor : ModuleDescriptor
        {
            public override string Description
            {
                get
                {
                    return "Test";
                }
            }

            public override string Name
            {
                get
                {
                    return "Test";
                }
            }
        }
    }
}
