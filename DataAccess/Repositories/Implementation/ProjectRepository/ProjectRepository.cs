using DataAccess.Context;
using DbEntities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementation.ProjectRepository
{
    public class ProjectRepository : EntityRepositoryBase<HelpdeskAppContext, Project>, IProjectRepository
    {
        public ProjectRepository(HelpdeskAppContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetAll(MySqlParameter[] mySqlParameters)
        {
            var projects = await GetResultMapping(CommandType.Text, mySqlParameters, "SELECT * FROM `projects`");

            return projects;
        }

        public async Task<int?> Create(Project project)
        {
            string cmdInsert = "INSERT INTO `helpdesk`.`projects` (`id`, `name`, `attribute01`) " +
                $"VALUES (NULL, '{project.Name}', '{project.Attribute01}');";

            var data = await PutData(CommandType.Text, null, cmdInsert);
            return data;
        }

        public new async Task<int?> Remove(int id)
        {
            string cmdDelete = $"DELETE FROM `helpdesk`.`projects` WHERE `id`={id}";
            
            var data = await PutData(CommandType.Text, null, cmdDelete);
            return data;
        }

    }
}
