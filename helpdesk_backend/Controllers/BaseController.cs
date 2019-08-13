using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.Repositories.Implementation.ProjectRepository;
using DataAccess.Repositories.Implementation.PropertyRepository;
using DataAccess.Uow;
using DbEntities;
using IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using ViewModels.Common;

namespace helpdesk_backend.Controllers
{
    [Authorize]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly IUowProvider _uowProvider;

        public BaseController(IUowProvider uowProvider)
        {
            _uowProvider = uowProvider;
        }

        private string _currentUserId;
        private string _currentUserName;

        protected string CurrentUserId
        {
            get
            {
                if (_currentUserId != null) return _currentUserId;
                _currentUserId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
                return _currentUserId;
            }
        }

        protected string CurrentUserName
        {
            get
            {
                if (_currentUserName != null) return _currentUserName;
                _currentUserName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return _currentUserName;
            }
        }

        [HttpGet]
        [Route("GetProperty")]
        public async Task<JsonResult> GetProperty(string tableName)
        {
            using (var uow = _uowProvider.CreateUnitOfWork())
            {

                var projectRepository = (PropertyRepository)uow.GetCustomRepository<IPropertyRepository>();

                var properties = await projectRepository.GetProperty(tableName);
                //return Json(new { total = total, data = groups, esps = listESP }, "application/json", Encoding.UTF8,
                //    JsonRequestBehavior.AllowGet);
                return new JsonResult(properties.ToList());
            }
        }
    }
}