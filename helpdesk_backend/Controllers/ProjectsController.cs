using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repositories.Implementation.ProjectRepository;
using DataAccess.Uow;
using DbEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using ViewModels.Common;

namespace helpdesk_backend.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : BaseController
    {
        private readonly IUowProvider _uowProvider;
        public ProjectsController(IUowProvider uowProvider) :base(uowProvider)
        {
            _uowProvider = uowProvider;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<JsonResult> Get(string data)
        {
            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var requestParams = JsonConvert.DeserializeObject<PagedTableRequestParameters>(data);
                var sqlPagingParams = new List<MySqlParameter>
                {
                    new MySqlParameter("@RowBeginSelection", (object) requestParams.Skip),
                    new MySqlParameter("@RowLenghtSelection", (object) requestParams.PageSize)
                };

                var projectRepository = (ProjectRepository)uow.GetCustomRepository<IProjectRepository>();
                IEnumerable<Project> projects;

                projects = await projectRepository.GetAll(sqlPagingParams.ToArray());
                //return Json(new { total = total, data = groups, esps = listESP }, "application/json", Encoding.UTF8,
                //    JsonRequestBehavior.AllowGet);
                return new JsonResult(new { total = projects.Count(), data = projects });
            }
        }


        [HttpPost]
        [Route("create")]
        public async Task<JsonResult> Create([FromBody]Project data)
        {
            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var projectRepository = (ProjectRepository)uow.GetCustomRepository<IProjectRepository>();

                var output = await projectRepository.Create(data);

                return new JsonResult(output);
            }
        }

        [HttpPost]
        [Route("delete")]
        public async Task<JsonResult> Delete([FromBody]int[] data)
        {
            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var projectRepository = (ProjectRepository)uow.GetCustomRepository<IProjectRepository>();

                foreach (var id in data)
                {
                    await projectRepository.Remove(id);
                }

                return new JsonResult(1);
            }
        }
    }
}