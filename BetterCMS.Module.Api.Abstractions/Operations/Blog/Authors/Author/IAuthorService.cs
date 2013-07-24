namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    public interface IAuthorService
    {
        GetAuthorResponse Get(GetAuthorRequest request);
    }
}