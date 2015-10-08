namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Renders separator element (hr).
    /// </summary>
    public class SeparatorProjection : HtmlElementProjection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatorProjection" /> class.
        /// </summary>
        /// <param name="order">Projection order.</param>
        public SeparatorProjection(int order)
            : base("hr", true)
        {
            Order = order;
        }
    }
}
