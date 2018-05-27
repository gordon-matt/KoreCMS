using Kore.Data;
using KoreCMS.Data.Domain;

namespace KoreCMS.Services
{
    public class MembershipService : IdentityMembershipService
    {
        public MembershipService(IRepository<UserProfileEntry> userProfileRepository)
            : base(userProfileRepository)
        {
        }
    }
}