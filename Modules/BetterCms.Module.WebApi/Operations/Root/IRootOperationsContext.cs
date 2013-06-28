using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;
using BetterCms.Module.Api.Operations.Root.Version;

namespace BetterCms.Module.Api.Operations.Root
{
    public interface IRootOperationsContext
    {
        TagsService Tags { get; }

        TagService Tag { get; }
    }
}
