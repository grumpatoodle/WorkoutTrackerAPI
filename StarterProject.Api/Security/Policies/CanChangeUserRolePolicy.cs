using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using StarterProject.Api.Common;

namespace StarterProject.Api.Security.Policies
{
    public class CanChangeUserRoleRequirement : IAuthorizationRequirement
    {
        public CanChangeUserRoleRequirement()
        {
        }
    }

    public class CanChangeUserRoleHandler : AuthorizationHandler<CanChangeUserRoleRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext authContext,   
            CanChangeUserRoleRequirement requirement)
        {
            if (!authContext.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;
            }
            
            var userRole = authContext.User.FindFirst(x => x.Type == ClaimTypes.Role).Value;
            
            if (userRole == Constants.Users.Roles.Admin)
            {
                authContext.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
