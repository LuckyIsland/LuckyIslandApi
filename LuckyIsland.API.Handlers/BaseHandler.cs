using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LuckyIsland.API.Handlers
{
    public class BaseHandler
    {
        private string version;
        protected string Version
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(version))
                    {
                        var v = Assembly.GetExecutingAssembly().GetName().Version;
                        version = v.ToString();
                    }
                }
                catch
                {
                    version = "1.0.0.0";
                }
                return version;
            }
        }

        public BaseHandler()
        {

        }
    }
}
