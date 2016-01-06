using System.Globalization;
using System.Linq;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Language;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Language.SaveLanguage
{
    public class SaveLanguageCommand : CommandBase, ICommand<LanguageViewModel, LanguageViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Updated language view model</returns>
        public LanguageViewModel Execute(LanguageViewModel request)
        {
            var isNew = request.Id.HasDefaultValue();
            Models.Language language;

            // Validate
            ValidateLanguage(request, isNew);

            if (isNew)
            {
                language = new Models.Language();
                language.Code = request.Code;
            }
            else
            {
                language = Repository.AsQueryable<Models.Language>(w => w.Id == request.Id).FirstOne();
            }

            language.Name = request.Name;
            language.Version = request.Version;

            Repository.Save(language);
            UnitOfWork.Commit();

            if (isNew)
            {
                Events.RootEvents.Instance.OnLanguageCreated(language);
            }
            else
            {
                Events.RootEvents.Instance.OnLanguageUpdated(language);
            }

            return new LanguageViewModel
                {
                    Id = language.Id,
                    Version = language.Version,
                    Name = language.Name,
                    Code = CultureInfo.GetCultures(CultureTypes.AllCultures).First(c => c.Name == language.Code).GetFullName(),
                };
        }

        private void ValidateLanguage(LanguageViewModel request, bool isNew)
        {
            var query = Repository.AsQueryable<Models.Language>();
            if (!request.Id.HasDefaultValue())
            {
                query = query.Where(language => language.Id != request.Id);
            }

            if (query.Any(language => language.Name == request.Name))
            {
                var logMessage = string.Format("Language with name {0} already exists. id: {1}", request.Name, request.Id);
                var message = string.Format(RootGlobalization.SaveLanguageCommand_NameAlreadyExists_Message, request.Name);

                throw new ValidationException(() => message, logMessage);
            }

            if (query.Any(language => language.Code == request.Code))
            {
                var logMessage = string.Format("Language with code {0} already exists. id: {1}", request.Code, request.Id);
                var message = string.Format(RootGlobalization.SaveLanguageCommand_CodeAlreadyExists_Message, request.Code);

                throw new ValidationException(() => message, logMessage);
            }

            if (isNew)
            {
                if (!CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => c.Name == request.Code))
                {
                    var logMessage = string.Format("Language with code {0} doesn't exist. Id: {1}, Name: {2}", request.Code, request.Id, request.Name);
                    var message = string.Format(RootGlobalization.SaveLanguageCommand_LanguageNotExists_Message, request.Code);

                    throw new ValidationException(() => message, logMessage);
                }
            }
        }
    }
}