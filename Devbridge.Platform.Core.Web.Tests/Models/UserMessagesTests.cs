using System;
using System.Collections.Generic;
using System.Linq;

using Devbridge.Platform.Core.Web.Models;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Models
{
    [TestFixture]
    public class UserMessagesTests : TestBase
    {
        [Test]
        public void Should_Create_Empty_Warn_Messages()
        {
            var messages = new UserMessages();

            Assert.IsNotNull(messages.Error);
            Assert.IsNotNull(messages.Success);
            Assert.IsNotNull(messages.Warn);
            Assert.IsNotNull(messages.Info);

            Assert.IsEmpty(messages.Error);
            Assert.IsEmpty(messages.Success);
            Assert.IsEmpty(messages.Warn);
            Assert.IsEmpty(messages.Info);
        }
        
        [Test]
        public void Should_Add_Return_Messages_Correctly()
        {
            var messages = new UserMessages();

            Assert.IsNotNull(messages.Error);
            Assert.IsNotNull(messages.Success);
            Assert.IsNotNull(messages.Warn);
            Assert.IsNotNull(messages.Info);

            messages.AddError("E1");
            messages.AddError("E2");
            messages.AddWarn("W1");
            messages.AddWarn("W2");
            messages.AddSuccess("S1");
            messages.AddSuccess("S2");
            messages.AddInfo("I1");
            messages.AddInfo("I2");

            Assert.AreEqual(messages.Error.Count, 2);
            Assert.IsTrue(messages.Error.Any(e => e == "E1"));
            Assert.IsTrue(messages.Error.Any(e => e == "E2"));
            
            Assert.AreEqual(messages.Warn.Count, 2);
            Assert.IsTrue(messages.Warn.Any(e => e == "W1"));
            Assert.IsTrue(messages.Warn.Any(e => e == "W2"));
            
            Assert.AreEqual(messages.Success.Count, 2);
            Assert.IsTrue(messages.Success.Any(e => e == "S1"));
            Assert.IsTrue(messages.Success.Any(e => e == "S2"));
            
            Assert.AreEqual(messages.Info.Count, 2);
            Assert.IsTrue(messages.Info.Any(e => e == "I1"));
            Assert.IsTrue(messages.Info.Any(e => e == "I2"));
        }
        
        [Test]
        public void Should_Clear_Messages_Correctly()
        {
            var messages = new UserMessages();

            Assert.IsNotNull(messages.Error);
            Assert.IsNotNull(messages.Success);
            Assert.IsNotNull(messages.Warn);
            Assert.IsNotNull(messages.Info);

            messages.AddError("M");
            messages.AddSuccess("M");
            messages.AddInfo("M");
            messages.AddWarn("M");
            
            Assert.AreEqual(messages.Error.Count, 1);
            Assert.AreEqual(messages.Info.Count, 1);
            Assert.AreEqual(messages.Warn.Count, 1);
            Assert.AreEqual(messages.Success.Count, 1);
            
            messages.Clear();

            Assert.IsEmpty(messages.Error);
            Assert.IsEmpty(messages.Success);
            Assert.IsEmpty(messages.Warn);
            Assert.IsEmpty(messages.Info);
        }

        /// <summary>
        /// Should not to allow edit the collection (add new item)
        /// </summary>
        [Test]
        public void Should_Throw_ReadOnly_Exception()
        {
            var thrown = 0;

            var messages = new UserMessages();

            new List<Action>
                {
                    () => messages.Info.Add("M"),
                    () => messages.Warn.Add("M"),
                    () => messages.Success.Add("M"),
                    () => messages.Error.Add("M")
                }.ForEach(
                action =>
                {
                    try
                    {
                        action();
                    }
                    catch (NotSupportedException)
                    {
                        thrown++;
                    }
                });

            Assert.AreEqual(thrown, 4);
        }
    }
}
