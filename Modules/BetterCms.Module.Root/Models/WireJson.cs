using System;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// Generic communication object to transfer information between Ajax posts.
    /// </summary>
    [Serializable]
    public class WireJson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WireJson" /> class.
        /// </summary>
        public WireJson()
        {
            DataType = "html";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WireJson" /> class.
        /// </summary>
        /// <param name="success">Marks success flag.</param>
        /// <param name="messages">Messages to show.</param>
        public WireJson(bool success, params string[] messages) : this()
        {
            Success = success;
            Messages = messages;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WireJson" /> class.
        /// </summary>
        /// <param name="success">Marks success flag.</param>
        /// <param name="html">HTML to render in client side.</param>
        /// <param name="messages">Messages to show.</param>
        public WireJson(bool success, string html, params string[] messages)
        {
            Success = success;
            Messages = messages;
            DataType = "html";
            Data = html;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WireJson" /> class.
        /// </summary>
        /// <param name="success">Marks success flag.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="data">The data.</param>
        /// <param name="messages">Messages to show.</param>
        public WireJson(bool success, string dataType, string data, string[] messages)
        {
            Success = success;
            Messages = messages;
            DataType = dataType;
            Data = data;
        }

        public WireJson(bool success, dynamic data)
        {
            Success = success;
            Data = data;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WireJson" /> response is in success state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets messages list.
        /// </summary>
        /// <value>
        /// A list of messages to render in client side.
        /// </value>
        public string[] Messages { get; set; }

        /// <summary>
        /// Gets or sets the type of the returned data. Default is 'html'.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the returning data.
        /// </summary>
        /// <value>
        /// The returning data.
        /// </value>
        public object Data { get; set; }
    }
}