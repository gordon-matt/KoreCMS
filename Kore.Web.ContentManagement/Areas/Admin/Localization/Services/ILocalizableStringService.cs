using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Collections;
using Kore.Data;
using Kore.Data.Services;
using Kore.Localization.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Localization.Models;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Services
{
    public interface ILocalizableStringService : IGenericDataService<LocalizableString>
    {
        IEnumerable<ComparitiveLocalizableString> GetComparitiveTable(string cultureCode, int pageIndex, int pageSize, out int totalRecords);

        void Update(string cultureCode, IEnumerable<ComparitiveLocalizableString> table);
    }

    public class LocalizableStringService : GenericDataService<LocalizableString>, ILocalizableStringService
    {
        private readonly IRepository<LocalizableString> repository;

        public LocalizableStringService(IRepository<LocalizableString> repository)
            : base(repository)
        {
            this.repository = repository;
        }

        public virtual IEnumerable<ComparitiveLocalizableString> GetComparitiveTable(string cultureCode, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = repository.Table.Count(x => x.CultureCode == null);

            var table = new List<ComparitiveLocalizableString>();

            try
            {
                var query = repository.Table
                    .Where(x => x.CultureCode == null || x.CultureCode == cultureCode)
                    .GroupBy(x => x.TextKey)
                    .Select(grp => new ComparitiveLocalizableString
                    {
                        Key = grp.Key,
                        InvariantValue = grp.FirstOrDefault(x => x.CultureCode == null).TextValue,
                        LocalizedValue = grp.FirstOrDefault(x => x.CultureCode == cultureCode) == null ? "" : grp.FirstOrDefault(x => x.CultureCode == cultureCode).TextValue
                    })
                    .OrderBy(x => x.Key)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);

                table.AddRange(query.ToList());
            }
            catch (Exception)
            {
                //SQLite throws error: "APPLY joins are not supported"
                // So do it in memory instead

                var query = repository.Table
                    .Where(x => x.CultureCode == null || x.CultureCode == cultureCode)
                    .ToHashSet()
                    .GroupBy(x => x.TextKey)
                    .Select(grp => new ComparitiveLocalizableString
                    {
                        Key = grp.Key,
                        InvariantValue = grp.FirstOrDefault(x => x.CultureCode == null).TextValue,
                        LocalizedValue = grp.FirstOrDefault(x => x.CultureCode == cultureCode) == null ? "" : grp.FirstOrDefault(x => x.CultureCode == cultureCode).TextValue
                    })
                    .OrderBy(x => x.Key)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);

                table.AddRange(query.ToList());
            }

            return table;
        }

        public virtual void Update(string cultureCode, IEnumerable<ComparitiveLocalizableString> table)
        {
            var localizedStrings = repository.Table.Where(x => x.CultureCode == cultureCode).ToList().ToDictionary(k => k.TextKey, v => v.TextValue);
            var newItems = table.Where(x => !localizedStrings.Keys.Contains(x.Key));

            var batchInserts = newItems.Select(item => new LocalizableString
            {
                Id = Guid.NewGuid(),
                CultureCode = cultureCode,
                TextKey = item.Key,
                TextValue = item.LocalizedValue
            }).ToList();

            if (batchInserts.Any())
            {
                repository.Insert(batchInserts);
            }

            var changedItems = table.Where(x => localizedStrings.Keys.Contains(x.Key) && localizedStrings[x.Key] != x.LocalizedValue).ToList();
            var changedItemKeys = changedItems.Select(x => x.Key);

            var toUpdate = repository.Table
                .Where(x =>
                    x.CultureCode == cultureCode &&
                    changedItemKeys.Contains(x.TextKey))
                .ToList();

            var batchUpdates = new List<LocalizableString>();

            foreach (var item in toUpdate)
            {
                item.TextValue = changedItems.First(x => x.Key == item.TextKey).LocalizedValue;
                batchUpdates.Add(item);
            }

            if (batchUpdates.Any())
            {
                repository.Update(batchUpdates);
            }
        }
    }
}