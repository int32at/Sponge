using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sponge.Client.Configuration;
using Sponge.Configuration;
using Sponge.Models;
using Sponge.Utilities;

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
        public void GetAllValuesServerSide()
        {
            var app = "Sponge Unit Test";

            if (!ConfigurationManager.ApplicationExists(app))
                ConfigurationManager.CreateApplication(app);

            ConfigurationManager.Set(app, "Test", DateTime.Now);
            var actual = ConfigurationManager.GetAll(app);

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GetAllValuesClientSide()
        {
            var app = "Sponge Unit Test";

            using (var cfg = new ClientConfigurationManager("http://demo"))
            {
                cfg.Set(app, "Test1", DateTime.Now.ToString());
                var actual = cfg.GetAll(app);

                Assert.IsNotNull(actual);
            }
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
                Assert.AreEqual(true, cfg.ApplicationExists(app));
            }
        }

        [TestMethod]
        public void CreateConfigEntryClientSide()
        {
            var app = "Sponge App";
            var actual = "a";
            using (var cfg = new ClientConfigurationManager("http://demo"))
            {
                cfg.Set(app, "MyKey", "a");
                var expected = cfg.Get(app, "MyKey");
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void CreateConfigEntryServerSide()
        {
            var app = "Sponge App";
            var actual = "actual";

            ConfigurationManager.Set(app, "MyKey", actual);
            var expected = ConfigurationManager.Get(app, "MyKey");
            Assert.AreEqual(expected, actual);
        }
    }
}
