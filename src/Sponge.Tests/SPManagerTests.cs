using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sponge.Common.Models;
using Sponge.Common.Utilities;

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
    }
}