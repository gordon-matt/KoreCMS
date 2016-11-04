using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Kore.Web.Identity.Domain;
using Microsoft.AspNet.Identity;

namespace Kore.Web.Identity
{
    public abstract class KoreRoleValidator<TRole> : RoleValidator<TRole>
        where TRole : KoreIdentityRole
    {
        protected RoleManager<TRole, string> Manager { get; set; }

        public KoreRoleValidator(RoleManager<TRole, string> manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        public override async Task<IdentityResult> ValidateAsync(TRole item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var errors = new List<string>();
            await this.ValidateRoleName(item, errors);
            return errors.Count <= 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }

        protected virtual async Task ValidateRoleName(TRole role, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                errors.Add("Name cannot be null or empty.");
            }
            else
            {
                var existingRole = await this.Manager.Roles.FirstOrDefaultAsync(x => x.TenantId == role.TenantId && x.Name == role.Name);
                if (existingRole == null)
                {
                    return;
                }

                errors.Add(string.Format("{0} is already taken.", role.Name));
            }
        }
    }
}