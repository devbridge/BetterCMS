namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    public interface ITagService
    {
        GetTagResponse Get(GetTagRequest request);

        PostTagResponse Update(PostTagRequest request);
    }
}
