using System;

using BetterCms.Core.Exceptions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Tags;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Tag.GetTag
{
    /// <summary>
    /// A command to get tag item by id.
    /// </summary>
    public class GetTagCommand : CommandBase, ICommand<Guid, TagItemViewModel>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="tagId">The tag id.</param>
        /// <returns>Tag item on null if tag not found.</returns>
        public TagItemViewModel Execute(Guid tagId)
        {
            try
            {
                var tag = Repository.FirstOrDefault<Models.Tag>(f => f.Id == tagId);
                
                if (tag != null)
                {
                    return new TagItemViewModel
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Version = tag.Version
                    };
                }

                return null;
            }
            catch (Exception ex)
            {                
                throw new CmsException(string.Format("Failed to get tag by id={0}.", tagId), ex);
            }            
        }
    }
}