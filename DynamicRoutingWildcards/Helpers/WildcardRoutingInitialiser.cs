using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using DynamicRouting.Kentico.Wildcards.BaseClasses;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DynamicRouting.Kentico.Wildcards.Helpers
{
    public static class WildcardRoutingInitialiser
    {

        public static void AddWilcards()
        {
            DynamicRoutingEvents.GetPage.After += OverrideGetPageEvent;
            DynamicRoutingEvents.RequestRouting.After += OverrideRequestPageEvent;
        }
        public static string GetUrlCacheName(string relativeUrl) => relativeUrl.Replace("/", "_").ToUpper();


        public static void OverrideRequestPageEvent(object sender, RequestRoutingEventArgs args)
        {
            var relativeUrl = (string)HttpContext.Current.Items["CurrentRelUrl"];
            HttpContext.Current.Items.Remove("CurrentRelUrl");
            var cultureCode = (string)HttpContext.Current.Items["CultureCode"];
            HttpContext.Current.Items.Remove("CultureCode");
            var urlMatchup = GetUrlMatch(relativeUrl, cultureCode);
            if (urlMatchup != null)
            {
                foreach (var item in urlMatchup.QueryStringPairs)
                {
                    args.CurrentRequestContext.RouteData.Values.Add(item.Key, item.Value);
                    //TODO: add these args to the ViewBag too for purposes of non controller page directs
                }
            }

        }

        private static MatchedUrl GetUrlMatch(string relativeUrl, string cultureCode, DataTable possibleUrls = null)
        {
            var PossibleUrlPatterns = possibleUrls ?? GetPossibleUrls(relativeUrl, cultureCode);
            var urlMatchup = new UrlByWildcardCollection();
            if (PossibleUrlPatterns.Rows.Count > 0)
            {
                foreach (DataRow dr in PossibleUrlPatterns.Rows)
                {
                    int documentID = ValidationHelper.GetInteger(dr["UrlSlugNodeId"], 0);
                    string urlPattern = ValidationHelper.GetString(dr["possibleUrl"], "");
                    string className = ValidationHelper.GetString(dr["ClassName"], "");
                    urlMatchup.AddUrl(urlPattern, new KenticoData(documentID, className));
                }

                var matchedUrl = urlMatchup.FindBaseUrl(relativeUrl);
                if (matchedUrl.UrlBreakdown == null || !matchedUrl.HasMatch)
                {
                    return null;
                }
                return matchedUrl;
            }
            return null;
        }

        public static DataTable GetPossibleUrls(string relativeUrl, string cultureCode)
        {
            var routeKey = $"possiblePatterns-{GetUrlCacheName(relativeUrl)}";
            var cachedItem = (DataTable)HttpContext.Current.Cache[routeKey];
            if (cachedItem == null)
            {
                DataTable matches = ConnectionHelper.ExecuteQuery("DynamicRouting.WildcardUrl.GetPossibleUrls",
                new QueryDataParameters() {
                { "@RelativeUrl", relativeUrl },
                { "@CultureCode", cultureCode }
                }).Tables[0];

                HttpContext.Current.Cache.Add(routeKey, matches, null, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                return matches;
            }
            return (DataTable)cachedItem;
        }

        private static void UpdateCacheItem<T>(string key, T value)
        {
            if(HttpContext.Current.Items[key] == null)
                HttpContext.Current.Items.Add(key, value);
        }

        public static void OverrideGetPageEvent(object sender, GetPageEventArgs args)
        {
            string cultureCode = args.Culture;

            //Occasionally the culture code may come out as full text such as English - United Kingdom, using this info we can fetch the culture code
            if (args.Culture.Length > 5)
            {
                cultureCode = CultureInfoProvider.GetCultures().Where(a => a.CultureName == args.Culture).First().CultureCode;
            }
            UpdateCacheItem<string>("CurrentRelUrl", args.RelativeUrl);
            UpdateCacheItem<string>("CultureCode", cultureCode);
            if (args.FoundPage == null)
            {
                try
                {
                    args.FoundPage = CacheHelper.Cache<TreeNode>(cs =>
                    {
                        //TODO: ADD Culture
                        DataTable PossibleUrlPatterns = GetPossibleUrls(args.RelativeUrl, cultureCode);
                        if (PossibleUrlPatterns.Rows.Count > 0)
                        {
                            var matchedUrl = GetUrlMatch(args.RelativeUrl, cultureCode, PossibleUrlPatterns);
                            if (matchedUrl == null || !matchedUrl.HasMatch)
                            {
                                return null;
                            }
                            DocumentQuery Query = DocumentHelper.GetDocuments(matchedUrl.UrlBreakdown.KenticoData.ClassName).WhereEquals("NodeId", matchedUrl.UrlBreakdown.KenticoData.NodeId).CombineWithAnyCulture();

                            if (args.PreviewEnabled)
                            {
                                Query.LatestVersion(true).Published(false);
                            }
                            else
                            {
                                Query.PublishedVersion(true);
                            }

                            TreeNode page = Query.FirstOrDefault();

                            if (cs.Cached)
                            {
                                if (page != null)
                                {
                                    cs.CacheDependency = CacheHelper.GetCacheDependency(new string[] { $"{WildcardUrlInfo.OBJECT_TYPE}|all", "documentid|" + page.DocumentID });
                                }
                                else
                                {
                                    cs.CacheDependency = CacheHelper.GetCacheDependency(new string[] { $"{WildcardUrlInfo.OBJECT_TYPE}dynamicrouting.wildcards|all" });
                                }

                            }
                            return page;

                        }
                        else
                        {
                            return null;
                        }
                    }, new CacheSettings(args.PreviewEnabled ? 0 : 1440, "DynamicRouting.GetPage", args.RelativeUrl, cultureCode, args.DefaultCulture, args.SiteName, args.PreviewEnabled, args.ColumnsVal));
                }
                catch (Exception ex)
                {
                    args.ExceptionOnLookup = ex;

                }
                if (args.FoundPage == null)
                    HttpContext.Current.Response.StatusCode = 404;
            }

        }

        public class UrlWildcardArg
        {
            public int NodeId { get; set; }
            public string UrlPattern { get; set; }
        }
    }
}
