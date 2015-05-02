using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Security.Membership;

namespace Kore.Web.Mobile
{
    public class MobileUserProfileProvider : IUserProfileProvider
    {
        public class Fields
        {
            public const string DontUseMobileVersion = "DontUseMobileVersion";
        }

        [Display(Name = "Don't Use Mobile Version")]
        public bool DontUseMobileVersion { get; set; }

        #region IUserProfileProvider Members

        public string Name
        {
            get { return "Mobile"; }
        }

        public string DisplayTemplatePath
        {
            get { return "Kore.Web.Views.Shared.DisplayTemplates.MobileUserProfileProvider"; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.Views.Shared.EditorTemplates.MobileUserProfileProvider"; }
        }

        public int Order
        {
            get { return 9999; }
        }

        public IEnumerable<string> GetFieldNames()
        {
            return new[]
            {
                Fields.DontUseMobileVersion
            };
        }

        public void PopulateFields(string userId)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            string dontUseMobileVersion = membershipService.GetProfileEntry(userId, Fields.DontUseMobileVersion);
            DontUseMobileVersion = !string.IsNullOrEmpty(dontUseMobileVersion) && bool.Parse(dontUseMobileVersion);
        }

        #endregion IUserProfileProvider Members
    }
}