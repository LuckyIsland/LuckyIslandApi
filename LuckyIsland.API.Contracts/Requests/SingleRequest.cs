using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class SingleRequest
    {
        public SportsTreeRequest GetSportsTreeBySport { get; set; }
        public EmailSenderRequest SendEmail { get; set; }
        public EventOddsRequest GetEventOdds { get; set; }
        public LoginRequest Login { get; set; }
    }
}