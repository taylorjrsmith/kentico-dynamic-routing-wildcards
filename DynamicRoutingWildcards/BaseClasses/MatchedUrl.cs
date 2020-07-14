using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.Kentico.Wildcards.BaseClasses
{
    public class MatchedUrl
    {
        public UrlBreakdown UrlBreakdown { get; }
        public Dictionary<string, string> QueryStringPairs { get; }
        public bool HasMatch { get; }

        public MatchedUrl(UrlBreakdown urlBreakdown, Dictionary<string, string> queryStringPairs)
        {
            UrlBreakdown = urlBreakdown;
            QueryStringPairs = queryStringPairs;
            HasMatch = true;
        }

        private MatchedUrl()
        {
            HasMatch = false;
        }

        public static MatchedUrl FailedMatch()
        {
            return new MatchedUrl();
        }
    }
}
