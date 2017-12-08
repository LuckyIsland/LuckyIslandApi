using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class BetTypeGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ordering { get; set; }
        public List<BetType> BetTypes { get; set; }

        public BetTypeGroup(int id, string name, int ordering)
        {
            Id = id;
            Name = name;
            Ordering = ordering;
        }
    }
}