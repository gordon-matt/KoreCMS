﻿using System;
using Kore.Security.Membership;
using Kore.Web.Environment;

namespace Kore.Web.Security.Membership
{
    public class CurrentUserStateProvider : IWorkContextStateProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMembershipService membershipService;

        public CurrentUserStateProvider(IMembershipService membershipService, IHttpContextAccessor httpContextAccessor)
        {
            this.membershipService = membershipService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Func<WorkContext, T> Get<T>(string name)
        {
            if (name == KoreWebConstants.StateProviders.CurrentUser)
            {
                var httpContext = httpContextAccessor.Current();
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    var user = membershipService.GetUserByName(httpContext.User.Identity.Name);
                    if (user == null)
                    {
                        return null;
                    }
                    return ctx => (T)(object)user;
                }
            }
            return null;
        }
    }
}