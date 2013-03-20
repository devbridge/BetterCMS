using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public abstract class EventsBase
    {
        public delegate void DefaultEventHandler<in TArgs>(TArgs args) where TArgs : EventArgs;
    }
}
