using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Sponge.Common.Components.CorrelationViewer;

namespace Sponge.AdminPages.CorrelationViewer
{
    public partial class Get : LayoutsPageBase
    {
        DateTime today, startDate, endDate;
        List<string[]> result;

        protected void Page_Load(object sender, EventArgs e)
        {
            today = DateTime.Now;
            if (!IsPostBack)
            {
                date_StartDate.SelectedDate = today.AddHours(-1).AddMinutes(1);
                date_EndDate.SelectedDate = today.AddMinutes(1);
            }
            date_StartDate.MaxDate = date_EndDate.SelectedDate;
            date_EndDate.MaxDate = today;
            date_EndDate.MinDate = date_StartDate.SelectedDate;
        }

        protected void Btn_Ok_Click(object sender, EventArgs e)
        {
            startDate = date_StartDate.SelectedDate;
            endDate = date_EndDate.SelectedDate;
            if (startDate > endDate)
            {
                date_StartDate.ErrorMessage = "The start date, including the hour, should be lesser than the end date.";
                date_StartDate.IsValid = false;
                date_StartDate.SelectedDate = date_EndDate.SelectedDate.AddHours(-1);
            }
            else
            {
                lbl_Status.Text = "";
                using (SPLongOperation longOperation = new SPLongOperation(this.Page))
                {
                    longOperation.LeadingHTML = "Querying the farm";
                    longOperation.TrailingHTML = "Please wait while the farm's ULS logs are being queried.";
                    longOperation.Begin();

                    try
                    {
                        SPSecurity.RunWithElevatedPrivileges(
                            delegate()
                            {
                                var query = new CorrelationQuery(startDate, endDate, txt_CorrelationID.Text);
                                query.Query();
                                result = query.Result; 
                            });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("QUERY FAILED");
                    }
                    finally
                    {
                        string redirectURL = String.Format("{0}{1}", SPContext.Current.Web.Url, "/_admin/Sponge/CorrelationViewer/Results.aspx");
                        try
                        {
                            Session["sponge_corrViewer_result"] = result;
                            longOperation.End(redirectURL);
                        }
                        catch (System.Threading.ThreadAbortException) { }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
