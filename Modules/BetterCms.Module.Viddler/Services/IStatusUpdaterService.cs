using System;
using System.Collections.Generic;

namespace BetterCms.Module.Viddler.Services
{
    /// <summary>
    /// Viddler service interface.
    /// </summary>
    public interface IStatusUpdaterService
    {
        void UpdateStatus(IList<Guid> mediasIds);
    }
}