using System;
using System.Collections.Generic;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class SectionElement
    {
        public SectionElement()
        {
            Links = new List<LinkElement>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        public IList<LinkElement> Links { get; set; } 
    }
}