using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Option
{
    /// <summary>
    /// Option view model
    /// </summary>
    public class OptionViewModel : OptionViewModelBase, IOption
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

        #region IOption Members
        string IOption.Key
        {
            get
            {
                return OptionKey;
            }
            set
            {
                OptionKey = value;
            }
        }

        Core.DataContracts.Enums.OptionType IOption.Type
        {
            get
            {
                return Type;
            }
            set
            {
                Type = value;
            }
        }

        string IOption.Value
        {
            get
            {
                return OptionDefaultValue;
            }
            set
            {
                OptionDefaultValue = value;
            }
        }

        System.Guid IEntity.Id
        {
            get { throw new System.NotSupportedException(); }
        }

        bool IEntity.IsDeleted
        {
            get { throw new System.NotSupportedException(); }
        }

        System.DateTime IEntity.CreatedOn
        {
            get { throw new System.NotSupportedException(); }
        }

        System.DateTime IEntity.ModifiedOn
        {
            get { throw new System.NotSupportedException(); }
        }

        System.DateTime? IEntity.DeletedOn
        {
            get { throw new System.NotSupportedException(); }
        }

        string IEntity.CreatedByUser
        {
            get { throw new System.NotSupportedException(); }
        }

        string IEntity.ModifiedByUser
        {
            get { throw new System.NotSupportedException(); }
        }

        string IEntity.DeletedByUser
        {
            get { throw new System.NotSupportedException(); }
        }

        int IEntity.Version
        {
            get
            {
                throw new System.NotSupportedException();
            }
            set
            {
                throw new System.NotSupportedException();
            }
        }
        #endregion
    }
}