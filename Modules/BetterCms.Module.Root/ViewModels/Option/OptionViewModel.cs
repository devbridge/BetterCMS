namespace BetterCms.Module.Root.ViewModels.Option
{
    /// <summary>
    /// Option view model
    /// </summary>
    public class OptionViewModel : OptionViewModelBase
    {
        /// <summary>
        /// Gets or sets the option default value.
        /// </summary>
        /// <value>
        /// The option default value.
        /// </value>
        public string OptionDefaultValue { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("OptionKey: {0}, OptionDefaultValue: {1}, Type: {2}", OptionKey, OptionDefaultValue, Type);
        }
    }
}