using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Commands.SaveAuthor
{
    public class SaveAuthorCommand : CommandBase, ICommand<AuthorViewModel, AuthorViewModel>
    {
        public AuthorViewModel Execute(AuthorViewModel request)
        {
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                AuthorModel author;

                if (request.Id.HasDefaultValue())
                {
                    author = api.CreateAuthor(
                        new AuthorCreateRequest
                        {
                            Name = request.Name,
                            ImageId = request.Image != null ? request.Image.ImageId : null
                        }).Author;
                }
                else
                {
                    author = api.UpdateAuthor(
                        new AuthorUpdateRequest
                        {
                            AuthorId = request.Id,
                            Version = request.Version,
                            Name = request.Name,
                            ImageId = request.Image != null ? request.Image.ImageId : null
                        }).Author;
                }

                return new AuthorViewModel
                {
                    Id = author.Id,
                    Version = author.Version,
                    Name = author.Name
                };
            }
        }
    }
}