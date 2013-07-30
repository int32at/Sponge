using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sponge.Models;
using Sponge.Utilities;
using Microsoft.SharePoint;

namespace Sponge.Tests
{
    [TestClass]
    public class SPManagerTests
    {
        [TestMethod]
        public void CreateSpongeWebInCentralAdmin()
        {
            var ca = SPWebManager.GetCentralAdminWeb();
            Assert.IsNotNull(ca);

            using (var mgr = new SPManager(ca.Site))
            {
                Assert.AreEqual(false, mgr.Webs.Exists("Sponge"));
                var sponge = mgr.Webs.Create("Sponge Framework", "", "Sponge", "STS#1");
                Assert.IsNotNull(sponge);
            }
        }

        [TestMethod]
        public void RemoveSpongeWebInCentralAdmin()
        {
            var ca = SPWebManager.GetCentralAdminWeb();
            Assert.IsNotNull(ca);

            using (var mgr = new SPManager(ca.Site))
            {
                mgr.Webs.Delete("Sponge");
                Assert.AreEqual(false, mgr.Webs.Exists("Sponge"));
            }
        }

        [TestMethod]
        public void CreateTestList()
        {
            var ca = SPWebManager.GetCentralAdminWeb();
            Assert.IsNotNull(ca);

            using (var mgr = new SPManager(ca.Site))
            {
                mgr.Lists.Create("LogAppenders", "", SPListTemplateType.GenericList);
                Assert.AreEqual(true, mgr.Lists.Exists("LogAppenders"));
            }
        }

        [TestMethod]
        public void DeleteTestList()
        {
            var ca = SPWebManager.GetCentralAdminWeb();
            Assert.IsNotNull(ca);

            using (var mgr = new SPManager(ca.Site))
            {
                mgr.Lists.Delete("LogAppenders");
                Assert.AreEqual(false, mgr.Lists.Exists("LogAppenders"));
            }
        }
    }
}