using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Sponge.Components.CorrelationViewer;

namespace Sponge.AdminPages.CorrelationViewer
{
    public partial class Results : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var result = (List<string[]>)Session["sponge_corrViewer_result"];

                if (result == null)
                    throw new InvalidOperationException("Result cannot be null.");

                if(result.Count > 0)
                {
                    var data = CorrelationQuery.GetResult(result);
                    GenerateSPGridView(data);
                }
                else
                {
                    Label noResults = new Label();
                    noResults.Text = "No results were found for the given correlation id.";
                    ResultPanel.Controls.Add(noResults);
                }
            }
            catch (Exception ex)
            {
                lbl_Status.Text = ex.Message;
            }
        }

        private void GenerateSPGridView(List<CorrelationId> ids )
        {
            SPGridView oGrid = new SPGridView();
            oGrid.ID = "CorrelationQueryResultGrid";
            oGrid.DataSource = ids;
            oGrid.AutoGenerateColumns = false;
            oGrid.AlternatingRowStyle.CssClass = "ms-alternatingstrong";

            GenerateBoundField(oGrid, "Time");
            GenerateBoundField(oGrid, "Process");
            GenerateBoundField(oGrid, "Area");
            GenerateBoundField(oGrid, "Category");
            GenerateBoundField(oGrid, "Level");
            GenerateBoundField(oGrid, "EventID");
            GenerateBoundField(oGrid, "Message");

            ResultPanel.Controls.Add(oGrid);
            oGrid.DataBind();
        }

        private void GenerateBoundField(SPGridView oGrid, string columnName)
        {
            BoundField colTime = new BoundField();
            colTime.DataField = columnName;
            colTime.HeaderText = columnName;
            colTime.HeaderStyle.CssClass = "ms-vh2";
            colTime.ItemStyle.CssClass = "ms-vb2";
            oGrid.Columns.Add(colTime);
        }
    }
}
