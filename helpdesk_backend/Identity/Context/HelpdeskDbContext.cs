using IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helpdesk_backend.Identity.Context
{
    public class HelpdeskDbContext : IdentityDbContext<User, Role, string>
    {
        public new virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<LoginInfo> LoginIngos { get; set; }

        public HelpdeskDbContext(DbContextOptions<HelpdeskDbContext> options)
            : base(options)
        {

        }
        public HelpdeskDbContext()
        {
        }

        public static HelpdeskDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<HelpdeskDbContext>();
            optionsBuilder.UseMySQL("Server=157.230.125.134;Database=helpdesk;Uid=output;Pwd=a19731973;Charset=utf8");
            return new HelpdeskDbContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().Property(c => c.LoginProvider).HasMaxLength(255);
            modelBuilder.Entity<IdentityUserLogin<string>>().Property(c => c.ProviderKey).HasMaxLength(255);

            modelBuilder.Entity<IdentityUserRole<string>>().Property(c => c.RoleId).HasMaxLength(255);
            modelBuilder.Entity<IdentityUserRole<string>>().Property(c => c.UserId).HasMaxLength(255);

            modelBuilder.Entity<IdentityUserToken<string>>().Property(c => c.LoginProvider).HasMaxLength(255);
            modelBuilder.Entity<IdentityUserToken<string>>().Property(c => c.Name).HasMaxLength(255);
            modelBuilder.Entity<IdentityUserToken<string>>().Property(c => c.UserId).HasMaxLength(255);

            //modelBuilder.Entity<User>().HasMany(p => p.Roles).WithOne().HasForeignKey(p => p.UserId).IsRequired();
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().ToTable("AspNetRoles");
            //modelBuilder.Entity<Role>().Property(c => c.Id).HasMaxLength(255);
            modelBuilder.Entity<Role>().HasKey(c => c.Id);

            modelBuilder.Entity<LoginInfo>().ToTable("AspNetUserLoginInfos");
            //modelBuilder.Entity<LoginInfo>().Property(c => c.Id).HasMaxLength(255);
            //modelBuilder.Entity<LoginInfo>().Property(c => c.Id).HasMaxLength(255);

            modelBuilder.Entity<LoginInfo>().HasKey(li => li.Id);
            modelBuilder.Entity<LoginInfo>()
                .HasOne(li => li.User)
                .WithMany(u => u.LoginInfos)
                .HasForeignKey(li => li.UserId);
            //modelBuilder.Entity<Permission>()
            //    .HasOptional(p => p.Role)
            //    .WithMany(r => r.Permissions)
            //    .HasForeignKey(p => p.RoleId);
            modelBuilder.Entity<User>().Property(c => c.EmailConfirmed).HasConversion<Int16>();
            modelBuilder.Entity<User>().Property(c => c.LockoutEnabled).HasConversion<Int16>();
            modelBuilder.Entity<User>().Property(c => c.PhoneNumberConfirmed).HasConversion<Int16>();
            modelBuilder.Entity<User>().Property(c => c.TwoFactorEnabled).HasConversion<Int16>();
            //modelBuilder.Entity<User>().Property(c => c.LoginInfos).HasMaxLength(255);


            base.OnModelCreating(modelBuilder);
        }
    }
}
