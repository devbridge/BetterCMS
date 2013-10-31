using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Security;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    [Serializable]
    public class RenderPageViewModel : IRenderPage, IAccessSecuredObject, IAccessSecuredViewModel
    {
        public RenderPageViewModel(IPage page)
        {
            var rootPage = page as Page;

            Id = page.Id;
            IsDeleted = page.IsDeleted;
            Version = page.Version;
            HasSEO = page.HasSEO;
            Title = page.Title;
            MetaTitle = rootPage != null && !string.IsNullOrEmpty(rootPage.MetaTitle) ? rootPage.MetaTitle : Title;
            PageUrl = page.PageUrl;
            Status = page.Status;
            CreatedOn = page.CreatedOn;
            CreatedByUser = page.CreatedByUser;
            ModifiedOn = page.ModifiedOn;
            ModifiedByUser = page.ModifiedByUser;

            Bag = new DynamicDictionary();
        }

        public RenderPageViewModel()
        {
        }

        public Guid Id { get; private set; }

        public bool IsDeleted { get; private set; }

        public int Version { get; private set; }
        
        public DateTime CreatedOn { get; private set; }
        
        public DateTime ModifiedOn { get; private set; }
        
        public string CreatedByUser { get; private set; }
        
        public string ModifiedByUser { get; private set; }

        public PageStatus Status { get; private set; }
        
        public bool IsPublic { get; private set; }

        public bool HasSEO { get; private set; }

        public string Title { get; private set; }

        public string MetaTitle { get; private set; }

        public string PageUrl { get; private set; }

        /// <summary>
        /// Gets the entity id.
        /// </summary>
        /// <value>
        /// The entity id.
        /// </value>
        Guid IAccessSecuredObject.Id
        {
            get
            {
                return Id;
            }
        }

        /// <summary>
        /// Gets the entity version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        int IEntity.Version
        {
            get
            {
                return Version;
            }
            set
            {
                throw new NotSupportedException();                
            }
        }

        /// <summary>
        /// Gets the deleted on date.
        /// </summary>
        /// <value>
        /// The deleted on.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        DateTime? IEntity.DeletedOn
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the deleted by user.
        /// </summary>
        /// <value>
        /// The deleted by user.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        string IEntity.DeletedByUser
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the layout path.
        /// </summary>
        /// <value>
        /// The layout path.
        /// </value>
        public string LayoutPath { get; set; }

        /// <summary>
        /// Gets or sets the master page.
        /// </summary>
        /// <value>
        /// The master page.
        /// </value>
        public RenderPageViewModel MasterPage { get; set; }

        // TODO: find better solution
        public RenderPageViewModel RenderingPage { get; set; }

        /// <summary>
        /// Gets or sets page content projections list.
        /// </summary>
        /// <value>
        /// The list of page content projections.
        /// </value>
        public List<PageContentProjection> Contents { get; set; }

        /// <summary>
        /// Gets or sets the layout regions.
        /// </summary>
        /// <value>
        /// The layout regions.
        /// </value>
        public List<PageRegionViewModel> Regions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether regions and contents can be edited for current page.
        /// </summary>
        /// <value>
        /// <c>true</c> if regions and contents can be edited for current page; otherwise, <c>false</c>.
        /// </value>
        public bool AreRegionsEditable { get; set; }

        /// <summary>
        /// Gets or sets the page options.
        /// </summary>
        /// <value>
        /// The page options.
        /// </value>
        public IEnumerable<IOptionValue> Options
        {
            get
            {
                return OptionsAsDictionary.Values;
            }

            set
            {
                OptionsAsDictionary = new Dictionary<string, IOptionValue>();
                foreach (var optionValue in value.Distinct())
                {
                    OptionsAsDictionary.Add(optionValue.Key, optionValue);
                }
            }
        }

        /// <summary>
        /// Gets or sets the options as dictionary.
        /// </summary>
        /// <value>
        /// The options as dictionary.
        /// </value>
        public IDictionary<string, IOptionValue> OptionsAsDictionary { get; set; }

        /// <summary>
        /// Gets or sets the list of meta data projections.
        /// </summary>
        /// <value>
        /// The list of meta data projections.
        /// </value>
        public List<IPageActionProjection> Metadata { get; set; }

        /// <summary>
        /// Gets or sets the list of page style projections.
        /// </summary>
        /// <value>
        /// The page style projections.
        /// </value>
        public IList<IStylesheetAccessor> Stylesheets { get; set; }

        /// <summary>
        /// Gets or sets the java scripts.
        /// </summary>
        /// <value>
        /// The java scripts.
        /// </value>
        public IList<IJavaScriptAccessor> JavaScripts { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        public IList<IAccessRule> AccessRules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current user can manage page content.
        /// </summary>
        /// <value>
        /// <c>true</c> if whether current user can manage page content; otherwise, <c>false</c>.
        /// </value>
        public bool CanManageContent { get; set; }

        /// <summary>
        /// Gets or sets the path to a require.js file.
        /// </summary>
        /// <value>
        /// The path to require.js file.
        /// </value>
        public string RequireJsPath { get; set; }

        /// <summary>
        /// Gets or sets the path to a bcms.main.js file.
        /// </summary>
        /// <value>
        /// The path to a bcms.main.js file.
        /// </value>
        public string MainJsPath { get; set; }

        /// <summary>
        /// Gets or sets the path to a html5shiv.js file.
        /// </summary>
        /// <value>
        /// The path to a html5shiv.js file.
        /// </value>
        public string Html5ShivJsPath { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public dynamic Bag { get; set; }

        /// <summary>
        /// Removes the rule.
        /// </summary>
        /// <param name="accessRule">The access rule.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        void IAccessSecuredObject.RemoveRule(IAccessRule accessRule)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds the rule.
        /// </summary>
        /// <param name="accessRule">The access rule.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        void IAccessSecuredObject.AddRule(IAccessRule accessRule)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Title: {1}, PageUrl: {2}, LayoutPath: {3}", Id, Title, PageUrl, LayoutPath);
        }

        /// <summary>
        /// Gets or sets a value indicating whether page should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if page should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        public bool HasEditRole { get; set; }

        public bool SaveUnsecured { get; set; }
    }
}