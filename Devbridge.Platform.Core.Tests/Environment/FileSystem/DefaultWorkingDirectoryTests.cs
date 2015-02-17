using System;
using System.IO;
using System.Linq;

using Devbridge.Platform.Core.Environment.FileSystem;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Environment.FileSystem
{
    [TestFixture]
    public class DefaultWorkingDirectoryTests : TestBase
    {
        private string OriginalFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TestModules", "test.dll");
            }
        }
        
        private string RuntimeFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.dll");
            }
        }

        private string ModuleFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Modules", "test.dll");
            }
        }

        [Test]
        public void ShouldReturn_Correct_WorkingDirectoryPath()
        {
            var service = new DefaultWorkingDirectory();
            var path = service.GetWorkingDirectoryPath();

            Assert.IsNotNull(path);
            Assert.IsTrue(path.Contains(AppDomain.CurrentDomain.BaseDirectory));
        }
        
        [Test]
        public void ShouldReturn_AvailableModules()
        {
            PrepareTestDll();

            var service = new DefaultWorkingDirectory();
            var modules = service.GetAvailableModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(modules.Count(), 1);
            Assert.AreEqual(modules.First().Name, "test.dll");

            RemoveTestDll();
        }
        
        [Test]
        public void ShouldCopy_ModulesToRuntimeDirectory_Successfully()
        {
            PrepareTestDll();

            var service = new DefaultWorkingDirectory();
            service.RecopyModulesToRuntimeFolder(new FileInfo(ModuleFileName));
            Assert.IsTrue(File.Exists(RuntimeFileName));
            
            RemoveTestDll();
        }

        private void PrepareTestDll()
        {
            RemoveTestDll();

            var directory = Path.GetDirectoryName(ModuleFileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(ModuleFileName))
            {
                File.Copy(OriginalFileName, ModuleFileName);
            }
        }

        private void RemoveTestDll()
        {
            if (File.Exists(RuntimeFileName))
            {
                File.Delete(RuntimeFileName);
            }
            if (File.Exists(ModuleFileName))
            {
                File.Delete(ModuleFileName);
            }
        }
    }
}
