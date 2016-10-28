using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Kore.Caching;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Localization.Domain;
using Kore.Localization.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using LanguageEntity = Kore.Localization.Domain.Language;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class LanguageApiController : GenericTenantODataController<LanguageEntity, Guid>
    {
        private readonly Lazy<ICacheManager> cacheManager;
        private readonly Lazy<ILocalizableStringService> localizableStringService;

        public LanguageApiController(
            ILanguageService service,
            Lazy<ILocalizableStringService> localizableStringService,
            Lazy<ICacheManager> cacheManager)
            : base(service)
        {
            this.localizableStringService = localizableStringService;
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
        public virtual async Task<IHttpActionResult> ResetLocalizableStrings(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            int? tenantId = GetTenantId();
            await localizableStringService.Value.DeleteAsync(x => x.TenantId == tenantId);

            var languagePacks = EngineContext.Current.ResolveAll<ILanguagePack>();

            var toInsert = new HashSet<LocalizableString>();
            foreach (var languagePack in languagePacks)
            {
                foreach (var localizedString in languagePack.LocalizedStrings)
                {
                    toInsert.Add(new LocalizableString
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId,
                        CultureCode = languagePack.CultureCode,
                        TextKey = localizedString.Key,
                        TextValue = localizedString.Value
                    });
                }
            }
            await localizableStringService.Value.InsertAsync(toInsert);

            cacheManager.Value.RemoveByPattern("Kore_LocalizableStrings_.*");

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