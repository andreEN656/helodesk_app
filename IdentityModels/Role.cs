using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModels
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }

        public Role()
        {
        }
        public Role(string roleName) : base(roleName)
        {
        }
    }
}
