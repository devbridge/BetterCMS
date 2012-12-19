using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(LinkCollection), AddItemName = LinksNodeName, CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SectionElement : ConfigurationElementCollection
    {
        private const string NameAttribute = "name";
        private const string LinksNodeName = "links";

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [ConfigurationProperty(NameAttribute, IsRequired = true)]
        public string Name
        {
            get { return Convert.ToString(this[NameAttribute]); }
            set { this[NameAttribute] = value; }
        }

        /// <summary>
        /// Gets the links.
        /// </summary>
        [ConfigurationProperty(LinksNodeName, IsRequired = true)]
        public LinkCollection Links
        {
            get { return (LinkCollection)base[LinksNodeName]; }
        }

        #endregion

        #region Indexers

        public LinkCollection this[int index]
        {
            get
            {
                return (LinkCollection)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        #endregion

        #region Overrides

        protected override ConfigurationElement CreateNewElement()
        {
            return new LinkElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as LinkElement).Name;
        }

        protected override string ElementName
        {
            get { return "section"; }
        }

        #endregion
    }
}