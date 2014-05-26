using System;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Models;

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