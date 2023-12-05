using Microsoft.AspNetCore.Authorization;
using RamDam.BackEnd.Core.Enums;
using System.Collections.Generic;

namespace RamDam.BackEnd.Core.Utilities
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
