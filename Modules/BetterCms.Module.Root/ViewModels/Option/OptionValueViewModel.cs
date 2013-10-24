using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Option
{
    [Serializable]
    public class OptionValueViewModel : OptionViewModelBase, IOptionValue
    {
        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        public object OptionValue { get; set; }

        /// <summary>
        /// Gets the option key.
        /// </summary>
        /// <value>
        /// The option key.
        /// </value>
        string IOptionValue.Key
        {
            get
            {
                return OptionKey;
            }
        }

        /// <summary>
        /// Gets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        object IOptionValue.Value
        {
            get { return OptionValue; }
        }

        /// <summary>
        /// Gets or sets the custom option.
        /// </summary>
        /// <value>
        /// The custom option.
        /// </value>
        ICustomOption IOptionValue.CustomOption
        {
            get
            {
                return CustomOption;
            }
            set
            {
                throw new NotSupportedException("IOptionValue.CustomOption has no setter. Use view model");
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("OptionKey: {0}, OptionValue: {1}, Type: {2}", OptionKey, OptionValue, Type);
        }
    }
}