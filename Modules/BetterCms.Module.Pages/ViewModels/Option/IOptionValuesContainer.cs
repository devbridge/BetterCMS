using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.ViewModels.Option
{
    public interface IOptionValuesContainer
    {
        System.Collections.Generic.IList<OptionValueViewModel> OptionValues { get; set; }
    }
}