using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentOption : EquatableEntity<ContentOption>, IDeletableOption<Content>, IMultilingualOption
    {
        public ContentOption()
        {
            Translations = new List<ContentOptionTranslation>();
            IsDeletable = true;
        }

        public virtual Content Content { get; set; }

        public virtual string Key { get; set; }

        public virtual OptionType Type { get; set; }

        public virtual string DefaultValue { get; set; }

        public virtual bool IsDeletable { get; set; }

        public virtual CustomOption CustomOption { get; set; }

        public virtual IList<ContentOptionTranslation> Translations { get; set; }

        IList<IOptionTranslation> IMultilingualOption.Translations
        {
            get
            {
                return Translations.Cast<IOptionTranslation>().ToList();
            }
            set
            {
                Translations = value.Cast<ContentOptionTranslation>().ToList();
            }
        }

        string IOption.Value
        {
            get
            {
                return DefaultValue;
            }
            set
            {
                DefaultValue = value;
            }
        }

        Content IDeletableOption<Content>.Entity
        {
            get
            {
                return Content;
            }
            set
            {
                Content = value;
            }
        }

        ICustomOption IOption.CustomOption
        {
            get
            {
                return CustomOption;
            }
            set
            {
                CustomOption = (CustomOption)value;
            }
        }

        public virtual ContentOption Clone()
        {
            return CopyDataTo(new ContentOption());
        }

        public virtual ContentOption CopyDataTo(ContentOption contentOption, bool copyCollections = true)
        {
            contentOption.Key = Key;
            contentOption.Type = Type;
            contentOption.DefaultValue = DefaultValue;
            contentOption.IsDeletable = IsDeletable;
            contentOption.Content = Content;

            if (copyCollections && Translations != null)
            {
                foreach (var contentOptionTranslation in Translations)
                {
                    var clonedTranslation = contentOptionTranslation.Clone();
                    clonedTranslation.ContentOption = contentOption;
                    contentOption.Translations.Add(clonedTranslation);
                }
            }

            return contentOption;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Key: {1}, DefaultValue: {2}, Type: {3}", base.ToString(), Key, DefaultValue, Type);
        }
    }
}