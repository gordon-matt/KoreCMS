using System.Collections.Generic;
using System.Linq;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.Security.Membership
{
    public class KoreWebUserProfileProvider : IUserProfileProvider
    {
        public const string DontUseMobileVersion = "DontUseMobileVersion";

        #region IUserProfileProvider Members

        public string Category
        {
            get { return "General"; }
        }

        public IEnumerable<string> GetFieldNames()
        {
            return new[]
            {
                DontUseMobileVersion
            };
        }

        public IEnumerable<RoboControlAttribute> GetFields(string userId, bool onlyPublicProperties)
        {
            if (onlyPublicProperties)
            {
                // if there are many properties and some of them are public, then we can return only the public properties,
                //  but for this provider there is ONLY 1 property and it is not public, so we just return empty an result
                return Enumerable.Empty<RoboControlAttribute>();
            }

            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            string dontUseMobileVersion = membershipService.GetProfileEntry(userId, DontUseMobileVersion);

            return new[]
            {
                new RoboChoiceAttribute(RoboChoiceType.CheckBox)
                {
                    Name = DontUseMobileVersion,
                    LabelText = "Disable Mobile Version",
                    Value = !string.IsNullOrEmpty(dontUseMobileVersion) && bool.Parse(dontUseMobileVersion),
                    PropertyType = typeof(bool),
                }
            };
        }

        #endregion IUserProfileProvider Members
    }
}