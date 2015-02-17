using Devbridge.Platform.Core.DataContracts;
using Devbridge.Platform.Core.Models;
using Devbridge.Platform.Events;

using Moq;

using NHibernate;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Events
{
    [TestFixture]
    public class CoreEventsTests : TestBase
    {
        private int firedDelete;
        private int firedSave;
        private IEntity entity;
        private ISession session;

        [Test]
        public void Should_FireDeleteEvents_Correctly()
        {
            firedDelete = 0;
            entity = new Mock<IEntity>().Object;
            
            CoreEvents.Instance.EntityDeleting += Instance_EntityDeleting;

            Assert.AreEqual(firedDelete, 0);
            CoreEvents.Instance.OnEntityDelete(entity);
            Assert.AreEqual(firedDelete, 1);
            CoreEvents.Instance.OnEntityDelete(entity);
            Assert.AreEqual(firedDelete, 2);

            CoreEvents.Instance.EntityDeleting -= Instance_EntityDeleting;
        }
        
        [Test]
        public void Should_FireSaveEvents_Correctly()
        {
            firedSave = 0;
            entity = new Mock<IEntity>().Object;
            session = new Mock<ISession>().Object;

            CoreEvents.Instance.EntitySaving += Instance_EntitySaving;

            Assert.AreEqual(firedSave, 0);
            CoreEvents.Instance.OnEntitySaving(entity, session);
            Assert.AreEqual(firedSave, 1);
            CoreEvents.Instance.OnEntitySaving(entity, session);
            Assert.AreEqual(firedSave, 2);

            CoreEvents.Instance.EntitySaving -= Instance_EntitySaving;
        }

        void Instance_EntitySaving(EntitySavingEventArgs args)
        {
            firedSave ++;
        }

        void Instance_EntityDeleting(SingleItemEventArgs<DataContracts.IEntity> args)
        {
            firedDelete ++;
        }
    }
}
