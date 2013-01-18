using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Commands.SaveAuthor
{
    public class SaveAuthorCommand : CommandBase, ICommand<AuthorViewModel, AuthorViewModel>
    {
        public AuthorViewModel Execute(AuthorViewModel request)
        {
            var isNew = request.Id.HasDefaultValue();
            Author author;

            if (isNew)
            {
                author = new Author();
            }
            else
            {
                author = Repository.First<Author>(request.Id);
            }

            author.Name = request.Name;
            author.Version = request.Version;

            if (request.Image != null && request.Image.ImageId.HasValue)
            {
                author.Image = Repository.AsProxy<MediaImage>(request.Image.ImageId.Value);
            }
            else
            {
                author.Image = null;
            }

            Repository.Save(author);
            UnitOfWork.Commit();

            return new AuthorViewModel
                       {
                           Id = author.Id, 
                           Version = author.Version,
                           Name = author.Name
                       };
        }
    }
}