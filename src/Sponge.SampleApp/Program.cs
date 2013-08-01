using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sponge.Client.Logging;

namespace Sponge.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = ClientLogManager.GetOffline("NLog.config", "Sponge.SampleApp.Program");

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
