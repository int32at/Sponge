using System;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sponge.Tests
{
    [TestClass]
    public class FeatureTests
    {
        [TestMethod]
        public void CreateWeb()
        {
            var adminWebApp = SPWebService.AdministrationService.WebApplications["SharePoint Central Administration v4"];
        }
    }
}
