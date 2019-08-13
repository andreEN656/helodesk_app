using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DbEntities;
using DbMapper.Mapping;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace DataAccess.Repositories
{
    public abstract class EntityRepositoryBase<TContext, TEntity> : RepositoryBase<TContext>, IRepository<TEntity> where TContext : DbContext where TEntity : BaseEntity, new()
    {
        protected EntityRepositoryBase(TContext context) : base(context)
        { }

        public Task<int?> AddAsync(MySqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(MySqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public DataSet CreateDataSet(int TablesCount)
        {
            DataSet result = new DataSet();
            DataTable[] tables = new DataTable[TablesCount];
            while (TablesCount != 0)
            {
                result.Tables.Add(new DataTable());
                TablesCount--;
            }
            return result;
        }

        public string CreateCmdParameters(List<MySqlParameter> mySqlParameters)
        {
            string cmd = "";
            int i = 0;
            foreach (var parameter in mySqlParameters)
            {
                var paramData = parameter.ParameterName.Replace("@", "").Split(' ');
                string separator = "=";
                string logic = "AND";

                if (paramData.Length > 1) separator = paramData[1];
                if (paramData.Length > 2) logic = paramData[2];

                string cmdText = "";
                if (i == 0) {
                    cmdText = " {0}{1}'{2}'";
                    cmd += String.Format(cmdText, paramData[0], separator, parameter.Value);
                    i++;
                    continue;
                }
                cmdText = " {0} {1}{2}'{3}'";
                cmd += String.Format(cmdText, logic, paramData[0], separator, parameter.Value);

                i++;
            }
            return cmd;
        }

        public DataSet GetResult(CommandType CommandType, MySqlParameter[] sqlCommand, string CommandText, int TablesCount)
        {
            try
            {
                DataSet result = CreateDataSet(TablesCount);
                DataTable[] tables = new DataTable[result.Tables.Count];
                result.Tables.CopyTo(tables, 0);

                Context.Database.OpenConnection();
                using (var dbCommand = Context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandType = CommandType;
                    dbCommand.CommandText = CommandText;
                    if (sqlCommand != null) dbCommand.Parameters.AddRange(sqlCommand);

                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        dbCommand.Connection.Open();
                    }

                    using (DbDataReader reader = dbCommand.ExecuteReader())
                    {
                        result.Load(reader, LoadOption.OverwriteChanges, tables);
                    }
                }
                Context.Database.CloseConnection();

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<TEntity>> GetResultMapping(CommandType CommandType, MySqlParameter[] sqlCommand, string CommandText)
        {
            try
            {
                DataSet ds = CreateDataSet(1);
                DataTable[] tables = new DataTable[ds.Tables.Count];
                ds.Tables.CopyTo(tables, 0);

                Context.Database.OpenConnection();
                using (DbCommand dbCommand = Context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandType = CommandType;
                    dbCommand.CommandText = CommandText;
                    if (sqlCommand != null) dbCommand.Parameters.AddRange(sqlCommand);

                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        await dbCommand.Connection.OpenAsync();
                    }

                    using (DbDataReader reader = dbCommand.ExecuteReader())
                    {
                        ds.Load(reader, LoadOption.OverwriteChanges, tables);
                    }
                }
                Context.Database.CloseConnection();
                var mapper = new DataNamesMapper<TEntity>();
                return mapper.Map(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<TEntity> GetResultMappingNotAsync(CommandType CommandType, MySqlParameter[] sqlCommand, string CommandText)
        {
            try
            {
                DataSet ds = CreateDataSet(1);
                DataTable[] tables = new DataTable[ds.Tables.Count];
                ds.Tables.CopyTo(tables, 0);

                Context.Database.OpenConnection();
                using (DbCommand dbCommand = Context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandType = CommandType;
                    dbCommand.CommandText = CommandText;
                    if (sqlCommand != null) dbCommand.Parameters.AddRange(sqlCommand);

                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        dbCommand.Connection.Open();
                    }

                    using (DbDataReader reader = dbCommand.ExecuteReader())
                    {
                        ds.Load(reader, LoadOption.OverwriteChanges, tables);
                    }
                }
                Context.Database.CloseConnection();
                var mapper = new DataNamesMapper<TEntity>();
                return mapper.Map(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DataTable> GetResult(CommandType CommandType, MySqlParameter[] sqlCommand, string CommandText)
        {
            try
            {
                DataSet ds = CreateDataSet(1);
                DataTable[] tables = new DataTable[ds.Tables.Count];
                ds.Tables.CopyTo(tables, 0);

                Context.Database.OpenConnection();
                using (DbCommand dbCommand = Context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandType = CommandType;
                    dbCommand.CommandText = CommandText;
                    if (sqlCommand != null) dbCommand.Parameters.AddRange(sqlCommand);

                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        await dbCommand.Connection.OpenAsync();
                    }

                    using (DbDataReader reader = dbCommand.ExecuteReader())
                    {
                        ds.Load(reader, LoadOption.OverwriteChanges, tables);
                    }
                }
                Context.Database.CloseConnection();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> PutData(CommandType CommandType, MySqlParameter[] sqlCommand, string CommandText)
        {
            try
            {
                Context.Database.OpenConnection();
                using (DbCommand dbCommand = Context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandType = CommandType;
                    dbCommand.CommandText = CommandText;
                    if (sqlCommand != null) dbCommand.Parameters.AddRange(sqlCommand);

                    if (dbCommand.Connection.State != ConnectionState.Open)
                    {
                        await dbCommand.Connection.OpenAsync();
                    }

                    var result = await dbCommand.ExecuteScalarAsync();
                    Context.Database.CloseConnection();
                    return int.Parse(result.ToString());
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        public Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> QueryPageAsync(int startRij, int aantal, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void SetUnchanged(TEntity entitieit)
        {
            throw new NotImplementedException();
        }

        public Task<int?> UpdateAsync(TEntity items, MySqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> items, MySqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }


        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

    }
}
