using System.Globalization;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cultures;

namespace BetterCms.Module.Root.Commands.Culture.SaveCulture
{
    public class SaveCultureCommand : CommandBase, ICommand<CultureViewModel, CultureViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Updated culture view model</returns>
        public CultureViewModel Execute(CultureViewModel request)
        {
            var isNew = request.Id.HasDefaultValue();
            Models.Culture culture;

            // Validate
            ValidateCulture(request, isNew);

            if (isNew)
            {
                culture = new Models.Culture();
                culture.Code = request.Code;
            }
            else
            {
                culture = Repository.AsQueryable<Models.Culture>(w => w.Id == request.Id).FirstOne();
            }

            culture.Name = request.Name;
            culture.Version = request.Version;

            Repository.Save(culture);
            UnitOfWork.Commit();

            if (isNew)
            {
                Events.RootEvents.Instance.OnCultureCreated(culture);
            }
            else
            {
                Events.RootEvents.Instance.OnCultureUpdated(culture);
            }

            return new CultureViewModel
                {
                    Id = culture.Id,
                    Version = culture.Version,
                    Name = culture.Name,
                    Code = culture.Code,
                };
        }

        private void ValidateCulture(CultureViewModel request, bool isNew)
        {
            var query = Repository.AsQueryable<Models.Culture>();
            if (!request.Id.HasDefaultValue())
            {
                query = query.Where(culture => culture.Id != request.Id);
            }

            if (query.Any(culture => culture.Name == request.Name))
            {
                var logMessage = string.Format("Culture with name {0} already exists. id: {1}", request.Name, request.Id);
                var message = string.Format(RootGlobalization.SaveCultureCommand_NameAlreadyExists_Message, request.Name);

                throw new ValidationException(() => message, logMessage);
            }

            if (query.Any(culture => culture.Code == request.Code))
            {
                var logMessage = string.Format("Culture with code {0} already exists. id: {1}", request.Code, request.Id);
                var message = string.Format(RootGlobalization.SaveCultureCommand_CodeAlreadyExists_Message, request.Code);

                throw new ValidationException(() => message, logMessage);
            }

            if (isNew)
            {
                if (!CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => c.Name == request.Code))
                {
                    var logMessage = string.Format("Culture with code {0} doesn't exist. Id: {1}, Name: {2}", request.Code, request.Id, request.Name);
                    var message = string.Format(RootGlobalization.SaveCultureCommand_CultureNotExists_Message, request.Name);

                    throw new ValidationException(() => message, logMessage);
                }
            }
        }
    }
}