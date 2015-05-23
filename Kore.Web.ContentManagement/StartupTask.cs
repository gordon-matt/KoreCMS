using System;
using System.Linq;
using Kore.Collections;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Configuration;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;

namespace Kore.Web.ContentManagement
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            EnsurePageTypes();
        }

        private static void EnsurePageTypes()
        {
            var pageTypeService = EngineContext.Current.Resolve<IPageTypeService>();
            var pageTypeRepository = EngineContext.Current.Resolve<IRepository<PageType>>();

            var allPageTypes = pageTypeService.GetKorePageTypes();

            var allPageTypeNames = allPageTypes.Select(x => x.Name).ToList();
            var installedPageTypes = pageTypeRepository.Table.ToList();
            var installedPageTypeNames = installedPageTypes.Select(x => x.Name).ToList();

            var pageTypesToAdd = allPageTypes.Where(x => !installedPageTypeNames.Contains(x.Name)).Select(x => new PageType
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                LayoutPath = KoreWebConstants.DefaultFrontendLayoutPath,
                DisplayTemplatePath = x.DisplayTemplatePath,
                EditorTemplatePath = x.EditorTemplatePath
            }).ToList();

            if (!pageTypesToAdd.IsNullOrEmpty())
            {
                pageTypeRepository.Insert(pageTypesToAdd);
            }

            var pageTypesToDelete = installedPageTypes.Where(x => !allPageTypeNames.Contains(x.Name)).ToList();
            var pageTypesToDeleteIds = pageTypesToDelete.Select(y => y.Id).ToList();

            if (!pageTypesToDelete.IsNullOrEmpty())
            {
                pageTypeRepository.Delete(x => pageTypesToDeleteIds.Contains(x.Id));
            }
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IStartupTask Members
    }
}