using Devbridge.Platform.Core.DataContracts;
using Devbridge.Platform.Events;

using Moq;

using NHibernate;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Events
{
    [TestFixture]
    public class EntitySavingEventArgsTests : TestBase
    {
        [Test]
        public void Should_Assign_Correct_Session_Arg()
        {
            var session = new Mock<ISession>().Object;
            var entity = new Mock<IEntity>().Object;

            var args = new EntitySavingEventArgs(entity, session);

            Assert.AreEqual(args.Session, session);
            Assert.AreEqual(args.Entity, entity);
        }
    }
}
