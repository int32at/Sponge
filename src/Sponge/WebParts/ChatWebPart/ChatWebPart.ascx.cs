using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Sponge.Models;
using Sponge.Utilities;

namespace Sponge.WebParts
{
    [ToolboxItemAttribute(false)]
    public partial class ChatWebPart : WebPart
    {
        //private int _refreshInterval = 0;
        private int _rowLimit = 10;

        //[WebBrowsable(true),
        //Category("Configuration"),
        //Personalizable(PersonalizationScope.Shared),
        //WebDisplayName("Auto Refresh Interval"),
        //WebDescription("The Chat Web Part will automatically refresh in X milliseconds. Default = 0 (disabled)"),
        //DefaultValue(0)]
        //public int RefreshInterval { get { return _refreshInterval; } set { _refreshInterval = value; } }

        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Entry Limit"),
        WebDescription("The Chat Web Part will only display X items. Default = 10"),
        DefaultValue(10)]
        public int RowLimit { get { return _rowLimit; } set { _rowLimit = value; } }

        public int GetRowLimit()
        {
            return RowLimit;
        }

        //public int GetRefreshInterval()
        //{
        //    return RefreshInterval;
        //}

        public string GetSpongeChatListName()
        {
            return Constants.SPONGE_CHAT_LISTNAME;
        }

        public ChatWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                        if (!SPListManager.Exists(SPContext.Current.Web, Constants.SPONGE_CHAT_LISTNAME))
                        {
                            var guid = SPContext.Current.Web.Lists.Add(Constants.SPONGE_CHAT_LISTNAME, "Sponge Chat List", SPListTemplateType.GenericList);
                            var list = SPContext.Current.Web.Lists[guid];
                            list.Fields.Add("Message", SPFieldType.Note, true);

                            var view = list.DefaultView;
                            view.ViewFields.DeleteAll();
                            view.ViewFields.Add("Message");
                            view.Update();
                            list.Update();
                        }
                });
            }
            catch(Exception ex)
            {
                var lbl = new Label();
                lbl.Text = ex.ToString();
                this.Controls.Add(lbl);
            }
        }
    }
}
