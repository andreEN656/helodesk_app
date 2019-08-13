using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ViewModels;

namespace helpdesk_backend.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]SimpleUserViewModel model)
        {
            var user = userManager.Users.Where(c => c.UserName == model.UserName).FirstOrDefault();
            var result = await userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                 //   new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 //identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol),
                 //identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id)
                };
                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ASP.NET Identity"));

                // Create the JWT security token and encode it.
                var jwt = new JwtSecurityToken(
                    issuer: "http://localhost:56813",
                    //audience: "http://localhost:53120",
                    claims: claims,
                    //notBefore: _jwtOptions.NotBefore,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256));

                long epoch = (jwt.ValidTo.Ticks - 621355968000000000) / 10000000;
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwt),
                    expiration = epoch
                });
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        [Route("GetList")]
        public async Task<JsonResult> GetList(string data)
        {
            //todo: Постраничный вывод
            try
            {
                
                var total = userManager.Users.Count();
                var roles = roleManager.Roles.ToList();

                var users = userManager.Users.ToList();

                var usersList = AutoMapper.Mapper.Map<IEnumerable<User>, IList<UserViewModel>>(users);

                foreach (var user in users)
                {
                    var userVM = usersList.FirstOrDefault(u => u.Id == user.Id);

                    var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                    var role_ = roleManager.Roles.FirstOrDefault(c => c.Name == role);
                    userVM.Role = new RoleBaseViewModel
                    {
                        Id = role_.Id,
                        Name = role_.Name
                    };
                }
                return new JsonResult(new { total = total, data = usersList });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Get")]
        public async Task<JsonResult> Get(string id)
        {
            //todo: Постраничный вывод
            try
            {
                var user = userManager.Users.FirstOrDefault(u => u.Id == id);

                var userVM = AutoMapper.Mapper.Map<User, UserViewModel>(user);

                var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                var role_ = roleManager.Roles.FirstOrDefault(c => c.Name == role);

                userVM.Role = new RoleBaseViewModel
                {
                    Id = role_.Id,
                    Name = role_.Name
                };
                //string output = JsonConvert.SerializeObject(product);

                return new JsonResult(userVM);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<JsonResult> Create()
        {
            var data = JsonConvert.DeserializeObject<UserViewModel>(Request.Form["data"].ToString());

            var user = AutoMapper.Mapper.Map<UserViewModel, User>(data);
            user.Id = Guid.NewGuid().ToString().ToLowerInvariant();
            user.Email = user.PublicEmail;

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

                        using (FileStream fs = new FileStream(@"C:\public_html\some_folder\users\" + filename, FileMode.Create))
                        {
                            target.CopyTo(fs);
                            fs.Flush();
                        }

                    }

                    user.PhotoUrl = "/some_folder/users/" + filename;
                }
            }

            var result = await userManager.CreateAsync(user, user.PasswordHash);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Owner");

                return new JsonResult(user);
            }
            return new JsonResult(false);
        }

        [HttpPost]
        [Route("update")]
        public async Task<JsonResult> Update()
        {
            var data = JsonConvert.DeserializeObject<UserViewModel>(Request.Form["data"].ToString());

            var dbUser = userManager.Users.FirstOrDefault(u => u.Id == data.Id);
            dbUser.ChiefName = data.ChiefName;
            dbUser.Description = data.Description;
            dbUser.FullName = data.FullName;
            dbUser.PublicEmail = data.PublicEmail;
            dbUser.PhoneNumber = data.PhoneNumber;

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

                        using (FileStream fs = new FileStream(@"C:\public_html\some_folder\users\" + dbUser.Id + "." + ext, FileMode.Create))
                        {
                            target.CopyTo(fs);
                            fs.Flush();
                        }

                    }

                    dbUser.PhotoUrl = "/some_folder/users/" + dbUser.Id + "." + ext;
                }
            }

            if (data.PasswordHash != dbUser.PasswordHash)
            {
                var hasher = userManager.PasswordHasher;
                //Какой-то костыль, но без него не работает апдейт пароля. Ругается на то, что юзер с 
                //такой почтой уже существует
                var res1 = await userManager.RemovePasswordAsync(dbUser);
                var res2 = await userManager.AddPasswordAsync(dbUser, data.PasswordHash);
                dbUser.PasswordHash = hasher.HashPassword(dbUser, data.PasswordHash);
            }
            //await userManager.RemoveFromRoleAsync(user, role);
            //await userManager.AddToRoleAsync(user, data.Role.Name);
            var result = await userManager.UpdateAsync(dbUser);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<JsonResult> Delete([FromBody]List<string> data)
        {
            data.Remove(User.FindFirst(JwtRegisteredClaimNames.Jti).Value);
            foreach (var id in data)
            {
                var user = await userManager.FindByIdAsync(id);
                var result = await userManager.DeleteAsync(user);
            }
            return new JsonResult(new { success = true, message = "" });
        }
    }
}