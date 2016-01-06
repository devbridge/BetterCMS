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

using BetterModules.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    /// <summary>
    /// Represents view model for rendering page. Includes page data, contents, scripts, stylesheets, master pages, etc.
    /// </summary>
    [Serializable]
    public class RenderPageViewModel : IRenderPage, IAccessSecuredObject, IAccessSecuredViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPageViewModel" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public RenderPageViewModel(IPage page) : this()
        {
            var rootPage = page as Page;

            Id = page.Id;
            IsDeleted = page.IsDeleted;
            Version = page.Version;
            HasSEO = page.HasSEO;
            Title = page.Title;
            MetaTitle = rootPage != null && !string.IsNullOrEmpty(rootPage.MetaTitle) ? rootPage.MetaTitle : Title;
            MetaDescription = rootPage != null ? rootPage.MetaDescription : null;
            MetaKeywords = rootPage != null ? rootPage.MetaKeywords : null;
            PageUrl = page.PageUrl;
            Status = page.Status;
            CreatedOn = page.CreatedOn;
            CreatedByUser = page.CreatedByUser;
            ModifiedOn = page.ModifiedOn;
            ModifiedByUser = page.ModifiedByUser;
            IsMasterPage = page.IsMasterPage;
            ForceAccessProtocol = page.ForceAccessProtocol;

            if (rootPage != null && rootPage.Language != null)
            {
                LanguageCode = rootPage.Language.Code;
                LanguageId = rootPage.Language.Id;
            }

            PageData = page;

            RenderedPageContents = new List<Guid>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPageViewModel" /> class.
        /// </summary>
        public RenderPageViewModel()
        {
            Bag = new DynamicDictionary();

            RenderedPageContents = new List<Guid>();
        }

        public IPage PageData { get; private set; }

        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public int Version { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public DateTime ModifiedOn { get; set; }
        
        public string CreatedByUser { get; set; }
        
        public string ModifiedByUser { get; set; }

        public PageStatus Status { get; set; }

        public bool HasSEO { get; set; }

        public string Title { get; set; }
        
        public bool IsMasterPage { get; set; }

        public bool IsPreviewing { get; set; }

        public ForceProtocolType ForceAccessProtocol { get; set; }

        public string MetaTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string PageUrl { get; set; }

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

        /// <summary>
        /// Gets or sets the rendering page (the most child page).
        /// </summary>
        /// <value>
        /// The rendering page (the most child page).
        /// </value>
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
                if (OptionsAsDictionary == null)
                {
                    OptionsAsDictionary = new Dictionary<string, IOptionValue>();
                }
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
        /// Gets or sets the language code.
        /// </summary>
        /// <value>
        /// The language code.
        /// </value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        public Guid LanguageId { get; set; }

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
        /// Gets or sets a value indicating whether page should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if page should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current user has edit role.
        /// </summary>
        /// <value>
        /// <c>true</c> if current user has edit role; otherwise, <c>false</c>.
        /// </value>
        public bool HasEditRole { get; set; }

        /// <summary>
        /// Gets a value indicating whether entity should be saved without checking object security.
        /// </summary>
        /// <value>
        /// <c>true</c> if entity can be saved unsecured; otherwise, <c>false</c>.
        /// </value>
        public bool SaveUnsecured { get; set; }

        /// <summary>
        /// Gets or sets the rendered page contents.
        /// </summary>
        /// <value>
        /// The rendered page contents.
        /// </value>
        internal List<Guid> RenderedPageContents { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Cloned view model</returns>
        public RenderPageViewModel Clone()
        {
            return new RenderPageViewModel
                       {
                           Id = Id,
                           IsDeleted = IsDeleted,
                           Version = Version,
                           HasSEO = HasSEO,
                           Title = Title,
                           MetaTitle = MetaTitle,
                           IsMasterPage = IsMasterPage,
                           MetaDescription = MetaDescription,
                           MetaKeywords = MetaKeywords,
                           PageUrl = PageUrl,
                           Status = Status,
                           CreatedOn = CreatedOn,
                           CreatedByUser = CreatedByUser,
                           ModifiedOn = ModifiedOn,
                           ModifiedByUser = ModifiedByUser,
                           LayoutPath = LayoutPath,
                           RequireJsPath = RequireJsPath,
                           MainJsPath = MainJsPath,
                           Html5ShivJsPath = Html5ShivJsPath,
                           Bag = Bag,
                           CanManageContent = CanManageContent,
                           AreRegionsEditable = AreRegionsEditable,
                           IsReadOnly = IsReadOnly,
                           HasEditRole = HasEditRole,
                           SaveUnsecured = SaveUnsecured,

                           MasterPage = MasterPage != null ? MasterPage.Clone() : null,
                           RenderingPage = RenderingPage != null ? RenderingPage.Clone() : null,

                           Contents = Contents != null ? new List<PageContentProjection>(Contents) : null,
                           Regions = Regions != null ? new List<PageRegionViewModel>(Regions) : null,
                           Options = Options != null ? new List<IOptionValue>(Options) : null,
                           Metadata = Metadata != null ? new List<IPageActionProjection>(Metadata) : null,
                           Stylesheets = Stylesheets != null ? new List<IStylesheetAccessor>(Stylesheets) : null,
                           JavaScripts = JavaScripts != null ? new List<IJavaScriptAccessor>(JavaScripts) : null,
                           AccessRules = AccessRules != null ? new List<IAccessRule>(AccessRules) : null
                       };
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Title: {1}, PageUrl: {2}, LayoutPath: {3}, MasterPage: {4}", Id, Title, PageUrl, LayoutPath, MasterPage);
        }
    }
}