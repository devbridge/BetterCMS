using System;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentOptionTranslation : EquatableEntity<ContentOptionTranslation>, IOptionTranslationEntity
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
                return ContentOption;
            }
            set
            {
                ContentOption = (ContentOption)value;
            }
        }

        public virtual ContentOptionTranslation Clone()
        {
            return CopyDataTo(new ContentOptionTranslation());
        }

        public virtual ContentOptionTranslation CopyDataTo(ContentOptionTranslation contentOptionTranslation)
        {
            contentOptionTranslation.ContentOption = ContentOption;
            contentOptionTranslation.Value = Value;
            contentOptionTranslation.Language = Language;

            return contentOptionTranslation;
        }
    }
}