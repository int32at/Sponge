using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Sponge.WebParts
{
    [ToolboxItemAttribute(false)]
    public partial class WeatherWebPart : WebPart
    {
        public WeatherWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
