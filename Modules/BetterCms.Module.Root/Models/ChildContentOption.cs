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
    public class ChildContentOption : EquatableEntity<ChildContentOption>, IOptionEntity, IMultilingualOption
    {
        public ChildContentOption()
        {
            Translations = new List<ChildContentOptionTranslation>();
        }

        public virtual ChildContent ChildContent { get; set; }

        public virtual string Value { get; set; }

        public virtual string Key { get; set; }

        public virtual OptionType Type { get; set; }

        public virtual CustomOption CustomOption { get; set; }

        public virtual IList<ChildContentOptionTranslation> Translations { get; set; }

        IList<IOptionTranslation> IMultilingualOption.Translations
        {
            get
            {
                return Translations.Cast<IOptionTranslation>().ToList();
            }
            set
            {
                Translations = value.Cast<ChildContentOptionTranslation>().ToList();
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

        public virtual ChildContentOption Clone()
        {
            return CopyDataTo(new ChildContentOption());
        }

        public virtual ChildContentOption CopyDataTo(ChildContentOption contentOption, bool copyCollections = true)
        {
            contentOption.Key = Key;
            contentOption.Type = Type;
            contentOption.Value = Value;
            contentOption.CustomOption = CustomOption;

            if (copyCollections && Translations != null)
            {
                foreach (var childContentOptionTranslation in Translations)
                {
                    var clonedTranslation = childContentOptionTranslation.Clone();
                    clonedTranslation.ChildContentOption = contentOption;
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
            return string.Format("{0}, Key: {1}, Value: {2}, Type: {3}", base.ToString(), Key, Value, Type);
        }
    }
}