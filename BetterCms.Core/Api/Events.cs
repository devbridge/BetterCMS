using System;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{   
    public class SingleItemEventArgs<TItem> : EventArgs
    {
        public TItem Item { get; set; }

        public SingleItemEventArgs(TItem item)
        {
            Item = item;
        }
    }

    public abstract class EventsBase
    {
        public delegate void DefaultEventHandler<in TArgs>(TArgs args) where TArgs : EventArgs;
    }
}
