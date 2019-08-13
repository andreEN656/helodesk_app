using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModels
{
    public class LoginInfo
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string IP { get; set; }

        public virtual string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
