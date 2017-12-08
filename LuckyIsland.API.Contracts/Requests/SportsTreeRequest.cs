using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class SportsTreeRequest : BaseRequest
    {
        public int? BetTypeGroupTemplateId { get; set; }
    }
}