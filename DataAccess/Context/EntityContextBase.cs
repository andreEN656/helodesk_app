using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Context
{
    public class EntityContextBase<TContext> : IdentityDbContext<IdentityModels.User, IdentityModels.Role, string>, IEntityContext where TContext : IdentityDbContext<IdentityModels.User, IdentityModels.Role, string>
    {
        public EntityContextBase(DbContextOptions<TContext> options) : base(options)
        {
        }
    }
}
