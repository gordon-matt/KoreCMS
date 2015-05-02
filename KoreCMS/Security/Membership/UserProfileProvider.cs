using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Security.Membership;

namespace KoreCMS.Security.Membership
{
    public class UserProfileProvider : IUserProfileProvider
    {
        public class Fields
        {
            public const string FamilyName = "FamilyName";
            public const string GivenNames = "GivenNames";
            public const string ShowFamilyNameFirst = "ShowFamilyNameFirst";
        }

        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }

        [Display(Name = "Given Names")]
        public string GivenNames { get; set; }

        [Display(Name = "Show Family Name First")]
        public bool ShowFamilyNameFirst { get; set; }

        #region IUserProfileProvider Members

        public string Name
        {
            get { return "General"; }
        }

        public string DisplayTemplatePath
        {
            get { return "~/Views/Shared/DisplayTemplates/UserProfileProvider.cshtml"; }
        }

        public string EditorTemplatePath
        {
            get { return "~/Views/Shared/EditorTemplates/UserProfileProvider.cshtml"; }
        }

        public IEnumerable<string> GetFieldNames()
        {
            return new[]
            {
                Fields.FamilyName,
                Fields.GivenNames,
                Fields.ShowFamilyNameFirst
            };
        }

        public void PopulateFields(string userId)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            var profile = membershipService.GetProfile(userId);

            if (profile.ContainsKey(Fields.FamilyName))
            {
                FamilyName = profile[Fields.FamilyName];
            }
            if (profile.ContainsKey(Fields.GivenNames))
            {
                GivenNames = profile[Fields.GivenNames];
            }
            if (profile.ContainsKey(Fields.ShowFamilyNameFirst))
            {
                ShowFamilyNameFirst = bool.Parse(profile[Fields.ShowFamilyNameFirst]);
            }
        }

        #endregion IUserProfileProvider Members
    }
}