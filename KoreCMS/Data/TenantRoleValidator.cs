using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;

namespace KoreCMS.Data
{
    public class TenantRoleValidator : RoleValidator<ApplicationRole>
    {
        private RoleManager<ApplicationRole, string> Manager { get; set; }

        public TenantRoleValidator(RoleManager<ApplicationRole, string> manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationRole item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var errors = new List<string>();
            await this.ValidateRoleName(item, errors);
            return errors.Count <= 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }

        private async Task ValidateRoleName(ApplicationRole role, List<string> errors)
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