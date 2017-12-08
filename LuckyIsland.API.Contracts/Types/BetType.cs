using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class BetType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Odd> Odds { get; set; }

        public BetType(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}