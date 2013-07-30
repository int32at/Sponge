using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Sponge.Common.Extensions;

namespace Sponge.Pages
{
    public partial class SitePropertyManager : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            Redirect();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Redirect();
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            var property = this.txtKey.Text.Trim();
            var value = this.txtValue.Text.Trim();

            if (!string.IsNullOrEmpty(property) &&
                !string.IsNullOrEmpty(value))
            {
                SPContext.Current.Site.RootWeb.SetPropertyString(property, value);

                Response.Redirect(Request.Url.ToString());

                //RefreshPropertyTable();
            }
        }

        protected void ClearErrorButton_Click(object sender, EventArgs e)
        {
            //SPContext.Current.Site.RootWeb.TryRemovePropertyString(C.PROPERTY_PREFIX + C.PROPERTY_LAST_KNOWN_ERROR);
            //LoadLastKnownError();
        }

        private void Refresh()
        {
            const string cellPadding = "3px";
            const string propertyEditClickTemplate = "onclick='javascript:SetEditProperty(\"{0}\", \"{1}\")'";

            // clear table except header row
            while (propertyTable.Rows.Count > 1)
                propertyTable.Rows.RemoveAt(1);

            // get properties from site collection's property bag
            var properties = SPContext.Current.Site.RootWeb.AllProperties;

            // filter all prefixed sponge properties
            var sortedProperties = new SortedDictionary<string, string>();
            foreach (DictionaryEntry property in properties)
            {
                sortedProperties.Add(property.Key.ToString(), property.Value.ToString());
            }

            // build up table using the filtered, sorted property list
            var idx = 1;
            foreach (KeyValuePair<string, string> property in sortedProperties)
            {
                //var propertyKey = property.Key.ToString().Remove(0, C.PROPERTY_PREFIX.Length);
                var propertyKey = property.Key.ToString();
                var propertyValue = property.Value.ToString();
                var propertyEditClickHandler = string.Format(propertyEditClickTemplate, propertyKey, propertyValue);

                var removeButton = new ImageButton() { ImageUrl = "/_layouts/images/sponge/delete.png" };
                removeButton.Command += new CommandEventHandler(RemovePropertyButton_Click);
                removeButton.CommandName = "Open";
                removeButton.CommandArgument = property.Key.ToString();
                removeButton.ToolTip = "Remove this property";
                removeButton.ID = "RemovePropertyButton" + idx.ToString();
                // removeButton.OnClientClick = string.Format("__doPostBack($('[id$=\"{0}\"]')[0].id, '')", removeButton.ID);

                var propertyCell = new TableCell();
                propertyCell.Text = string.Format("<div style='font-weight:bold;margin:{0};cursor:pointer' {1}>{2}</div>", cellPadding, propertyEditClickHandler, propertyKey);
                propertyCell.ToolTip = "Edit this property";
                propertyCell.BorderWidth = 1;
                propertyCell.BorderStyle = BorderStyle.Dashed;
                propertyCell.BorderColor = Color.FromArgb(219, 221, 222); // =#dbddde

                var valueCell = new TableCell();
                valueCell.Text = string.Format("<div style='margin:{0}'>{1}</div>", cellPadding, propertyValue);
                valueCell.BorderWidth = 1;
                valueCell.BorderStyle = BorderStyle.Dashed;
                valueCell.BorderColor = Color.FromArgb(219, 221, 222); // =#dbddde

                var buttonCell = new TableCell();
                buttonCell.Controls.Add(removeButton);
                buttonCell.BorderWidth = 1;
                buttonCell.BorderStyle = BorderStyle.Dashed;
                buttonCell.BorderColor = Color.FromArgb(219, 221, 222); // =#dbddde

                var row = new TableRow();
                row.Cells.Add(propertyCell);
                row.Cells.Add(valueCell);
                row.Cells.Add(buttonCell);
                propertyTable.Rows.Add(row);

                idx++;
            }
        }

        protected void RemovePropertyButton_Click(object sender, CommandEventArgs e)
        {
            var button = sender as ImageButton;
            var cell = button.Parent as TableCell;
            var row = cell.Parent as TableRow;
            var property = StripHtml(row.Cells[0].Text);

            SPContext.Current.Site.RootWeb.TryRemovePropertyString(property);

            Response.Redirect(Request.Url.ToString());
        }

        private string StripHtml(string htmlText)
        {
            var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return HttpUtility.HtmlDecode(reg.Replace(htmlText, ""));
        }

        private void Redirect()
        {
            var source = Request.QueryString.Get("Source");

            if (!string.IsNullOrEmpty(source))
                this.Response.Redirect(source);
        }
    }
}
