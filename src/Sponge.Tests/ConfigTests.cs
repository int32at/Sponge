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
            var val1 = 3;
            var val2 = "abc";

            //set the value 
            ConfigurationManager.Set("myapp", "mykey1", val1);
            ConfigurationManager.Set("myapp", "mykey2", val2);

            //retreive it at casted string
            var set1 = ConfigurationManager.Get<int>("myapp", "mykey1");
            var set2 = ConfigurationManager.Get<string>("myapp", "mykey2");
            Assert.AreEqual(val1, set1);
            Assert.AreEqual(val2, set2);
        }
    }
}
