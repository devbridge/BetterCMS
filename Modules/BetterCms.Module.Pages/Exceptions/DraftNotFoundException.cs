using System;

using Devbridge.Platform.Core.Exceptions.DataTier;
using Devbridge.Platform.Core.Models;

namespace BetterCms.Module.Pages.Exceptions
{
    public class DraftNotFoundException : ConcurrentDataException
    {
        public DraftNotFoundException(string message)
            : base(message)
        {
        }

        public DraftNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DraftNotFoundException(Entity staleEntity)
            : base(staleEntity)
        {
        }
    }
}