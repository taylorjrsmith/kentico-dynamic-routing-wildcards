using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using DynamicRouting;

[assembly: RegisterObjectType(typeof(WildcardUrlInfo), WildcardUrlInfo.OBJECT_TYPE)]

namespace DynamicRouting
{
    /// <summary>
    /// Data container class for <see cref="WildcardUrlInfo"/>.
    /// </summary>
    [Serializable]
    public partial class WildcardUrlInfo : AbstractInfo<WildcardUrlInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "dynamicrouting.wildcardurl";


        /// <summary>
        /// Type information.
        /// </summary>
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(WildcardUrlInfoProvider), OBJECT_TYPE, "DynamicRouting.WildcardUrl", "WildcardUrlID", "WildcardUrlLastModified", "WildcardUrlGuid", null, null, null, null, null, null)
        {
            ModuleName = "DynamicRouting.Kentico.Wildcards",
            TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("WildcardUrlNodeId", "cms.node", ObjectDependencyEnum.Required),
            },
            LogEvents = true,
            ImportExportSettings =
            {
    IsExportable = true,
    ObjectTreeLocations = new List<ObjectTreeLocation>()
    {
        // Adds the custom class into a new category in the Global objects section of the export tree
        new ObjectTreeLocation(GLOBAL, "DynamicRouting.Wildcards"),
    },
},
            SynchronizationSettings =
{
    LogSynchronization = SynchronizationTypeEnum.LogSynchronization,
    ObjectTreeLocations = new List<ObjectTreeLocation>()
    {
        // Adds the custom class into a new category in the Global objects section of the staging tree
        new ObjectTreeLocation(GLOBAL, "DynamicRouting.Wildcards")
    },
},
            ContinuousIntegrationSettings =
{
    Enabled = true
},
            ContainsMacros = false,
        };


        /// <summary>
        /// Wildcard url ID.
        /// </summary>
        [DatabaseField]
        public virtual int WildcardUrlID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("WildcardUrlID"), 0);
            }
            set
            {
                SetValue("WildcardUrlID", value);
            }
        }


        /// <summary>
        /// Wildcard url node id.
        /// </summary>
        [DatabaseField]
        public virtual int WildcardUrlNodeId
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("WildcardUrlNodeId"), 0);
            }
            set
            {
                SetValue("WildcardUrlNodeId", value);
            }
        }


        /// <summary>
        /// This is the pattern to use wildcards with dynamic routing.
        /// </summary>
        [DatabaseField]
        public virtual string WildcardUrlPattern
        {
            get
            {
                return ValidationHelper.GetString(GetValue("WildcardUrlPattern"), String.Empty);
            }
            set
            {
                SetValue("WildcardUrlPattern", value);
            }
        }


        /// <summary>
        /// What specific culture this applies to, if none will not apply a culture to the page selection.
        /// </summary>
        [DatabaseField]
        public virtual string WildcardUrlCulture
        {
            get
            {
                return ValidationHelper.GetString(GetValue("WildcardUrlCulture"), String.Empty);
            }
            set
            {
                SetValue("WildcardUrlCulture", value, String.Empty);
            }
        }


        /// <summary>
        /// Wildcard url guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid WildcardUrlGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("WildcardUrlGuid"), Guid.Empty);
            }
            set
            {
                SetValue("WildcardUrlGuid", value);
            }
        }


        /// <summary>
        /// Wildcard url last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime WildcardUrlLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("WildcardUrlLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("WildcardUrlLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            WildcardUrlInfoProvider.DeleteWildcardUrlInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            WildcardUrlInfoProvider.SetWildcardUrlInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected WildcardUrlInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="WildcardUrlInfo"/> class.
        /// </summary>
        public WildcardUrlInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="WildcardUrlInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public WildcardUrlInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}