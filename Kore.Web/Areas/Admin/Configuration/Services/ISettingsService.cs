//using System.Linq;
//using Kore.Configuration.Domain;
//using Kore.Data.Services;
//using Kore.Web.Areas.Admin.Configuration.Models;

//namespace Kore.Web.Areas.Admin.Configuration.Services
//{
//    public interface ISettingsService : IGenericDataService<Setting>
//    {
//        SettingModel GetModel(string name);

//        void UpdateFromModel(SettingModel model);
//    }

//    public class SettingsService : GenericDataService<Setting>, ISettingsService
//    {
//        #region ISettingsService Members

//        public SettingModel GetModel(string name)
//        {
//            var setting = Repository.Table.FirstOrDefault(x => x.Name == name);
//            return setting ?? null;
//        }

//        public void UpdateFromModel(SettingModel model)
//        {
//            var record = Repository.Table.FirstOrDefault(x => x.Name == model.Name);
//            record.Value = ((object)model.Value).ToJson();
//            Repository.Update(record);
//        }

//        #endregion ISettingsService Members
//    }
//}