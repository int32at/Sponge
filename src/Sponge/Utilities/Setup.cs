using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Sponge.Models;

namespace Sponge.Utilities
{
    public static class Setup
    {
        public static void Install(SPWeb web)
        {
            Update(web, false);
        }

        public static void Uninstall(SPWeb web)
        {
            Update(web, true);
        }

        private static void Update(SPWeb web, bool delete)
        {
            using (var mgr = new SPManager(web))
            {
                UpdateWeb(mgr, delete);
            }
        }
        
        private static void UpdateWeb(SPManager mgr, bool delete)
        {
            if (delete)
            {
                mgr.Webs.Delete(Constants.SpongeWebUrl);
            }
            else
            {
                if (!mgr.Webs.Exists(Constants.SpongeWebUrl))
                {
                    var sponge = mgr.Webs.Create(Constants.SpongeWebName, "",
                        Constants.SpongeWebUrl, Constants.SpongeWebTemplate);
                    sponge.SiteLogoUrl = Constants.SpongeWebImgurl;
                    sponge.Update();
                    mgr.ParentWeb = sponge;

                    UpdateLists(mgr);
                    CreateWebParts(mgr);
                    AddDefaultItems(mgr);
                }
            }
        }

        private static void UpdateLists(SPManager mgr)
        {
            CreateConfigApplications(mgr);
            CreateConfigItems(mgr);
            CreateLogTargets(mgr);
            CreateLogConfigs(mgr);

        }

        private static void CreateConfigItems(SPManager mgr)
        {
            var list = CreateList(mgr, Constants.SpongeListConfigitems);

            var targetList = list.ParentWeb.Lists[Constants.SpongeListConfigapplications];

            list.Fields.Add("Value", SPFieldType.Note, true);

            list.Fields.AddLookup("Application", targetList.ID, false);
            var lkp = (SPFieldLookup)list.Fields["Application"];
            lkp.LookupField = targetList.Fields["Title"].InternalName;
            lkp.Required = true;
            lkp.Update();

            if (list.ContentTypes.Count > 0)
            {
                var ct = list.ContentTypes[0];
                ct.FieldLinks.Reorder(new[] { "Application", "Key", "Value" });
            }

            var title = list.Fields["Title"];
            title.Title = "Key";
            title.Description = "Do not use blanks or special characters here!";
            title.Update();

            var view = list.DefaultView;
            const string @group = @" <GroupBy Collapse=""TRUE"" GroupLimit=""100""> <FieldRef Name=""Application"" Ascending=""True""/> </GroupBy>";
            view.Query = group;

            view.ViewFields.DeleteAll();
            view.ViewFields.Add("Application");
            view.ViewFields.Add("Key");
            view.ViewFields.Add("Value");
            view.Update();
        }

        private static void CreateConfigApplications(SPManager mgr)
        {
            CreateList(mgr, Constants.SpongeListConfigapplications);
        }

        private static void CreateLogTargets(SPManager mgr)
        {
            var list = CreateList(mgr, Constants.SpongeListLogtargets);

            list.Fields.Add("Xml", SPFieldType.Note, true);
            SPView view = list.DefaultView;
            view.ViewFields.DeleteAll();
            view.ViewFields.Add("Title");
            view.ViewFields.Add("Xml");
            view.Update();
            list.Update();
        }

        private static void CreateLogConfigs(SPManager mgr)
        {
            var list = CreateList(mgr, Constants.SpongeListLogconfigs);

            var targetList = list.ParentWeb.Lists[Constants.SpongeListLogtargets];

            list.Fields.AddLookup("Target", targetList.ID, false);
            var lkp = (SPFieldLookup)list.Fields["Target"];
            lkp.LookupField = targetList.Fields["Title"].InternalName;
            lkp.Required = true;
            lkp.Update();

            SPView view = list.DefaultView;
            //var group = @" <GroupBy Collapse=""TRUE"" GroupLimit=""100""> <FieldRef Name=""Target"" Ascending=""True""/> </GroupBy>";
            //view.Query = group;

            view.ViewFields.DeleteAll();
            view.ViewFields.Add("Title");
            view.ViewFields.Add("Target");
            view.Update();
            list.Update();
        }

        private static SPList CreateList(SPManager mgr, string listName)
        {
            var list = mgr.Lists.Create(listName, "", SPListTemplateType.GenericList);
            list.OnQuickLaunch = true;
            list.Update();

            return list;
        }

        private static void CreateWebParts(SPManager mgr)
        {
            var configItems = new XsltListViewWebPart {ListId = mgr.ParentWeb.Lists[Constants.SpongeListConfigitems].ID};
            var configApps = new XsltListViewWebPart {ListId = mgr.ParentWeb.Lists[Constants.SpongeListConfigapplications].ID};
            var logConfig = new XsltListViewWebPart {ListId = mgr.ParentWeb.Lists[Constants.SpongeListLogconfigs].ID};
            var logTargets = new XsltListViewWebPart {ListId = mgr.ParentWeb.Lists[Constants.SpongeListLogtargets].ID};

            AddWebPart(mgr.ParentWeb, "default.aspx", configApps, "left", 1);
            AddWebPart(mgr.ParentWeb, "default.aspx", configItems, "left", 2);
            AddWebPart(mgr.ParentWeb, "default.aspx", logConfig, "right", 1);
            AddWebPart(mgr.ParentWeb, "default.aspx", logTargets, "right", 2);
        }

        private static void AddWebPart(SPWeb web, string pageUrl, System.Web.UI.WebControls.WebParts.WebPart webPart, string zoneId, int zoneIndex)
        {
            var webPartManager = web.GetLimitedWebPartManager(pageUrl, PersonalizationScope.Shared);
            webPartManager.AddWebPart(webPart, zoneId, zoneIndex);
            webPartManager.SaveChanges(webPart);
            web.Update();
        }

        private static void AddDefaultItems(SPManager mgr)
        {
            var logTarget = mgr.ParentWeb.Lists[Constants.SpongeListLogtargets];

            #region file target 
            var newLogApp = logTarget.AddItem();
            newLogApp["Title"] = "Sponge File Logging";
            newLogApp["Xml"] = @"<?xml version='1.0' ?>
<nlog xmlns='http://www.nlog-project.org/schemas/NLog.xsd'
      xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
  <targets>
    <target name='file' xsi:type='File'
        layout='${longdate} ${logger} ${message}'
        fileName='${basedir}/logs/logfile.txt'
        keepFileOpen='false'
        encoding='iso-8859-2' />
  </targets>
  <rules>
    <logger name='*' minlevel='Debug' writeTo='file' />
  </rules>
</nlog>";
            newLogApp.SystemUpdate();

            #endregion

            #region uls target
            var uls = logTarget.AddItem();
            uls["Title"] = "Sponge ULS Logging";
            uls["Xml"] = @"<nlog xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
  <extensions>
    <add assembly='Sponge.Logging'/>
  </extensions>
  <targets>
    <target name='UlsLogger' type='UlsTarget'/>
  </targets>
  <rules>
    <logger name='*' minlevel='Debug' writeTo='UlsLogger' />
  </rules>
</nlog>";

            uls.SystemUpdate();

            #endregion

            #region ws target

//            var ws = logTarget.AddItem();
//            ws["Title"] = "Sponge Logging Web Service";
//            ws["Xml"] = @"<?xml version='1.0'?>
//<nlog autoReload='true' xmlns='http://www.nlog-project.org/schemas/NLog.xsd' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
//    <targets>
//        <target name='ws' xsi:type='WebService' namespace='http://Sponge.WebService.LoggingService' protocol='Soap11' methodName='Log' url='http://demo/_layouts/Sponge/LoggingService.asmx'>
//            <parameter name='n3' type='System.String' layout='${level}' />
//            <parameter name='n2' type='System.String' layout='${message}' />
//        </target>
//    </targets>
//    <rules>
//        <logger name='*' writeTo='ws' />
//    </rules>
//</nlog>";
//            ws.SystemUpdate();

            #endregion

            #region db target

            var db = logTarget.AddItem();
            db["Title"] = "Sponge Database Logging";
            db["Xml"] = @"<?xml version='1.0' encoding='utf-8' ?>
<nlog xmlns='http://www.nlog-project.org/schemas/NLog.xsd'
      xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' internalLogFile='c:\Nlog.log'>
  <targets>
    <target name='database' type='Database'>
      <connectionString>
        Data Source=demo;Initial Catalog=Sponge;User ID=spongeloguser;Password=pass@word1;
      </connectionString>
      <commandText>
        insert into Logs(log_date,log_level,log_logger, log_version, log_message,log_machine_name, log_user_name, log_call_site, log_thread, log_exception, log_stacktrace) values(@time_stamp, @level, @logger, @version, @message,@machinename, @user_name, @call_site, @threadid, @log_exception, @stacktrace);
      </commandText>
      <parameter name='@time_stamp' layout='${longdate}'/>
      <parameter name='@level' layout='${level}'/>
      <parameter name='@logger' layout='${logger}'/>
      <parameter name='@version' layout='${assembly-version}' />
      <parameter name='@message' layout='${message}'/>
      <parameter name='@machinename' layout='${machinename}'/>
      <parameter name='@user_name' layout='${windows-identity:domain=true}'/>
      <parameter name='@call_site' layout='${callsite:filename=true}'/>
      <parameter name='@threadid' layout='${threadid}'/>
      <parameter name='@log_exception' layout='${exception}'/>
      <parameter name='@stacktrace' layout='${stacktrace}'/>
    </target>
  </targets>
  <rules>
    <logger name='*' minlevel='Debug' appendTo='database'/>
  </rules>
</nlog>";
            db.SystemUpdate();
            #endregion

            var logItems = mgr.ParentWeb.Lists[Constants.SpongeListLogconfigs];

            var internalWs = logItems.AddItem();
            internalWs["Title"] = Constants.SpongeLoggerWsname;
            internalWs["Target"] = uls.ID;
            internalWs.SystemUpdate();
        }
    }
}
