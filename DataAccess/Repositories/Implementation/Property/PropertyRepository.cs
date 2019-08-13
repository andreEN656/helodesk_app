using DataAccess.Context;
using DbEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementation.PropertyRepository
{
    public class PropertyRepository : EntityRepositoryBase<HelpdeskAppContext, Property>, IPropertyRepository
    {
        public PropertyRepository(HelpdeskAppContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Property>> GetProperty(string tableName)
        {
            var property = await GetResultMapping(CommandType.Text, null, $"SELECT * FROM `{tableName}`");
            return property;
        }  
    }
}
