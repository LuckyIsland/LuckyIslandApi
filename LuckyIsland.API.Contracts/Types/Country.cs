using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImageCode { get; set; }
        public bool IsFavorite { get; set; }
        public List<League> Leagues { get; set; }
        public Country(string code, string name, string imageCode, bool isfav)
        {
            Code = code;
            Name = name;
            ImageCode = imageCode;
            IsFavorite = isfav;
        }

        public Country(string countryCode, string countryName)
        {
            Code = countryCode;
            Name = countryName;
        }

        public int EventCount
        {
            get
            {
                if (Leagues != null)
                    return this.Leagues.Sum(x => x.EventCount.HasValue ? x.EventCount.Value : 0);
                else
                    return 0;
            }
        }
    }
}