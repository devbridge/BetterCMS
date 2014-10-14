using System;

namespace BetterCms.Module.MediaManager.Command.History.RestoreMediaVersion
{
    public class RestoreMediaVersionRequest
    {
        public Guid VersionId { get; set; }

        public bool ShouldOverridUrl { get; set; }
    }
}