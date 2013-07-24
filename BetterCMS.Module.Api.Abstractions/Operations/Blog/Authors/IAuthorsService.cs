namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    public interface IAuthorsService
    {
        GetAuthorsResponse Get(GetAuthorsRequest request);
    }
}