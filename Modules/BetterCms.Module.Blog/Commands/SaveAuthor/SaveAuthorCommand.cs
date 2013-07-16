using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Commands.SaveAuthor
{
    public class SaveAuthorCommand : CommandBase, ICommand<AuthorViewModel, AuthorViewModel>
    {
        private IAuthorService authorService;

        public SaveAuthorCommand(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public AuthorViewModel Execute(AuthorViewModel request)
        {
            Author author;

            if (request.Id.HasDefaultValue())
            {
                author = authorService.CreateAuthor(request.Name, request.Image != null ? request.Image.ImageId : null);
            }
            else
            {
                author = authorService.UpdateAuthor(request.Id, request.Version, request.Name, request.Image != null ? request.Image.ImageId : null);
            }

            return new AuthorViewModel {
                                           Id = author.Id,
                                           Version = author.Version,
                                           Name = author.Name
                                       };
        }
    }
}