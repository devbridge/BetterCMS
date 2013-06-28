
using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

namespace BetterCms.Module.Api.Operations.Root
{
    public class DefaultRootOperationsContext : IRootOperationsContext
    {
        public TagsService Tags
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public TagService Tag
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
    }
}