namespace BetterCms.Module.Root.ViewModels.Tags
{
    public class TagsTemplateViewModel
    {
        /// <summary>
        /// Gets or sets the tooltip description.
        /// </summary>
        /// <value>
        /// The tooltip description.
        /// </value>
        public string TooltipDescription { get; set; }

        public override string ToString()
        {
            return string.Format("Tooltip description: {0}", TooltipDescription);
        }
    }
}