using System;

using BetterCms.Module.Pages.Command.Widget.SaveWidget;

namespace BetterCms.Module.Pages.Command.History.DestroyContentDraft
{
    public class DestroyContentDraftCommandResponse : SaveWidgetResponse
    {
        public Guid PublishedId { get; set; }
    }
}