using BetterCms.Core.DataContracts;
using BetterCms.Module.Pages.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public interface IOptionService
    {
        void SetOptionValues(IOptionValuesContainer viewModel, IOptions optionsContainer, IOptionValues optionValuesContainer);
    }
}