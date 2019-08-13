using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityModels
{
    public class User : IdentityUser
    {
        public string PublicEmail { get; set; }

        public string FullName { get; set; }

        public string Description { get; set; }

        public string ChiefName { get; set; }

        public string ChiefId { get; set; }

        public string PhotoUrl { get; set; }


        #region Navigation fields

        public virtual ICollection<LoginInfo> LoginInfos { get; set; }

        #endregion

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            //var userIdentity = await manager.Crea(this);
            return null;
        }
    }
}
