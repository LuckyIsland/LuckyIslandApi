using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class EventOddsRequest : BaseRequest
    {
        public string BetTypeGroupIds { get; set; }
        public string LeagueIds { get; set; }
        public string EventIds { get; set; }
        public int Type { get; set; }
    }
}