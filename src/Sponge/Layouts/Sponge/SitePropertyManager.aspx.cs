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

namespace Sponge.Layouts.Sponge
{
    public partial class SitePropertyManager : LayoutsPageBase
    {
        private string Referrer = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RefreshPropertyTable();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (this.Referrer == null)
            {
                this.Referrer = this.Request.QueryString["Source"];
            }
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            // finally redirect to the referrer
            if (!string.IsNullOrEmpty(this.Referrer))
                this.Response.Redirect(this.Referrer);
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {

            // go back to the referring page only
            if (!string.IsNullOrEmpty(this.Referrer))
                this.Response.Redirect(this.Referrer);
        }

        protected void AddPropertyButton_Click(object sender, EventArgs e)
        {
            var property = this.PropertyTextBox.Text.Trim();
            var value = this.ValueTextBox.Text.Trim();

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

        private void RefreshPropertyTable()
        {
            const string cellPadding = "3px";
            const string propertyEditClickTemplate = "onclick='javascript:SetEditProperty(\"{0}\", \"{1}\")'";

            // clear table except header row
            while (this.PropertyTable.Rows.Count > 1)
                this.PropertyTable.Rows.RemoveAt(1);

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
                this.PropertyTable.Rows.Add(row);

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

            //RefreshPropertyTable();
        }

        private string StripHtml(string htmlText)
        {
            var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return HttpUtility.HtmlDecode(reg.Replace(htmlText, ""));
        }
    }
}
