using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    public interface IOptionTranslation
    {
        string Value { get; set; }

        string LanguageId { get; }
    }

    public interface IOptionTranslationEntity : IOptionTranslation, IEntity
    {
        ILanguage Language { get; set; }

        IOptionEntity Option { get; set; }
    }
}
