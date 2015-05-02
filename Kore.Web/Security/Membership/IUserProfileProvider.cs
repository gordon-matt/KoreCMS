using System.Collections.Generic;

namespace Kore.Web.Security.Membership
{
    public interface IUserProfileProvider
    {
        string Name { get; }

        string DisplayTemplatePath { get; }

        string EditorTemplatePath { get; }

        IEnumerable<string> GetFieldNames();

        void PopulateFields(string userId);
    }
}