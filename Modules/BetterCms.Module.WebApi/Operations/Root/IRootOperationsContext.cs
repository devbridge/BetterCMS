using BetterCms.Module.Api.Operations.Root.Version;

namespace BetterCms.Module.Api.Operations.Root
{
    public interface IRootOperationsContext
    {
        GetVersionResponse GetVersion();

        GetTags.GetTagsResponse GetTags(GetTags.GetTagsRequest request);

        GetTagByName.GetTagByNameResponse GetTagByName(GetTagByName.GetTagByNameRequest request);
    }
}
