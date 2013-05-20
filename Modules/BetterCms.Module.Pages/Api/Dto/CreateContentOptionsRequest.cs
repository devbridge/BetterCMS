using System;
using System.Collections.Generic;

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