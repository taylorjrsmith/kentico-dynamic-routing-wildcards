using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DynamicRouting.Kentico.Wildcards.BaseClasses
{
    public class UrlByWildcardCollection
    {
        private static Regex WildcardFinder = new Regex("{(?<ParamName>[a-zA-Z0-9À-ž]*)}");

        public UrlByWildcardCollection()
        {
            Urls = new Dictionary<int, List<UrlBreakdown>>();
        }

        public UrlByWildcardCollection(string url, KenticoData kenticoData)
        {
            Urls = new Dictionary<int, List<UrlBreakdown>>();
            AddUrl(url, kenticoData);
        }

        public Dictionary<int, List<UrlBreakdown>> Urls { get; }

        public void AddUrl(string urlPattern, KenticoData kenticoData)
        {
            var matchCount = WildcardFinder.Matches(urlPattern).Count;

            var urlBreakdown = new UrlBreakdown(urlPattern, GetUrlWithoutWildcards(urlPattern), kenticoData);

            if (Urls.ContainsKey(matchCount))
                Urls[matchCount].Add(urlBreakdown);
            else
            {
                Urls.Add(matchCount, new List<UrlBreakdown>
                {
                    urlBreakdown
                });
            }
        }

        private string GetUrlWithoutWildcards(string url)
        {
            var matches = WildcardFinder.Matches(url);
            if (matches.Count == 0)
                return url;

            url = matches.Cast<Match>().Aggregate(url, (current, match) => current.Replace(match.Value, ""));

            return url.Trim('/');
        }

        public MatchedUrl FindBaseUrl(string urlToMatch)
        {
            var maxWildcardCount = Urls.Keys.Max();

            for (var x = 0; x <= maxWildcardCount; x++)
            {
                if (!Urls.ContainsKey(x))
                    continue;
                var cutUrlToMatch = CutPathFromUrl(urlToMatch, x);
                var matchedUrl = Urls[x].FirstOrDefault(url => url.UrlWithoutWildcards == cutUrlToMatch);
                if (matchedUrl != null)
                    return new MatchedUrl(matchedUrl, GetQueryStringPairs(matchedUrl, urlToMatch));
            }

            return MatchedUrl.FailedMatch();
        }

        private Dictionary<string, string> GetQueryStringPairs(UrlBreakdown matchedUrl, string urlToMatch)
        {
            var wildcardMatches = WildcardFinder.Matches(matchedUrl.Url).Cast<Match>().Reverse().ToList();
            var pathMatches = UrlByLengthCollection.PathMatcher.Match(urlToMatch).Groups["Path"].Captures.Cast<Capture>().Reverse().ToList();

            var queryCollection = new Dictionary<string, string>();

            for (var x = 0; x < wildcardMatches.Count; x++)
                queryCollection.Add(wildcardMatches[x].Value.Trim('}', '{'), pathMatches[x].Value.Trim('/'));

            return queryCollection;
        }

        private string CutPathFromUrl(string urlToMatch, int pathsToCut)
        {
            if (pathsToCut == 0)
                return urlToMatch;
            var match = UrlByLengthCollection.PathMatcher.Match(urlToMatch);

            var captures = match.Groups["Path"].Captures.Cast<Capture>().Reverse().ToList();

            for (var x = 0; x < pathsToCut; x++)
            {
                var captureValue = captures[x].Value.Trim('/');

                if (string.IsNullOrEmpty(captureValue))
                {
                    pathsToCut++; //Nothing was cut, probably due to an odd group match. So just increase to get the first actual path.
                    continue;
                }

                urlToMatch = urlToMatch.Replace(captureValue, "");
            }

            return urlToMatch.Trim('/');
        }
    }
}
