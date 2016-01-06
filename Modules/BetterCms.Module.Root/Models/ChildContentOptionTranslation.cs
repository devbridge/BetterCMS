using System;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ChildContentOptionTranslation : EquatableEntity<ChildContentOptionTranslation>, IOptionTranslationEntity
    {
        public virtual ChildContentOption ChildContentOption { get; set; }

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
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, ChildContentOption: {1}, Language: {2}, Value: {3}", base.ToString(), ChildContentOption, Language, Value);
        }

        ILanguage IOptionTranslationEntity.Language
        {
            get
            {
                return Language;
            }
            set
            {
                Language = (Language)value;
            }
        }

        IOptionEntity IOptionTranslationEntity.Option
        {
            get
            {
                return ChildContentOption;
            }
            set
            {
                ChildContentOption = (ChildContentOption)value;
            }
        }

        public virtual ChildContentOptionTranslation Clone()
        {
            return CopyDataTo(new ChildContentOptionTranslation());
        }

        private ChildContentOptionTranslation CopyDataTo(ChildContentOptionTranslation childContentOptionTranslation)
        {
            childContentOptionTranslation.ChildContentOption = ChildContentOption;
            childContentOptionTranslation.Value = Value;
            childContentOptionTranslation.Language = Language;

            return childContentOptionTranslation;
        }
    }
}