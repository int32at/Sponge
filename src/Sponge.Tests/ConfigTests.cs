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
        public void CreateAppAndAdd10TestItemsAndChangeValues()
        {
            var appName = "Sponge Unit Tests";
            ConfigurationManager.CreateApplication(appName);

            Assert.AreEqual(true, ConfigurationManager.ApplicationExists(appName));

            foreach (var i in Enumerable.Range(0, 10))
                ConfigurationManager.Set(appName, "Sample" + i, i * i);

            Assert.AreNotEqual(0, ConfigurationManager.GetAll(appName));
            Assert.AreNotEqual(0, ConfigurationManager.Get<int>(appName, "Sample9"));

            ConfigurationManager.Set(appName, "Sample9", "ASDF");
            Assert.AreEqual("ASDF", ConfigurationManager.Get<string>(appName, "Sample9"));
        }
    }
}
