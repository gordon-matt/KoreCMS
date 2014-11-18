using Kore.Data;
using KoreCMS.Data;
using KoreCMS.Data.Domain;

namespace KoreCMS.Services
{
    public class MembershipService : IdentityMembershipService<ApplicationDbContext>
    {
        public MembershipService(IRepository<Permission> permissionRepository)
            : base(permissionRepository)
        {
        }
    }
}