using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? EventCount { get; }
        public int? Ordering { get; set; }
        public int? SportId { get; set; }
        public List<Event> Events { get; set; }
        public List<BetTypeGroup> BetTypeGroups { get; set; }

        public League(string name, int id, int? eventCount, int? sportId, int? ordering)
        {
            Name = name;
            Id = id;
            EventCount = eventCount;
            SportId = sportId;
            Ordering = ordering;
        }

        public League(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}