using LuckyIsland.API.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace LuckyIsland.API.Handlers
{
    /// <summary>
    /// Summary description for BettingHandler
    /// </summary>
    public class BettingHandler : BaseHandler, IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Session != null)
                    context.Session["RefreshKey"] = DateTime.Now;

                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                if (context.Request.HttpMethod == "POST" && context.Request.InputStream.Length != 0)
                {
                    var inputStream = new MemoryStream();
                    context.Request.InputStream.CopyTo(inputStream);

                    string requestString = Encoding.UTF8.GetString(inputStream.ToArray());
                    var request = JsonConvert.DeserializeObject<SingleRequest>(requestString);
                    var outputStream = new MemoryStream();

                    SingleResponse response = new SingleResponse();
                    BettingManager manager = new BettingManager();

                    if (request.GetSportsTreeBySport != null)
                        response.SportsTree = manager.GetSportsTree(context, request.GetSportsTreeBySport);

                    if (request.SendEmail != null)
                        response = manager.SendEmail(request.SendEmail);

                    if (request.GetEventOdds != null)
                        response.EventOdds = manager.GetEventOdds(context, request.GetEventOdds);

                    if (request.Login != null)
                        response.Account = manager.Login(context, request.Login);

                    if (context.Session != null && context.Session.IsCookieless && context.Response.Cookies["SessionId"] != null)
                    {
                        response.SessionId = context.Response.Cookies["SessionId"].Value;
                        context.Response.Cookies.Remove("SessionId");
                    }

                    var outputString = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc
                    });
                    var outputArray = Encoding.UTF8.GetBytes(outputString);
                    outputStream.Write(outputArray, 0, outputArray.Length);
                    outputStream.Position = 0;

                    context.Response.BinaryWrite(outputStream.ToArray());
                    context.Response.ContentType = "application/json";
                }
                else
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 400;
                    context.Response.Write(this.Version);
                }
            }
#if DEBUG
            catch (Exception e)
            {
                throw e;
            }
#else
            catch (Exception ex)
            {
                //LogHelper.WriteSystemEvents(ex, "BettingHandler");

                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.Write(this.Version);
            }
#endif
            finally
            {
                context.Response.Headers.Add("API-Version", this.Version);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}