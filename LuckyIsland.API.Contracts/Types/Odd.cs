using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class Odd
    {
        public int Id { get; set; }
        public double OddFactor { get; set; }
        public double? OddPoint { get; set; }
        public int? TeamId { get; set; }
        public int OddTypeId { get; set; }
    }
}