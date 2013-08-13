using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Sponge.Pages
{
    public partial class EventHandlerManager : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            LoadLists();
        }

        private void LoadLists()
        {
            var lists = SPContext.Current.Web.Lists.Cast<SPList>().ToList();
            ddlLists.DataMember = "Title";
            ddlLists.DataSource = lists;
            ddlLists.DataBind();
        }

        public void CancelButton_Click(object sender, EventArgs e)
        {
        }

        public void RegisterButton_Click(object sender, EventArgs e)
        {
        }


        public void ListIndex_Changed(object sender, EventArgs e)
        {
            lbExistingHandlers.Items.Add(sender.ToString());
        }
    }
}