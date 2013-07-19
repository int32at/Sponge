using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sponge.Common.Configuration;

namespace Sponge.Tests
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void SetValue()
        {
            var val = 3;

            //set the value 
            ConfigurationManager.Set("", "", val);

            //retreive it at casted string
            var set = ConfigurationManager.Get<int>("", "");

            Assert.AreEqual(val, set);
        }
    }
}
