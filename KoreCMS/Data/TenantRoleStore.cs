//using System;
//using System.Data.Entity;
//using System.Threading.Tasks;
//using KoreCMS.Data.Domain;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;

//namespace KoreCMS.Data
//{
//    public class TenantRoleStore<TRole> : RoleStore<TRole>
//        where TRole : ApplicationRole, new()
//    {
//        public TenantRoleStore(int tenantId)
//            : base(new ApplicationDbContext())
//        {
//            this.DisposeContext = true;
//        }

//        /// <summary>Constructor</summary>
//        /// <param name="context"></param>
//        public TenantRoleStore(int tenantId, DbContext context)
//            : base(context)
//        {
//        }

//        private int TenantId { get; set; }

//        public override Task CreateAsync(TRole role)
//        {
//            if (role == null)
//            {
//                throw new ArgumentNullException("role");
//            }
//            role.TenantId = TenantId;
//            return base.CreateAsync(role);
//        }
//    }
//}