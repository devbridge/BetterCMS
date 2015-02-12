using System.Web;

// ReSharper disable CheckNamespace
namespace Devbridge.Platform.Events
// ReSharper restore CheckNamespace
{
    public class WebCoreEvents : EventsBase<WebCoreEvents>
    {
        /// <summary>
        /// Occurs when a host starts.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<HttpApplication>> HostStart;

        public event DefaultEventHandler<SingleItemEventArgs<HttpApplication>> HostStop;

        public event DefaultEventHandler<SingleItemEventArgs<HttpApplication>> HostError;

        public event DefaultEventHandler<SingleItemEventArgs<HttpApplication>> HostAuthenticateRequest;

        /// <summary>
        /// Called when a host starts.
        /// </summary>
        /// <param name="host">The host.</param>
        public void OnHostStart(HttpApplication host)
        {
            if (HostStart != null)
            {
                HostStart(new SingleItemEventArgs<HttpApplication>(host));
            }
        }

        /// <summary>
        /// Called when a host stops.
        /// </summary>
        /// <param name="host">The host.</param>
        public void OnHostStop(HttpApplication host)
        {
            if (HostStop != null)
            {
                HostStop(new SingleItemEventArgs<HttpApplication>(host));
            }
        }

        /// <summary>
        /// Called when a host throws error.
        /// </summary>
        /// <param name="host">The host.</param>
        public void OnHostError(HttpApplication host)
        {
            if (HostError != null)
            {
                HostError(new SingleItemEventArgs<HttpApplication>(host));
            }
        }

        /// <summary>
        /// Called when a host authenticates request.
        /// </summary>
        /// <param name="host">The host.</param>
        public void OnHostAuthenticateRequest(HttpApplication host)
        {
            if (HostAuthenticateRequest != null)
            {
                HostAuthenticateRequest(new SingleItemEventArgs<HttpApplication>(host));
            }
        }
    }
}
