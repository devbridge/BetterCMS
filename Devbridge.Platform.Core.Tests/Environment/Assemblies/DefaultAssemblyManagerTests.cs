using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Environment.FileSystem;
using Devbridge.Platform.Core.Modules.Registration;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Environment.Assemblies
{
    [TestFixture]
    public class DefaultAssemblyManagerTests : TestBase
    {
        [Test]
        public void ShouldAddReferencedModulesCorrectly()
        {
            var registrationMock = new Mock<IModulesRegistration>();
            var assemblyLoaderMock = new Mock<IAssemblyLoader>();
            var workingDirectoryMock = new Mock<IWorkingDirectory>();

            var allAssemblies = new List<string>();

            assemblyLoaderMock
                .Setup(r => r.Load(It.IsAny<AssemblyName>()))
                .Callback<AssemblyName>(a => allAssemblies.Add(a.Name));

            registrationMock
                .Setup(r => r.AddModuleDescriptorTypeFromAssembly(It.IsAny<Assembly>()))
                .Callback<Assembly>(a =>
                {
                    if (a != null)
                    {
                        allAssemblies.Add(a.FullName);
                    }
                });

            var manager = new DefaultAssemblyManager(workingDirectoryMock.Object, registrationMock.Object, assemblyLoaderMock.Object);
            manager.AddReferencedModules();

            Assert.IsTrue(allAssemblies.Any(a => a.Contains("Devbridge.Platform.Sample.Module")));
        }

        /// <summary>
        /// Should retrieve the lists of modules from working directory, copy them to bin folder
        /// and load them using assembly loader
        /// </summary>
        [Test]
        public void ShouldAddUploadedModulesCorrectly()
        {
            var registrationMock = new Mock<IModulesRegistration>();
            var assemblyLoaderMock = new Mock<IAssemblyLoader>();
            var workingDirectoryMock = new Mock<IWorkingDirectory>();

            var assemblyLoaded = false;
            var assemblyCopied = false;

            var files = new[] { new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TestModules", "test.dll")) };
            workingDirectoryMock
                .Setup(d => d.GetAvailableModules())
                .Returns(() => files);
            
            workingDirectoryMock
                .Setup(d => d.RecopyModulesToRuntimeFolder(It.IsAny<FileInfo>()))
                .Returns<FileInfo>(
                    file =>
                    {
                        assemblyCopied = true;
                        Assert.AreEqual(files[0], file);

                        return file;
                    });

            assemblyLoaderMock
                .Setup(r => r.Load(It.IsAny<AssemblyName>()))
                .Callback<AssemblyName>(
                    file =>
                    {
                        assemblyLoaded = true;
                        Assert.AreEqual(file.Name, "ClassLibrary1");
                    } );

            var manager = new DefaultAssemblyManager(workingDirectoryMock.Object, registrationMock.Object, assemblyLoaderMock.Object);
            manager.AddUploadedModules();

            Assert.IsTrue(assemblyLoaded);
            Assert.IsTrue(assemblyCopied);
        }
    }
}
