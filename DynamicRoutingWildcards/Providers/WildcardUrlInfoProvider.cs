using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace DynamicRouting
{
    /// <summary>
    /// Class providing <see cref="WildcardUrlInfo"/> management.
    /// </summary>
    public partial class WildcardUrlInfoProvider : AbstractInfoProvider<WildcardUrlInfo, WildcardUrlInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="WildcardUrlInfoProvider"/>.
        /// </summary>
        public WildcardUrlInfoProvider()
            : base(WildcardUrlInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="WildcardUrlInfo"/> objects.
        /// </summary>
        public static ObjectQuery<WildcardUrlInfo> GetWildcardUrls()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="WildcardUrlInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="WildcardUrlInfo"/> ID.</param>
        public static WildcardUrlInfo GetWildcardUrlInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="WildcardUrlInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="WildcardUrlInfo"/> to be set.</param>
        public static void SetWildcardUrlInfo(WildcardUrlInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="WildcardUrlInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="WildcardUrlInfo"/> to be deleted.</param>
        public static void DeleteWildcardUrlInfo(WildcardUrlInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="WildcardUrlInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="WildcardUrlInfo"/> ID.</param>
        public static void DeleteWildcardUrlInfo(int id)
        {
            WildcardUrlInfo infoObj = GetWildcardUrlInfo(id);
            DeleteWildcardUrlInfo(infoObj);
        }
    }
}