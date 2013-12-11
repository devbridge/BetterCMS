using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class RootEvents
    {
        /// <summary>
        /// Occurs when a culture is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Culture>> CultureCreated;

        /// <summary>
        /// Occurs when a culture is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Culture>> CultureUpdated;

        /// <summary>
        /// Occurs when a culture is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Culture>> CultureDeleted;

        public void OnCultureCreated(params Culture[] cultures)
        {
            if (CultureCreated != null && cultures != null)
            {
                foreach (var culture in cultures)
                {
                    CultureCreated(new SingleItemEventArgs<Culture>(culture));
                }
            }
        }

        public void OnCultureCreated(IEnumerable<Culture> cultures)
        {
            if (cultures != null)
            {
                OnCultureCreated(cultures.ToArray());
            }
        }

        public void OnCultureUpdated(Culture culture)
        {
            if (CultureUpdated != null)
            {
                CultureUpdated(new SingleItemEventArgs<Culture>(culture));
            }
        }

        public void OnCultureDeleted(Culture culture)
        {
            if (CultureDeleted != null)
            {
                CultureDeleted(new SingleItemEventArgs<Culture>(culture));
            }        
        }
    }
}
