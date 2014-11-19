using System.Data.Entity;
using Kore.Localization.Domain;
using LanguageEntity = Kore.Localization.Domain.Language;

namespace Kore.Data
{
    public interface IKoreDbContext
    {
        DbSet<LanguageEntity> Languages { get; set; }

        DbSet<LocalizableString> LocalizableStrings { get; set; }
    }
}