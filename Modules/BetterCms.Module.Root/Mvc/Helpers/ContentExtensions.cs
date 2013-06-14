using System.Linq;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class ContentExtensions
    {
        public static Models.Content FindEditableContentVersion(this Models.Content content)
        {
            Models.Content contentForEdit = null;

            if (content.Status == ContentStatus.Draft)
            {
                contentForEdit = content;
            }
            else if (content.History != null)
            {
                contentForEdit = content.History.FirstOrDefault(f => f.Status == ContentStatus.Draft);
            }

            if (contentForEdit == null && content.Status == ContentStatus.Published)
            {
                contentForEdit = content;
            }

            return contentForEdit;
        }
    }
}