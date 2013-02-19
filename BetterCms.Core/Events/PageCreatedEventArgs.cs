using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Events
{
    public class PageCreatedEventArgs : EventArgs
    {
        public IPage Page { get; set; }

        public delegate void PageCreatedEventHandler(object sender, PageCreatedEventArgs args);
    }
}
