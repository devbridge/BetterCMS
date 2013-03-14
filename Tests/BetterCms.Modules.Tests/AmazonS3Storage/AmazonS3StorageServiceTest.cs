using System;

using BetterCms.Configuration;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Services.Storage;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.AmazonS3Storage
{
    [TestFixture]
    public class AmazonS3torageServiceTest
    {
        [Test]
        public void Should_Check_That_File_Not_Exists()
        {
           Assert.Ignore();
        }

        [Test]
        public void Should_Throw_StorageException_If_Given_Uri_Is_Not_Of_File_Scheme()
        {          
        }

        [Test]
        public void Should_Download_Object_Successfully()
        {
            Assert.Ignore();
        }

        [Test]
        public void Should_Upload_Object_Successfully()
        {
            Assert.Ignore();
        }        
    }
}
