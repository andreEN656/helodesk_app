using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Uow
{
    public class UowProvider : IUowProvider
    {
        public UowProvider()
        { }

        public UowProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;

        public IUnitOfWork CreateUnitOfWork()
        {
            var _context = (DbContext)_serviceProvider.GetService(typeof(IEntityContext));

            //if (!trackChanges)
            //    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var uow = new UnitOfWork(_context, _serviceProvider);

            return uow;

        }

        public IUnitOfWork CreateUnitOfWork<TEntityContext>() where TEntityContext : DbContext
        {
            var _context = (TEntityContext)_serviceProvider.GetService(typeof(IEntityContext));

            //if (!trackChanges)
            //    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var uow = new UnitOfWork(_context, _serviceProvider);
            return uow;
        }
    }
}
