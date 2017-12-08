using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class SportsTree
    {
        public List<Sport> Sports { get; set; }
        public SportsTree()
        {
            this.Sports = new List<Sport>();
        }
    }
}