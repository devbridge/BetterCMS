using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Services
{
    internal class DefaultRedirectService : IRedirectService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// The URL service
        /// </summary>
        private IUrlService urlService;
        
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRedirectService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="repository">The repository.</param>
        public DefaultRedirectService(IUnitOfWork unitOfWork, IUrlService urlService, IRepository repository)
        {
            this.unitOfWork = unitOfWork;
            this.urlService = urlService;
            this.repository = repository;
        }

        /// <summary>
        /// Checks if such redirect doesn't exists yet and creates new redirect entity.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <returns>
        /// New created redirect entity or null, if such redirect already exists
        /// </returns>
        public Redirect CreateRedirectEntity(string pageUrl, string redirectUrl)
        {
            ValidateRedirectExists(pageUrl);
            ValidateForCircularLoop(pageUrl, redirectUrl);

            var redirect = new Redirect { PageUrl = pageUrl, RedirectUrl = redirectUrl };


            return redirect;
        }

        /// <summary>
        /// Checks, if such redirect exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="id">The redirect id.</param>
        /// <returns>
        /// true, if redirect exists, false if not exists
        /// </returns>
        public void ValidateRedirectExists(string pageUrl, Guid? id = null)
        {
            var redirect = GetPageRedirect(pageUrl, id);

            if (redirect != null)
            {
                var message = string.Format(PagesGlobalization.SaveRedirect_RedirectExists_Message, pageUrl);
                var logMessage = string.Format("Redirect from URL {0} already exists.", pageUrl);
                throw new ValidationException(() => message, logMessage);
            }
        }

        /// <summary>
        /// Gets redirect, if such redirect exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="id">The redirect id.</param>
        /// <returns>
        /// redirect, if redirect exists, null if not exists
        /// </returns>
        public Redirect GetPageRedirect(string pageUrl, Guid? id = null)
        {
            var query = unitOfWork
                .Session.Query<Redirect>()
                .Where(r => r.PageUrl == pageUrl 
                    && !r.IsDeleted);

            if (id.HasValue && !id.Value.HasDefaultValue())
            {
                query = query.Where(r => r.Id != id.Value);
            }
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Validates urls: checks if the the circular loop exists.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="id">The id.</param>
        public void ValidateForCircularLoop(string pageUrl, string redirectUrl, Guid? id = null)
        {
            var redirects = GetAllRedirects();
            if (id.HasValue && !id.Value.HasDefaultValue())
            {
                redirects.Where(r => r.Id == id.Value).ForEach(r =>
                    {
                        r.PageUrl = pageUrl;
                        r.RedirectUrl = redirectUrl;
                    });
            }
            else
            {
                id = Guid.NewGuid();
                redirects.Add(new Redirect
                    {
                        Id = Guid.NewGuid(),
                        PageUrl = pageUrl,
                        RedirectUrl = redirectUrl
                    });
            }
            var checkedIds = new List<Guid> { id.Value };
            RecursiveCircularLoop(redirects, redirectUrl, checkedIds, pageUrl, redirectUrl);
        }

        /// <summary>
        /// Recursive method checks for circular loops in redirects list.
        /// </summary>
        /// <param name="redirects">The redirects.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="checkedIds">The checked ids.</param>
        /// <param name="startPageUrl">The start page URL.</param>
        /// <param name="startRedirectUrl">The start redirect URL.</param>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        private void RecursiveCircularLoop(IList<Redirect> redirects, string redirectUrl, List<Guid> checkedIds, string startPageUrl, string startRedirectUrl)
        {
            var redirectTo = redirects.FirstOrDefault(r => r.PageUrl == redirectUrl);
            if (redirectTo != null)
            {
                if (checkedIds.Contains(redirectTo.Id))
                {
                    var message = PagesGlobalization.SaveRedirect_CircularLoopDetected_Message;
                    var logMessage = string.Format("Cannot save redirect. Circular redirect loop from url {0} to url {1} detected.", startPageUrl, startRedirectUrl);
                    throw new ValidationException(() => message, logMessage);
                }

                checkedIds.Add(redirectTo.Id);
                RecursiveCircularLoop(redirects, redirectTo.RedirectUrl, checkedIds, startPageUrl, startRedirectUrl);
            }
        }

        /// <summary>
        /// Gets the list with all redirects.
        /// </summary>
        /// <returns>The list with all redirectss</returns>
        private IList<Redirect> GetAllRedirects()
        {
            var query = unitOfWork
                .Session
                .Query<Redirect>()
                .Where(r => !r.IsDeleted);
            var redirects = query.ToList();

            return redirects;
        }

        /// <summary>
        /// Gets the redirect for given url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// Redirect url
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetRedirect(string url)
        {
            var redirect = unitOfWork
                .Session
                .Query<Redirect>()
                .Where(r => r.PageUrl == url
                    && !r.IsDeleted)
                .Select(r => r.RedirectUrl)
                .SingleOrDefault();
            return redirect;
        }

        public Redirect SaveRedirect(ViewModels.SiteSettings.SiteSettingRedirectViewModel model, bool createIfNotExists = false)
        {
            var isRedirectInternal = urlService.ValidateInternalUrl(model.RedirectUrl);
            if (!isRedirectInternal && urlService.ValidateInternalUrl(urlService.FixUrl(model.RedirectUrl)))
            {
                isRedirectInternal = true;
            }

            model.PageUrl = urlService.FixUrl(model.PageUrl);
            if (isRedirectInternal)
            {
                model.RedirectUrl = urlService.FixUrl(model.RedirectUrl);
            }

            // Validate request
            if (!urlService.ValidateInternalUrl(model.PageUrl))
            {
                var message = PagesGlobalization.SaveRedirect_InvalidPageUrl_Message;
                var logMessage = string.Format("Invalid page url {0}.", model.PageUrl);
                throw new ValidationException(() => message, logMessage);
            }
            if (!urlService.ValidateExternalUrl(model.RedirectUrl))
            {
                var message = PagesGlobalization.SaveRedirect_InvalidRedirectUrl_Message;
                var logMessage = string.Format("Invalid redirect url {0}.", model.RedirectUrl);
                throw new ValidationException(() => message, logMessage);
            }

            // Validate for url patterns
            string patternsValidationMessage;
            if (!urlService.ValidateUrlPatterns(model.PageUrl, out patternsValidationMessage))
            {
                var logMessage = string.Format("{0}. URL: {1}.", patternsValidationMessage, model.PageUrl);
                throw new ValidationException(() => patternsValidationMessage, logMessage);
            }
            if (isRedirectInternal && !urlService.ValidateUrlPatterns(model.RedirectUrl, out patternsValidationMessage, PagesGlobalization.SaveRedirect_RedirectUrl_Name))
            {
                var logMessage = string.Format("{0}. URL: {1}.", patternsValidationMessage, model.PageUrl);
                throw new ValidationException(() => patternsValidationMessage, logMessage);
            }

            ValidateRedirectExists(model.PageUrl, model.Id);
            ValidateForCircularLoop(model.PageUrl, model.RedirectUrl, model.Id);

            Redirect redirect = null;
            var isNew = model.Id.HasDefaultValue();
            
            if (!isNew)
            {
                redirect = repository.FirstOrDefault<Redirect>(model.Id);
                isNew = redirect == null;

                if (isNew && !createIfNotExists)
                {
                    throw new EntityNotFoundException(typeof(Redirect), model.Id);
                }
            }

            if (isNew)
            {
                redirect = new Redirect { Id = model.Id };
            }

            if (model.Version > 0)
            {
                redirect.Version = model.Version;
            }
            redirect.PageUrl = model.PageUrl;
            redirect.RedirectUrl = model.RedirectUrl;

            repository.Save(redirect);
            unitOfWork.Commit();

            // Notify.
            if (isNew)
            {
                Events.PageEvents.Instance.OnRedirectCreated(redirect);
            }
            else
            {
                Events.PageEvents.Instance.OnRedirectUpdated(redirect);
            }

            return redirect;
        }

        public bool DeleteRedirect(Guid id, int version)
        {
            Redirect redirect;
            if (version > 0)
            {
                redirect = repository.Delete<Redirect>(id, version);
            }
            else
            {
                redirect = repository.First<Redirect>(id);
                repository.Delete(redirect);
            }

            unitOfWork.Commit();
            Events.PageEvents.Instance.OnRedirectDeleted(redirect);

            return true;
        }
    }
}