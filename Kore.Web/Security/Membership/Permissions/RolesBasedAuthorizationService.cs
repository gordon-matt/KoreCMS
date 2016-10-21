using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Kore.Collections;
using Kore.Localization;
using Kore.Security.Membership;
using Kore.Threading;

namespace Kore.Web.Security.Membership.Permissions
{
    public class RolesBasedAuthorizationService : IAuthorizationService
    {
        private readonly IMembershipService membershipService;
        private readonly IWorkContext workContext;

        private static readonly string[] anonymousRole = new[] { "Anonymous" };
        private static readonly string[] authenticatedRole = new[] { "Authenticated" };

        public RolesBasedAuthorizationService(IMembershipService membershipService, IWorkContext workContext)
        {
            this.membershipService = membershipService;
            this.workContext = workContext;
        }

        public Localizer T { get; set; }

        public void CheckAccess(Permission permission, KoreUser user)
        {
            if (!TryCheckAccess(permission, user))
            {
                throw new SecurityException();
            }
        }

        public bool TryCheckAccess(Permission permission, KoreUser user)
        {
            var context = new CheckAccessContext { Permission = permission, User = user };

            for (var adjustmentLimiter = 0; adjustmentLimiter != 3; ++adjustmentLimiter)
            {
                //if (!context.Granted && context.User != null && context.User.IsSuperUser)
                //{
                //    context.Granted = true;
                //}

                if (!context.Granted)
                {
                    // determine which set of permissions would satisfy the access check
                    var grantingNames = PermissionNames(context.Permission, Enumerable.Empty<string>()).Distinct().ToArray();

                    // determine what set of roles should be examined by the access check
                    IEnumerable<string> rolesToExamine;
                    if (context.User == null)
                    {
                        rolesToExamine = anonymousRole;
                    }
                    else
                    {
                        rolesToExamine = (AsyncHelper.RunSync(() => membershipService.GetRolesForUser(context.User.Id))).Select(x => x.Name).ToList();
                        if (!rolesToExamine.Contains(anonymousRole[0]))
                        {
                            rolesToExamine = rolesToExamine.Concat(authenticatedRole);
                        }
                    }

                    foreach (var role in rolesToExamine)
                    {
                        var rolePermissions = AsyncHelper.RunSync(() => membershipService.GetPermissionsForRole(workContext.CurrentTenant.Id, role));
                        foreach (var rolePermission in rolePermissions)
                        {
                            string possessedName = rolePermission.Name;
                            if (grantingNames.Any(grantingName => String.Equals(possessedName, grantingName, StringComparison.OrdinalIgnoreCase)))
                            {
                                context.Granted = true;
                            }

                            if (context.Granted)
                                break;
                        }

                        if (context.Granted)
                            break;
                    }
                }

                context.Adjusted = false;
                if (!context.Adjusted)
                    break;
            }

            return context.Granted;
        }

        private static IEnumerable<string> PermissionNames(Permission permission, IEnumerable<string> stack)
        {
            // the given name is tested
            yield return permission.Name;

            // iterate implied permissions to grant, it present
            if (!permission.ImpliedBy.IsNullOrEmpty())
            {
                foreach (var impliedBy in permission.ImpliedBy)
                {
                    // avoid potential recursion
                    if (stack.Contains(impliedBy.Name))
                        continue;

                    // otherwise accumulate the implied permission names recursively
                    foreach (var impliedName in PermissionNames(impliedBy, stack.Concat(new[] { permission.Name })))
                    {
                        yield return impliedName;
                    }
                }
            }

            yield return StandardPermissions.FullAccess.Name;
        }
    }
}