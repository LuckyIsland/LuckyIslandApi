using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class Account
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public int BetttingLineTemplateId { get; set; }

    }
}