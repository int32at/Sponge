using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System;
using System.Web.UI.WebControls;

namespace Sponge.WebParts
{
    [ToolboxItemAttribute(false)]
    public class HostingWebPart : WebPart
    {
        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Relative .ascx Path"),
        WebDescription("The relative path to your .ascx file. Example: ~/_CONTROLTEMPLATES/Sponge/SampleControl.ascx")]
        public string AscxPath { get; set; }

        protected override void CreateChildControls()
        {
            if (!string.IsNullOrEmpty(AscxPath))
            {
                try
                {
                    var control = Page.LoadControl(AscxPath);
                    Controls.Add(control);
                }
                catch(Exception ex)
                {
                    var lbl = new Label {Text = ex.Message};
                    Controls.Add(lbl);
                }
            }
        }
    }
}
