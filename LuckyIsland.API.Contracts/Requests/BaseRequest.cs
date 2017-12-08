using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class BaseRequest
    {
        public string LanguageCode { get; set; }
        public int? Duration { get; set; }
        public DateTime? Day { get; set; }
        public int? Bias { get; set; }
    }
}