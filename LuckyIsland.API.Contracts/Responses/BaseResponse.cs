using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyIsland.API.Contracts
{
    public class BaseResponse
    {
        
    }

    public class BaseResponse<T> : BaseResponse
    {
        public List<T> Items { get; set; }
    }
}