using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Results;
using Kore.Caching;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Localization.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using LanguageEntity = Kore.Localization.Domain.Language;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class LanguageApiController : GenericODataController<LanguageEntity, Guid>
    {
        private readonly Lazy<ICacheManager> cacheManager;
        private readonly Lazy<IRepository<LocalizableString>> localizableStringRepository;

        public LanguageApiController(
            IRepository<LanguageEntity> repository,
            Lazy<IRepository<LocalizableString>> localizableStringRepository,
            Lazy<ICacheManager> cacheManager)
            : base(repository)
        {
            this.localizableStringRepository = localizableStringRepository;
            this.cacheManager = cacheManager;
        }

        protected override Guid GetId(LanguageEntity entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(LanguageEntity entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [HttpPost]
        public virtual IHttpActionResult ResetLocalizableStrings(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            localizableStringRepository.Value.DeleteAll();

            var localizedStringsProviders = EngineContext.Current.ResolveAll<IDefaultLocalizableStringsProvider>();

            var toInsert = new HashSet<LocalizableString>();
            foreach (var provider in localizedStringsProviders)
            {
                foreach (var translation in provider.GetTranslations())
                {
                    foreach (var localizedString in translation.LocalizedStrings)
                    {
                        toInsert.Add(new LocalizableString
                        {
                            Id = Guid.NewGuid(),
                            CultureCode = translation.CultureCode,
                            TextKey = localizedString.Key,
                            TextValue = localizedString.Value
                        });
                    }
                }
            }
            localizableStringRepository.Value.Insert(toInsert);

            cacheManager.Value.Remove("LocalizableStrings_");

            return Ok();
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.LanguagesRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.LanguagesWrite; }
        }
    }
}