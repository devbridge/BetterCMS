using System;

using BetterCms.Api;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Tags;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Commands.SaveTag
{
    /// <summary>
    /// A command to save tag item.
    /// </summary>
    public class SaveTagCommand : CommandBase, ICommand<TagItemViewModel, TagItemViewModel>
    {
        /// <summary>
        /// Executes a command to save tag.
        /// </summary>
        /// <param name="tagItem">The tag item.</param>
        /// <returns>
        /// true if tag saved successfully; false otherwise.
        /// </returns>
        public TagItemViewModel Execute(TagItemViewModel tagItem)
        {
            Tag tag;

            var tagName = Repository.FirstOrDefault<Tag>(c => c.Name == tagItem.Name);
            if (tagName != null && tagName.Id != tagItem.Id)
            {
                var message = string.Format(PagesGlobalization.SaveTag_TagExists_Message, tagItem.Name);
                var logMessage = string.Format("Tag already exists. Tag name: {0}, Id: {1}", tagItem.Name, tagItem.Id);

                throw new ValidationException(() => message, logMessage);
            }

            if (tagItem.Id == default(Guid))
            {
                tag = new Tag();
            }
            else
            {
                tag = Repository.AsProxy<Tag>(tagItem.Id);                
            }

            tag.Version = tagItem.Version;
            tag.Name = tagItem.Name;

            Repository.Save(tag);                       
            UnitOfWork.Commit();

            if (tagItem.Id == default(Guid))
            {
                PagesApiContext.Events.OnTagCreated(tag);
            }
            else
            {
                PagesApiContext.Events.OnTagUpdated(tag);
            }

            return new TagItemViewModel
                {
                    Id = tag.Id,
                    Version = tag.Version,
                    Name = tag.Name
                };
        }
    }
}