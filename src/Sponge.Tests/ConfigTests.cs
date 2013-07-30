using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sponge.Common.Configuration;
using Sponge.Common.Models;
using Sponge.Common.Utilities;

namespace Sponge.Tests
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void GetSpongeWeb()
        {
            var sponge = Utils.GetSpongeWeb();
            Assert.IsNotNull(sponge);
        }

        [TestMethod]
        public void GetSpongeUrl()
        {
            var spongeUrl = Utils.GetSpongeUrl();
            Assert.IsNotNull(spongeUrl);
        }

        [TestMethod]
        public void GetAllValues()
        {
            var app = "Sponge Unit Test";

            var actual = ConfigurationManager.GetAll(app);

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void CreateAppServerSide()
        {
            var app = "Sponge Unit Test";
            ConfigurationManager.CreateApplication(app);
            Assert.AreEqual(true, ConfigurationManager.ApplicationExists(app));
        }

        [TestMethod]
        public void CreateAppClientSide()
        {
            var app = "Sponge App";
            using (var cfg = new ClientConfigurationManager("http://demo"))
            {
                cfg.CreateApplication(app);
                Assert.AreEqual("true", cfg.ApplicationExists(app));
            }
        }
    }
}
