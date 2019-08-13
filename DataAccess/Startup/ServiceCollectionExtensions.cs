using DataAccess.Context;
using DataAccess.Repositories;
using DataAccess.Repositories.Implementation.NotificationRepository;
using DataAccess.Repositories.Implementation.ProjectRepository;
using DataAccess.Repositories.Implementation.PropertyRepository;
using DataAccess.Repositories.Implementation.TaskRepository;
using DataAccess.Uow;
using IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                //параметры пароля
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                //параметры пользователя
                options.User.RequireUniqueEmail = true;

                //параметры блокировки
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<HelpdeskAppContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddDataAccess/*<TEntityContext>*/(this IServiceCollection services, IConfiguration Configuration) /*where TEntityContext : EntityContextBase<TEntityContext>*/
        {
            RegisterDataAccess/*<TEntityContext>*/(services, Configuration);
            return services;
        }

        private static void RegisterDataAccess/*<TEntityContext>*/(IServiceCollection services, IConfiguration Configuration)/* where TEntityContext : EntityContextBase<TEntityContext>*/
        {
            services.AddDbContext<HelpdeskAppContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.TryAddTransient<IUowProvider, UowProvider>();
            services.TryAddTransient<IEntityContext, HelpdeskAppContext>();

            services.TryAddTransient<IProjectRepository, ProjectRepository>();
            services.TryAddTransient<ITaskRepository, TaskRepository>();
            services.TryAddTransient<IPropertyRepository, PropertyRepository>();
            services.TryAddTransient<INotificationRepository, NotificationRepository>();

            services.TryAddTransient(typeof(IRepository<>), typeof(GenericEntityRepository<>));
            //services.TryAddTransient(typeof(IDataPager<>), typeof(DataPager<>));
        }

        private static void ValidateMandatoryField(string field, string fieldName)
        {
            if (field == null) throw new ArgumentNullException(fieldName, $"{fieldName} cannot be null.");
            if (field.Trim() == String.Empty) throw new ArgumentException($"{fieldName} cannot be empty.", fieldName);
        }

        private static void ValidateMandatoryField(object field, string fieldName)
        {
            if (field == null) throw new ArgumentNullException(fieldName, $"{fieldName} cannot be null.");
        }
    }
}
