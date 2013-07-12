using System;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Tags;

namespace BetterCms.Module.Root.Commands.Tag.SaveTag
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
            Models.Tag tag;

            var tagName = Repository.FirstOrDefault<Models.Tag>(c => c.Name == tagItem.Name.Trim());
            if (tagName != null && tagName.Id != tagItem.Id)
            {
                var message = string.Format(RootGlobalization.SaveTag_TagExists_Message, tagItem.Name);
                var logMessage = string.Format("Tag already exists. Tag name: {0}, Id: {1}", tagItem.Name, tagItem.Id);

                throw new ValidationException(() => message, logMessage);
            }

            if (tagItem.Id == default(Guid))
            {
                tag = new Models.Tag();
            }
            else
            {
                tag = Repository.AsProxy<Models.Tag>(tagItem.Id);                
            }

            tag.Version = tagItem.Version;
            tag.Name = tagItem.Name.Trim();

            Repository.Save(tag);                       
            UnitOfWork.Commit();

            if (tagItem.Id == default(Guid))
            {
                Events.RootEvents.Instance.OnTagCreated(tag);
            }
            else
            {
                Events.RootEvents.Instance.OnTagUpdated(tag);
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