using CMS;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Modules;
using DynamicRouting.Kentico.Wildcards.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: RegisterModule(typeof(DynamicRouteWildcardInitializationModule))]


namespace DynamicRouting.Kentico.Wildcards.Helpers
{

    public class DynamicRouteWildcardInitializationModule : Module
    {
        public DynamicRouteWildcardInitializationModule() : base("DynamicRouteWildcardInitialisationModule")
        {
            CMS.Modules.ModulePackagingEvents.Instance.BuildNuSpecManifest.After += BuildNuSpecManifest_After;
        }

        protected override void OnInit()
        {
            try
            {
                ConnectionHelper.ExecuteNonQuery("DynamicRouting.WildcardUrl.InitializeSQLEntities");
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("DynamicRouting.Wildcards", "ErrorRunningSQLEntities", ex, additionalMessage: "Could not run DynamicRouting.WildcardUrl.InitializeSQLEntities Query, this sets up foreign keys vital to operation.  Please ensure these queries exist.");
            }
        }

        private void BuildNuSpecManifest_After(object sender, BuildNuSpecManifestEventArgs e)
        {
            if (e.ResourceName.Equals("DynamicRouting.Kentico.Wildcards", System.StringComparison.InvariantCultureIgnoreCase))
            {
                e.Manifest.Metadata.Owners = "reallymoving Ltd";
                e.Manifest.Metadata.ProjectUrl = "https://github.com/reallymoving/kentico-dynamic-routing-wildcards";
                e.Manifest.Metadata.IconUrl = "http://www.kentico.com/favicon.ico";
                e.Manifest.Metadata.Copyright = "Copyright 2020 reallymoving Open Source";
                e.Manifest.Metadata.Title = "Wildcard extension for Dynamic Routing for Kentico v12 SP";
                e.Manifest.Metadata.ReleaseNotes = "Fixing multiple wildcards not correctly routing";
            }
        }
    }
}
