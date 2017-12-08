using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class SingleResponse
    {
        public SportsTree SportsTree { get; set; }
        public Sport EventOdds { get; set; }
        public Account Account { get; set; }
        public string SessionId { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }

        public enum ResponseStatus: int
        {
            OK = 0,
            EMAIL_ERROR = 1
        };
    }
}