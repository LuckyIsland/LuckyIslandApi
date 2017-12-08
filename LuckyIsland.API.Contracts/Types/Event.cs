using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class Event
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public int? Interval { get; set; }
        public int? HomeId { get; set; }
        public int? GuestId { get; set; }
        public string HomeName { get; set; }
        public string GuestName { get; set; }
        public List<BetType> BetTypes { get; set; }
        public List<BetTypeGroup> BetTypeGroups { get; set; }
    }
}