using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.Kentico.Wildcards.BaseClasses
{
    public class UrlBreakdown
    {
        public string Url { get; set; }
        public string UrlWithoutWildcards { get; set; }
        public KenticoData KenticoData { get; set; }

        public UrlBreakdown(string url, string urlWithoutWildcards, KenticoData kenticoData)
        {
            Url = url;
            UrlWithoutWildcards = urlWithoutWildcards;
            KenticoData = kenticoData;

        }
    }

    public class KenticoData
    {
        public int NodeId { get; set; }
        public string ClassName { get; set; }

        public KenticoData(int nodeId, string className)
        {
            NodeId = nodeId;
            ClassName = className;
        }
    }
}
