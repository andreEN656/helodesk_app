using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Uow
{
    public interface IUowProvider
    {
        IUnitOfWork CreateUnitOfWork();
        IUnitOfWork CreateUnitOfWork<TEntityContext>() where TEntityContext : DbContext;
    }
}
