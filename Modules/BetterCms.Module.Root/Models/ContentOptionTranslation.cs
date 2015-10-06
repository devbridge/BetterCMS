using System;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentOptionTranslation : EquatableEntity<ContentOptionTranslation>, IOptionTranslation
    {
        public virtual ContentOption ContentOption { get; set; }

        public virtual Language Language { get; set; }

        public virtual string Value { get; set; }

        string IOptionTranslation.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        string IOptionTranslation.LanguageId
        {
            get
            {
                return Language.Id.ToString();
            }
            set
            {
                throw new NotImplementedException();
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
            return string.Format("{0}, ContentOption: {1}, Language: {2}, Value: {3}", base.ToString(), ContentOption, Language, Value);
        }
    }
}