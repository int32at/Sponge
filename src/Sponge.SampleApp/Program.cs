using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Sponge.Client.Configuration;
using Sponge.Client.Logging;

namespace Sponge.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var spUrl = "http://demo";
            var app = "MyConsoleApp";

            var log = ClientLogManager.GetOnline(spUrl, app);
            //var log = ClientLogManager.GetOffline("NLog.config", app);

            using(var cfg = new ClientConfigurationManager(spUrl))
            {
                foreach(var entry in cfg.GetAll(app))
                {
                    log.Debug("{0} - {1}", entry.Key, entry.Value);
                }
            }

            //Dummy(log);
        }

        static void Dummy(Logger log)
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    var a = 0;
                    var x = 3 / a;
                }
                catch (Exception ex)
                {
                    log.FatalException("Unexpected Exception occured.", ex);
                }
            }
        }
    }
}
