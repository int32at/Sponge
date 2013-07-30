using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sponge.Logging;
using Sponge.Client.Logging;

namespace Sponge.Tests
{
    [TestClass]
    public class LoggingTests
    {
        private const string _offlineCfg = @"..\..\offline.config";
        [TestMethod]
        public void LogMsgServerSideOffline()
        {
            var log = LogManager.GetOffline(_offlineCfg, this.GetType());
            Assert.IsNotNull(log);
            log.Debug("This is {0} debug msg", "my");
        }

        [TestMethod]
        public void LogMsgClientSideOffline()
        {
            var log = ClientLogManager.GetOffline(_offlineCfg);
            log.Debug("This is {0} debug msg", "my");
        }

        [TestMethod]
        public void LogMsgServerSideOnline()
        {
            var log = LogManager.GetOnline("SpongeUnitTest");
            log.Debug("This is {0} debug msg", "my");
        }


        [TestMethod]
        public void LogMsgClientSideOnline()
        {
            var log = ClientLogManager.GetOnline("http://demo", "SpongeUnitTest");
            log.Debug("This is {0} debug msg", "my");
        }
    }
}
