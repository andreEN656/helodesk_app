using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class UserViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("role")]
        public RoleBaseViewModel Role { get; set; }

        [JsonProperty("email")]
        public string PublicEmail { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("chiefName")]
        public string ChiefName { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("passwordConfirm")]
        public string PasswordConfirm { get; set; }

        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; set; }

        public UserViewModel()
        {
            PasswordConfirm = Password;
        }
    }
}
