using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.Repositories.Implementation.TaskRepository;
using DataAccess.Uow;
using DbEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using ViewModels;
using ViewModels.Common;

namespace helpdesk_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly IUowProvider _uowProvider;
        public TasksController(IUowProvider uowProvider) : base(uowProvider)
        {
            _uowProvider = uowProvider;
        }

        [HttpPost]
        [Route("create")]
        public async Task<JsonResult> Create()
        {
            try
            {
                using (var uow = _uowProvider.CreateUnitOfWork())
                {
                    var data = JsonConvert.DeserializeObject<Tasks>(Request.Form["data"].ToString());

                    if (Request.Form.Files.Count > 0)
                    {
                        for (var i = 0; i < Request.Form.Files.Count; i++)
                        {
                            var file = Request.Form.Files[i];
                            var parts = file.FileName.Split('.');
                            var ext = string.Empty;
                            if (parts.Length > 1)
                                ext = parts.LastOrDefault();
                            var filename = $"{file.FileName}";

                            using (var target = new MemoryStream())
                            {
                                file.CopyTo(target);
                                target.Position = 0;

                                using (FileStream fs = new FileStream(@"C:\public_html\some_folder\" + filename, FileMode.Create))
                                {
                                    target.CopyTo(fs);
                                    fs.Flush();
                                }

                            }
                            data.Files.Add(new Files() {
                                Url = "/some_folder/" + filename,
                                Name = filename
                            });
                            
                        }
                    }
                    var taskRepository = (TaskRepository)uow.GetCustomRepository<ITaskRepository>();
                    data.FromUserId = CurrentUserId;
                    data.FromUserName = CurrentUserName;

                    var output = await taskRepository.Create(data);

                    return new JsonResult(output);
                }
            } catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<JsonResult> Update()
        {

            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var data = JsonConvert.DeserializeObject<Tasks>(Request.Form["data"].ToString());

                var taskRepository = (TaskRepository)uow.GetCustomRepository<ITaskRepository>();
                var output = await taskRepository.Update(data);

                return new JsonResult(output);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<JsonResult> GetAll(string data)
        {

            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var requestParams = JsonConvert.DeserializeObject<PagedTableRequestParameters>(data);
                var sqlPagingParams = new List<MySqlParameter>
                {
                    new MySqlParameter("@RowBeginSelection", (object) requestParams.Skip),
                    new MySqlParameter("@RowLenghtSelection", (object) requestParams.PageSize)
                };

                var sqlRequestParams = new List<MySqlParameter>();

                if (requestParams.Filter != null)
                {
                    var filter = JsonConvert.DeserializeObject<Filter>(requestParams.Filter);
                    foreach (var f in filter.Filters)
                    {
                        switch (f.Field)
                        {
                            case "projectId":
                                sqlRequestParams.Add(new MySqlParameter("@project_id", (object)f.Value));
                                break;
                            case "typeview":
                                sqlRequestParams.Add(new MySqlParameter("@finish_datetime >=", (object)DateTime.Now.ToString("yyyy-MM-dd")));
                                sqlRequestParams.Add(new MySqlParameter("@finish_datetime <", (object)DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));


                                switch (f.Value)
                                {
                                    case "0":
                                        sqlRequestParams.Add(new MySqlParameter("@from_user_id =", (object)CurrentUserId));
                                        sqlRequestParams.Add(new MySqlParameter("@`tasks_users`.user_id = OR", (object)CurrentUserId));
                                        break;
                                    case "1":
                                        sqlRequestParams.Add(new MySqlParameter("@from_user_id", (object)CurrentUserId));
                                        break;
                                    case "2":
                                        sqlRequestParams.Add(new MySqlParameter("@`tasks_users`.user_id", (object)CurrentUserId));
                                        break;
                                }
                                break;
                        }
                    }
                }

                var taskRepository = (TaskRepository)uow.GetCustomRepository<ITaskRepository>();

                var output = await taskRepository.GetAll(sqlRequestParams);

                return new JsonResult(new { total = output.Count(), data = output });
            }
        }


        [HttpGet]
        [Route("Get")]
        public async Task<JsonResult> Get(int id)
        {

            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var taskRepository = (TaskRepository)uow.GetCustomRepository<ITaskRepository>();

                var output = await taskRepository.Get(id);

                return new JsonResult(output);
            }
        }

        [HttpPost]
        [Route("CreateComment")]
        public async Task<JsonResult> CreateComment()
        {

            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var data = JsonConvert.DeserializeObject<Comment>(Request.Form["data"].ToString());

                if (Request.Form.Files.Count > 0)
                {
                    for (var i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var file = Request.Form.Files[i];
                        var parts = file.FileName.Split('.');
                        var ext = string.Empty;
                        if (parts.Length > 1)
                            ext = parts.LastOrDefault();
                        var filename = $"{file.FileName}";

                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            target.Position = 0;
                            using (FileStream fs = new FileStream(@"C:\public_html\some_folder\" + filename, FileMode.Create))
                            {
                                target.CopyTo(fs);
                                fs.Flush();
                            }

                        }
                        data.Files.Add(new Files()
                        {
                            Url = "/some_folder/" + filename,
                            Name = filename
                        });

                    }
                }

                data.UserId = CurrentUserId;
                data.UserName = CurrentUserName;
                var taskRepository = (TaskRepository)uow.GetCustomRepository<ITaskRepository>();
                
                var output = await taskRepository.CreateComment(data);

                return new JsonResult(output);
            }
        }
    }
}