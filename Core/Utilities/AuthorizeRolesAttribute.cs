using Microsoft.AspNetCore.Authorization;
using PecqBoxingClubApi.BackEnd.Core.Enums;
using System.Collections.Generic;

namespace PecqBoxingClubApi.BackEnd.Core.Utilities
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
