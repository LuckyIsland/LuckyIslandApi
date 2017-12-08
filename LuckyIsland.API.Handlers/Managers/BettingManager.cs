using LuckyIsland.API.Contracts;
using LuckyIsland.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace LuckyIsland.API.Handlers
{
    public class BettingManager
    {
        internal SportsTree GetSportsTree(HttpContext context, SportsTreeRequest request)
        {
            using (var dc = DataContexts.GetDataContext())
            {
                //int bettingLineTemplateId = this.GetBettingLineTemplateId(context);
                int bettingLineTemplateId = 1;

                string key = $"sportstree_{request.LanguageCode}_{request.Duration}_{request.Bias}_{bettingLineTemplateId}_{request.Day?.Year}_{request.Day?.Month}_{request.Day?.Day}";
                CacheManager manager = new CacheManager(key);
                var result = manager.GetCache() as SportsTree;
                if (result == null)
                {
                    var ans = dc.webAPI_Client_GetSportsTree(request.LanguageCode, request.Duration, bettingLineTemplateId, request.Day, request.Bias, request.BetTypeGroupTemplateId).ToList();
                    result = GenerateSportsTree(ans);
                    manager.SetCache(result, 10 * 60);
                }
                return result;
            }
        }

        internal SingleResponse SendEmail(EmailSenderRequest request)
        {
            try
            {
                SmtpClient client = new SmtpClient()
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = Convert.ToInt32(587),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential("ooo.lucky.island@gmail.com", "123qwasd")
                };
                MailMessage mail = new MailMessage("ooo.lucky.island@gmail.com", request.Email)
                {
                    From = new MailAddress("ooo.lucky.island@gmail.com"),
                    Subject = request.Subject,
                    Body = request.Message
                };
                mail.To.Add(new MailAddress(request.Email));
                client.Send(mail);
                return new SingleResponse() { Status = (int)SingleResponse.ResponseStatus.OK };
            }
            catch (Exception e)
            {
                return new SingleResponse() { Status = (int)SingleResponse.ResponseStatus.EMAIL_ERROR, Message = e.ToString() };
            }
        }

        private static SportsTree GenerateSportsTree(List<DBSportTree> tree)
        {
            tree = tree.OrderBy(x => x.BetTypeGroupOrdering).ToList();

            SportsTree sportTree = new SportsTree();

            foreach (var elem in tree)
            {
                if (!sportTree.Sports.Select(x => x.Id).Contains(elem.SportId))
                {
                    sportTree.Sports.Add(new Sport(elem.SportId, elem.SportName, true, 1));
                    sportTree.Sports.Last().Countries = new List<Country>();
                }
                var sport = sportTree.Sports.FirstOrDefault(x => x.Id == elem.SportId);
                if (!sport.Countries.Select(y => y.Code).Contains(elem.CountryCode))
                {
                    sport.Countries.Add(new Country(elem.CountryCode, elem.CountryName, elem.ImageCode, true));
                    sport.Countries.Last().Leagues = new List<League>();
                }
                var country = sport.Countries.FirstOrDefault(x => x.Code == elem.CountryCode);
                if (!country.Leagues.Select(l => l.Id).Contains(elem.LeagueId))
                {
                    country.Leagues.Add(new League(elem.LeagueName, elem.LeagueId, elem.EventCount, elem.SportId, 1));
                    country.Leagues.Last().BetTypeGroups = new List<BetTypeGroup>();
                }
                var league = country.Leagues.FirstOrDefault(x => x.Id == elem.LeagueId);
                if (league != null)
                {
                    league.BetTypeGroups.Add(new BetTypeGroup(elem.BetTypeGroupId, elem.BetTypeGroupName, elem.BetTypeGroupOrdering));
                }
            }

            return sportTree;
        }

        internal Sport GetEventOdds(HttpContext context, EventOddsRequest request)
        {
            int bettingLineTemplateId = 1;

            string key = $"eventodds_{request.BetTypeGroupIds}_{request.LeagueIds}_{request.LanguageCode}_{request.Duration}_{request.Bias}_{bettingLineTemplateId}_{request.Day?.Year}_{request.Day?.Month}_{request.Day?.Day}";
            CacheManager manager = new CacheManager(key);

            var result = manager.GetCache() as Sport;

            if (result == null)
            {
                using (var dc = DataContexts.GetDataContext())
                {
                    key = $"events_{request.LanguageCode}_{request.LeagueIds}_{request.EventIds}_{request.Day}_{request.Bias}_{request.Duration}_{bettingLineTemplateId}";

                    CacheManager eventsCache = new CacheManager(key);
                    var events = eventsCache.GetCache() as List<webAPI_Client_GetEventInfoResult>;

                    if (events == null)
                    {
                        events = dc.webAPI_Client_GetEventInfo(request.LeagueIds, request.EventIds, request.LanguageCode, request.Day,
                            request.Bias, request.Duration, bettingLineTemplateId).ToList();
                        eventsCache.SetCache(events, 10 * 60);
                    }

                    var eventIds = events.Select(ev => ev.EventID).Distinct().ToList();
                    
                    var odds = dc.webAPI_Client_GetEventOdds(String.Join("|", eventIds), request.LanguageCode, request.BetTypeGroupIds,
                        bettingLineTemplateId, request.Type).ToList();

                    int eventid;
                    if (int.TryParse(request.EventIds, out eventid))
                    {
                        result = GenerateOddsbyBettypeGroup(events, odds);
                    }
                    else
                    {
                        result = GenerateOddsByBettype(events, odds);
                    }
                }
            }
            manager.SetCache(result, 5 * 60);

            return result;
        }

        internal Account Login (HttpContext context, LoginRequest request)
        {
            Account result = null;

            return result;
        }

        private static Sport GenerateOddsByBettype(List<webAPI_Client_GetEventInfoResult> events, List<webAPI_Client_GetEventOddsResult> odds)
        {
            Sport result = null;

            foreach (var odd in odds)
            {
                var e = events.FirstOrDefault(ev => ev.EventID == odd.EventId);
                if (e == null)
                    continue;

                if (result == null)
                {
                    result = new Sport(e.SportId, e.SportName);
                    result.Countries = new List<Country>();
                }

                if (!result.Countries.Select(y => y.Code).Contains(e.CountryCode))
                {
                    result.Countries.Add(new Country(e.CountryCode, e.CountryName));
                    result.Countries.Last().Leagues = new List<League>();
                }
                var country = result.Countries.FirstOrDefault(x => x.Code == e.CountryCode);
                if (!country.Leagues.Select(l => l.Id).Contains(e.LeagueId))
                {
                    country.Leagues.Add(new League(e.LeagueName, e.LeagueId));
                    country.Leagues.Last().Events = new List<Event>();
                }
                var league = country.Leagues.FirstOrDefault(x => x.Id == e.LeagueId);
                if (!league.Events.Select(ev => ev.Id).Contains(e.EventID))
                {
                    league.Events.Add(new Event()
                    {
                        Id = e.EventID,
                        Code = Convert.ToInt32(e.EventCode),
                        Name = e.EventName,
                        Type = e.EventType,
                        Date = e.EventDate,
                        Interval = e.Interval,
                        HomeId = e.HomeId,
                        GuestId = e.GuestId,
                        HomeName = e.HomeName,
                        GuestName = e.GuestName,
                        BetTypes = new List<BetType>()
                    });
                }
                var lEvent = league.Events.FirstOrDefault(x => x.Id == e.EventID);
                if (!lEvent.BetTypes.Select(bt => bt.Id).Contains(odd.BetTypeId))
                {
                    lEvent.BetTypes.Add(new BetType(odd.BetTypeId, ""));
                    lEvent.BetTypes.Last().Odds = new List<Odd>();
                }
                var bettype = lEvent.BetTypes.FirstOrDefault(x => x.Id == odd.BetTypeId);
                if (bettype != null)
                {
                    bettype.Odds.Add(new Odd()
                    {
                        Id = odd.BetTypeId,
                        OddFactor = odd.OddFactor,
                        OddPoint = odd.OddPoint,
                        TeamId = odd.TeamId,
                        OddTypeId = odd.OddTypeId
                    });
                }
            };

            return result;
        }

        private static Sport GenerateOddsbyBettypeGroup(List<webAPI_Client_GetEventInfoResult> events, List<webAPI_Client_GetEventOddsResult> odds)
        {
            Sport result = null;

            foreach (var odd in odds)
            {
                var e = events.FirstOrDefault(ev => ev.EventID == odd.EventId);
                if (e == null)
                    continue;

                if (result == null)
                {
                    result = new Sport(e.SportId, e.SportName);
                    result.Countries = new List<Country>();
                }

                if (!result.Countries.Select(y => y.Code).Contains(e.CountryCode))
                {
                    result.Countries.Add(new Country(e.CountryCode, e.CountryName));
                    result.Countries.Last().Leagues = new List<League>();
                }
                var country = result.Countries.FirstOrDefault(x => x.Code == e.CountryCode);
                if (!country.Leagues.Select(l => l.Id).Contains(e.LeagueId))
                {
                    country.Leagues.Add(new League(e.LeagueName, e.LeagueId));
                    country.Leagues.Last().Events = new List<Event>();
                }
                var league = country.Leagues.FirstOrDefault(x => x.Id == e.LeagueId);
                if (!league.Events.Select(ev => ev.Id).Contains(e.EventID))
                {
                    league.Events.Add(new Event()
                    {
                        Id = e.EventID,
                        Code = Convert.ToInt32(e.EventCode),
                        Name = e.EventName,
                        Type = e.EventType,
                        Date = e.EventDate,
                        Interval = e.Interval,
                        HomeId = e.HomeId,
                        GuestId = e.GuestId,
                        HomeName = e.HomeName,
                        GuestName = e.GuestName,
                        BetTypes = new List<BetType>(),
                        BetTypeGroups = new List<BetTypeGroup>()
                    });
                }
                var lEvent = league.Events.FirstOrDefault(x => x.Id == e.EventID);
                if (!lEvent.BetTypeGroups.Select(btg => btg.Id).Contains(odd.BetGroupId))
                {
                    lEvent.BetTypeGroups.Add(new BetTypeGroup(odd.BetGroupId, "", 0));
                    lEvent.BetTypeGroups.Last().BetTypes = new List<BetType>();
                }
                var bettypegroup = lEvent.BetTypeGroups.FirstOrDefault(x => x.Id == odd.BetGroupId);
                if (!bettypegroup.BetTypes.Select(bt => bt.Id).Contains(odd.BetTypeId))
                {
                    bettypegroup.BetTypes.Add(new BetType(odd.BetTypeId, ""));
                    bettypegroup.BetTypes.Last().Odds = new List<Odd>();
                }
                var bettype = bettypegroup.BetTypes.FirstOrDefault(x => x.Id == odd.BetTypeId);
                if (bettype != null)
                {
                    bettype.Odds.Add(new Odd()
                    {
                        Id = odd.BetTypeId,
                        OddFactor = odd.OddFactor,
                        OddPoint = odd.OddPoint,
                        TeamId = odd.TeamId,
                        OddTypeId = odd.OddTypeId
                    });
                }
            };

            return result;
        }
    }
}