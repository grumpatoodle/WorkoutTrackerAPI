using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Common
{
    public static class Constants
    {
        public static class Policies
        {
            public const string CanChangeUserRole = "CanChangeUserRole";
        }

        public static class Users
        {
            public static class Roles
            {
                public const string Admin = "Admin";
                public const string User = "User";
            }
        }
    }
}
