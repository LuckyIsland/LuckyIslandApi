using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}