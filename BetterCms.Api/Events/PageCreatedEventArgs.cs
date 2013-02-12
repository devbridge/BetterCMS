using System;
using BetterCms.Core.Models;

namespace BetterCms.Api.Events
{
    public class PageCreatedEventArgs : EventArgs
    {
        public IPage Page { get; set; }

        public delegate void PageCreatedEventHandler(object sender, PageCreatedEventArgs args);
    }
}
