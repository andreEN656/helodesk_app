using DataAccess.Context;
using DbEntities;
using DbMapper.Mapping;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementation.TaskRepository
{
    public class TaskRepository : EntityRepositoryBase<HelpdeskAppContext, Tasks>, ITaskRepository
    {
        public TaskRepository(HelpdeskAppContext context) : base(context)
        {
        }

        public async Task<Tasks> Get(int id)
        {

            var selectText = "SELECT * FROM `tasks` WHERE `id`=" + id;

            var task = (await GetResultMapping(CommandType.Text, null, selectText)).FirstOrDefault();

            selectText = "SELECT `tasks_users`.`user_id`, `AspNetUsers`.`UserName` FROM `tasks_users`" +
                " LEFT JOIN `AspNetUsers` ON `AspNetUsers`.`Id` = `tasks_users`.`user_id`" +
                " WHERE `task_id` = " + task.Id;

            var ds = await GetResult(CommandType.Text, null, selectText);
            task.Users = (new DataNamesMapper<TaskUser>().Map(ds)).ToList();

            selectText = "SELECT * FROM `tasks_comments` WHERE `task_id` = " + task.Id;
            ds = await GetResult(CommandType.Text, null, selectText);
            task.Comments = (new DataNamesMapper<Comment>().Map(ds)).ToList();

            selectText = "SELECT * FROM `tasks_files` WHERE `task_id` = " + task.Id;
            ds = await GetResult(CommandType.Text, null, selectText);
            task.Files = (new DataNamesMapper<Files>().Map(ds)).ToList();

            foreach (var comment in task.Comments)
            {
                selectText = "SELECT * FROM `comments_files` WHERE `comment_id` = " + comment.Id;
                ds = await GetResult(CommandType.Text, null, selectText);
                comment.Files = (new DataNamesMapper<Files>().Map(ds)).ToList();
            }

            return task;
        }

        public async Task<IEnumerable<Tasks>> GetAll(List<MySqlParameter> mySqlParameters)
        {
            //var cmd = Context.Database.GetDbConnection().CreateCommand() as MySqlCommand;
            
            var selectText = "SELECT `tasks`.*, `task_status`.`name` AS status_name FROM `tasks`" +
                " LEFT JOIN `tasks_users` ON `tasks_users`.`task_id` = `tasks`.`id`" +
                " LEFT JOIN `task_status` ON `task_status`.`id` = `tasks`.`status_id`";

            if (mySqlParameters.Count != 0)
                selectText += " WHERE " + CreateCmdParameters(mySqlParameters);
            selectText += " GROUP BY `tasks`.`id`";
            var tasks = await GetResultMapping(CommandType.Text, null, selectText);

            foreach (var task in tasks)
            {
                selectText = "SELECT `tasks_users`.`user_id`, `AspNetUsers`.`UserName` FROM `tasks_users`" +
                            " LEFT JOIN `AspNetUsers` ON `AspNetUsers`.`Id` = `tasks_users`.`user_id`" +
                            " WHERE `task_id` = " + task.Id;
                var ds = await GetResult(CommandType.Text, null, selectText);
                task.Users = (new DataNamesMapper<TaskUser>().Map(ds)).ToList();

            }

            return tasks;
        }

        public async Task<int?> Create(Tasks task)
        {
            string cmdInsert = "INSERT INTO `helpdesk`.`tasks` (`id`, `status_id`, `priority_id`, `project_id`, `from_user_id`," +
                " `from_user_name`,`name`, `description`,`finish_datetime`) " +
                $"VALUES (NULL, '{task.StatusId}', '{task.PriorityId}', '{task.ProjectId}', '{task.FromUserId}', '{task.FromUserName}', " +
                $"'{task.Name}', '{task.Description}', '{task.FinishDatetime.ToString("yyyy-MM-dd HH:mm:ss")}'); SELECT LAST_INSERT_ID();";

            var idInsert = await PutData(CommandType.Text, null, cmdInsert);

            foreach(var user in task.Users)
            {
                cmdInsert = "INSERT INTO `helpdesk`.`tasks_users` (`user_id`, `task_id`) " +
                $"VALUES ('{user.UserId}', '{idInsert}')";
                await PutData(CommandType.Text, null, cmdInsert);
            }

            foreach (var file in task.Files)
            {
                cmdInsert = "INSERT INTO `helpdesk`.`tasks_files` (`task_id`, `url`, `name`) " +
                    $"VALUES ('{idInsert}', '{file.Url}', '{file.Name}')";
                await PutData(CommandType.Text, null, cmdInsert);
            }

            return idInsert;
        }

        public new async Task<int?> Update(Tasks task)
        {
            string cmdInsert = $"UPDATE `helpdesk`.`tasks` SET `status_id`={task.StatusId}, " +
                $"`priority_id`={task.PriorityId}, `finish_datetime`='{task.FinishDatetime.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                $" WHERE `id`={task.Id}";
            var idInsert = await PutData(CommandType.Text, null, cmdInsert);

            return idInsert;
        }

        public async Task<int?> CreateComment(Comment comment)
        {
            string cmdInsert = "INSERT INTO `helpdesk`.`tasks_comments` (`id`, `task_id`, `user_id`, `user_name`, `text`) " +
                $"VALUES (NULL, '{comment.TaskId}', '{comment.UserId}', '{comment.UserName}', '{comment.Text}'); SELECT LAST_INSERT_ID();";

            var idInsert = await PutData(CommandType.Text, null, cmdInsert);

            foreach (var file in comment.Files)
            {
                cmdInsert = "INSERT INTO `helpdesk`.`comments_files` (`comment_id`, `url`, `name`) " +
                    $"VALUES ('{idInsert}', '{file.Url}', '{file.Name}')";
                await PutData(CommandType.Text, null, cmdInsert);
            }


            return idInsert;
        }

        //public new async Task<int?> Remove(int id)
        //{
        //    string cmdDelete = $"DELETE FROM `helpdesk`.`projects` WHERE `id`={id}";

        //    var data = await PutData(CommandType.Text, null, cmdDelete);
        //    return data;
        //}

    }
}
