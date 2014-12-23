using System;
using Kore.Web.Configuration;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.Indexing.Configuration
{
    public class SearchSettings : ISettings
    {
        public SearchSettings()
        {
            SearchedFields = new[] { "title", "body" };
        }

        //string ISettings.Name { get { return "Search Settings"; } }

        //bool ISettings.Hidden { get { return false; } }

        //void ISettings.OnEditing(RoboUIFormResult<ISettings> roboForm, WorkContext workContext)
        //{
        //    var indexingService = workContext.Resolve<IIndexingService>();
        //    IndexEntry indexEntry;

        //    try
        //    {
        //        indexEntry = indexingService.GetIndexEntry("Search");
        //    }
        //    catch (Exception)
        //    {
        //        indexEntry = null;
        //    }

        //    if (indexEntry == null)
        //    {
        //        roboForm.RegisterExternalDataSource("SearchedFields", "title", "body");
        //    }
        //    else
        //    {
        //        roboForm.RegisterExternalDataSource("SearchedFields", indexEntry.Fields);
        //    }
        //}

        //[RoboChoice(RoboChoiceType.CheckBoxList, Columns = 2, LabelText = "Searched Fields")]
        public string[] SearchedFields { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "Search Settings"; }
        }

        public string EditorTemplatePath
        {
            get { throw new NotImplementedException(); }
        }

        #endregion ISettings Members
    }
}