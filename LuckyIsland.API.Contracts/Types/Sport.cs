using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Country> Countries { get; set; }
        public bool IsFavorite { get; set; }
        public int Ordering { get; set; }

        public Sport(int id, string name, bool isfav, int ord)
        {
            Id = id;
            Name = name;
            IsFavorite = isfav;
            Ordering = ord;
        }

        public Sport(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int EventCount
        {
            get
            {
                if (Countries != null)
                    return this.Countries.Sum(x => x.EventCount);
                else
                    return 0;
            }
        }
    }
}