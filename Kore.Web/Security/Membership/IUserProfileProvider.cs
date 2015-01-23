using System.Collections.Generic;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.Security.Membership
{
    public interface IUserProfileProvider
    {
        string Category { get; }

        IEnumerable<string> GetFieldNames();

        IEnumerable<RoboControlAttribute> GetFields(string userId, bool onlyPublicProperties);
    }
}