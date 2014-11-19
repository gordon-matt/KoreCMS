using System;
using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Configuration;
using Kore.Web.Http.OData;

namespace Kore.Web.Areas.Admin.Configuration.Controllers.Api
{
    public class SettingsController : GenericODataController<Setting, Guid>
    {
        public SettingsController(IRepository<Setting> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(Setting entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Setting entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<Setting> Get()
        {
            var settings = EngineContext.Current.ResolveAll<ISettings>().Select(x => new
            {
                x.Name,
                Type = x.GetType().FullName
            });

            var existing = Repository.Table.Select(x => x.Type).ToList();
            var newItems = settings.Where(x => !existing.Contains(x.Type));

            if (newItems.Any())
            {
                var newRecords = newItems.Select(x => new Setting
                {
                    Id = Guid.NewGuid(),
                    Name = x.Name,
                    Type = x.Type
                });

                Repository.Insert(newRecords);
            }

            return base.Get();
        }
    }
}