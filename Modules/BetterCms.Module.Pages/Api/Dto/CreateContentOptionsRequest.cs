using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Api.Attributes;

namespace BetterCms.Module.Pages.Api.Dto
{
    public class CreateContentOptionsRequest
    {
        public CreateContentOptionsRequest()
        {
            Options = new List<ContentOptionDto>();
        }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        [EmptyGuidValidation(ErrorMessage = "Content Id must be set.")]
        public Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public IList<ContentOptionDto> Options { get; set; }
    }
}