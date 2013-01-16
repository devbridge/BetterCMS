using System;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// Generic communication object to transfer information between Ajax posts.
    /// </summary>
    [Serializable]
    public class ComboWireJson : WireJson
    {
        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        public string Html { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WireJson" /> class.
        /// </summary>
        /// <param name="success">Marks success flag.</param>
        /// <param name="html">HTML to render in client side.</param>
        /// <param name="data">The data.</param>
        /// <param name="messages">The messages.</param>
        public ComboWireJson(bool success, string html, dynamic data, params string[] messages)
        {
            Success = success;
            Html = html;
            DataType = "combo";
            Data = data;
            Messages = messages;
        }
    }
}