using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Context
{
    public class HelpdeskAppContext : EntityContextBase<HelpdeskAppContext>
    {
        public HelpdeskAppContext(DbContextOptions<HelpdeskAppContext> options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.LowerCaseTablesAndFields();
        }
    }
}
