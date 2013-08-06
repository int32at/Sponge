using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sponge.Client.Configuration;
using Sponge.Client.Logging;
using Sponge.Configuration;
using Sponge.Logging;

namespace Sponge.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //site collection
            var spUrl = "http://demo";
            var app = "MyConsoleApp";

            //if last parameter = true --> the config manager will look for the config/logging in the central admin
            //if parameter = false --> it will look in the http://demo/Sponge web to retreive the config/logging info
            //that means that the Sponge Logging & Configuration Feature has to be activated in this SiteColl 
            var cfg = ClientConfigurationManager.GetOnline(spUrl, app, false);

            //it is possible that the logging info is retrieved from central, but config info from relative
            var log = ClientLogManager.GetOnline(spUrl, app, true);

            //get all config entries within the config
            foreach(var entry in cfg.Items)
            {
                log.Debug("{0} - {1}", entry.Key, entry.Value);
            }

            //get a specific entry and cast it to int
            //int a = cfg.Get<int>("MyKey");
        }

        static void Dummy(NLog.Logger log)
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
