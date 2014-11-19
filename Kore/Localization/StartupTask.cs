//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Transactions;
//using Kore.Data;
//using Kore.Infrastructure;
//using LanguageEntity = Kore.Localization.Domain.Language;
//using LocalizableStringEntity = Kore.Localization.Domain.LocalizableString;

//namespace Kore.Localization
//{
//    public class StartupTask : IStartupTask
//    {
//        private readonly IRepository<LanguageEntity> languageRepository;
//        private readonly IRepository<LocalizableStringEntity> localizableStringRepository;

//        public StartupTask(
//            IRepository<LanguageEntity> languageRepository,
//            IRepository<LocalizableStringEntity> localizableStringRepository)
//        {
//            this.languageRepository = languageRepository;
//            this.localizableStringRepository = localizableStringRepository;
//        }

//        #region IStartupTask Members

//        public void Execute()
//        {
//            var language = languageRepository.Table.FirstOrDefault(x => x.CultureCode == "en-US");
//            if (language == null)
//            {
//                using (var transactionScope = new TransactionScope())
//                {
//                    language = new LanguageEntity
//                    {
//                        Id = Guid.NewGuid(),
//                        CultureCode = "en-US",
//                        IsEnabled = true,
//                        Name = "English (United States)",
//                        SortOrder = 0
//                    };
//                    languageRepository.Insert(language);

//                    //TODO:
//                    //var values = KoreConstants.LocalizedStrings.DefaultValues.Select(x => new LocalizableStringEntity
//                    //{
//                    //    Id = Guid.NewGuid(),
//                    //    TextKey = x.Key,
//                    //    TextValue = x.Value
//                    //});

//                    //localizableStringRepository.Insert(values);

//                    transactionScope.Complete();
//                }
//            }
//        }

//        public int Order
//        {
//            get { return 0; }
//        }

//        #endregion
//    }
//}