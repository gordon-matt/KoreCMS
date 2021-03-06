﻿using System.Collections.Generic;
using System.Linq;
using Kore.Data;
using Kore.Infrastructure;

namespace Kore.Localization
{
    public class LanguageManager : ILanguageManager
    {
        public IEnumerable<Language> GetActiveLanguages(int tenantId)
        {
            var repository = EngineContext.Current.Resolve<IRepository<Kore.Localization.Domain.Language>>();

            return repository.Find(x => x.TenantId == tenantId && x.IsEnabled)
                .Select(x => new Language
                {
                    CultureCode = x.CultureCode,
                    Name = x.Name
                });
        }
    }
}