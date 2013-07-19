using System;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sponge.Common.Utilities;

namespace Sponge.Tests
{
    [TestClass]
    public class FeatureTests
    {
        [TestMethod]
        public void GetCentralAdminRootWeb()
        {
            var web = Utils.GetCentralAdminRootWeb();
            Assert.IsNotNull(web);
        }
    }
}
