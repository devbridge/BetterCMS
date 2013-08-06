namespace BetterCms.Module.Root.ViewModels.Option
{
    public class OptionValueViewModel : OptionViewModel
    {
        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        public string OptionValue { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, OptionValue: {1}", base.ToString(), OptionValue);
        }
    }
}