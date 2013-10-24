using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Security;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class CoreEvents : EventsBase<CoreEvents>
    {
        /// <summary>
        /// Occurs when a CMS host starts.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<HttpApplication>> HostStart;

        public event DefaultEventHandler<SingleItemEventArgs<HttpApplication>> HostStop;

        public event DefaultEventHandler<SingleItemEventArgs<HttpApplication>> HostError;

        public event DefaultEventHandler<SingleItemEventArgs<HttpApplication>> HostAuthenticateRequest;

        public event DefaultEventHandler<SingleItemEventArgs<IEntity>> EntitySaving;

        public event DefaultEventHandler<SingleItemEventArgs<IEntity>> EntityDeleting;


        /// <summary>
        /// Called when a CMS host starts.
        /// </summary>
        /// <param name="host">The CMS host.</param>
        public void OnHostStart(HttpApplication host)
        {
            if (HostStart != null)
            {
                HostStart(new SingleItemEventArgs<HttpApplication>(host));
            }
        }

        /// <summary>
        /// Called when a CMS host stops.
        /// </summary>
        /// <param name="host">The CMS host.</param>
        public void OnHostStop(HttpApplication host)
        {
            if (HostStop != null)
            {
                HostStop(new SingleItemEventArgs<HttpApplication>(host));
            }
        }

        /// <summary>
        /// Called when a CMS host throws error.
        /// </summary>
        /// <param name="host">The CMS host.</param>
        public void OnHostError(HttpApplication host)
        {
            if (HostError != null)
            {
                HostError(new SingleItemEventArgs<HttpApplication>(host));
            }
        }

        /// <summary>
        /// Called when a CMS host authenticates request.
        /// </summary>
        /// <param name="host">The CMS host.</param>
        public void OnHostAuthenticateRequest(HttpApplication host)
        {
            if (HostAuthenticateRequest != null)
            {
                HostAuthenticateRequest(new SingleItemEventArgs<HttpApplication>(host));
            }
        }

        /// <summary>
        /// Called before an entity is saved.
        /// </summary>
        /// <param name="accessSecuredObject">The access secured object.</param>
        public void OnEntitySaving(IEntity accessSecuredObject)
        {
            if (EntitySaving != null)
            {
                EntitySaving(new SingleItemEventArgs<IEntity>(accessSecuredObject));
            }
        }

        /// <summary>
        /// Called before an entity is deleted.
        /// </summary>
        /// <param name="accessSecuredObject">The access secured object.</param>
        public void OnEntityDelete(IEntity accessSecuredObject)
        {
            if (EntityDeleting != null)
            {
                EntityDeleting(new SingleItemEventArgs<IEntity>(accessSecuredObject));
            }
        }    
    }
}
