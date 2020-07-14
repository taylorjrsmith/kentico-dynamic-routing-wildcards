using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DynamicRouting.Kentico.Wildcards.BaseClasses
{
    public class UrlByLengthCollection
    {
        public static Regex PathMatcher = new Regex("^[/]?(?<Path>[^/]{1,}[/]?)*$");

        public Dictionary<int, UrlByWildcardCollection> Urls { get; }

        public UrlByLengthCollection()
        {
            Urls = new Dictionary<int, UrlByWildcardCollection>();
        }

        public void AddUrls(Dictionary<string, KenticoData> urls)
        {
            foreach (var url in urls)
            {
                var urlLower = url.Key.ToLower().Trim('/');
                var pathCount = PathMatcher.Match(urlLower).Groups["Path"].Captures.Count;
                if (Urls.ContainsKey(pathCount))
                {
                    Urls[pathCount].AddUrl(urlLower, url.Value);
                }
                else
                {
                    Urls.Add(pathCount, new UrlByWildcardCollection(urlLower, url.Value));
                }
            }
        }

        public MatchedUrl FindBaseUrl(string urlToMatch)
        {
            urlToMatch = urlToMatch.ToLower().Trim('/');
            var pathCount = PathMatcher.Match(urlToMatch).Groups["Path"].Captures.Count;
            if (!Urls.ContainsKey(pathCount))
            {
                return MatchedUrl.FailedMatch();
            }

            return Urls[pathCount].FindBaseUrl(urlToMatch);
        }
    }
}
