using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Sponge.Server.Components.CorrelationViewer
{
    public class CorrelationQuery
    {
        List<string[]> _results;
        private string _tempFilePath;
        private string _mergeCmd;

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Id { get; set; }
        public string SavePath { get { return _tempFilePath; } }
        public string MergeCommand { get { return _mergeCmd; } }
        public List<string[]> Result { get { return _results; } }

        public CorrelationQuery(DateTime start, DateTime end, string correlationId)
        {
            Start = start;
            End = end;
            Id = correlationId;
            _results = new List<string[]>();
        }

        public void Query()
        {
            Runspace runspace = null;

            try
            {
                _tempFilePath = string.Format("{0}{1}.log", Path.GetTempPath(), Guid.NewGuid().ToString());
                _mergeCmd = string.Format("Merge-SPLogFile -OverWrite -Path '{0}' -StartTime '{1}' -EndTime '{2}' -Correlation '{3}'",
                    _tempFilePath, Start, End, Id);

                var iss = InitialSessionState.CreateDefault();
                PSSnapInException warning;

                iss.ImportPSSnapIn("Microsoft.SharePoint.PowerShell", out warning);

                runspace = RunspaceFactory.CreateRunspace(iss);
                runspace.Open();

                var pipe = runspace.CreatePipeline();
                pipe.Commands.AddScript(_mergeCmd);

                try
                {
                    pipe.Invoke();
                }
                catch (Exception ex)
                {
                    File.Delete(_tempFilePath);
                    throw ex;
                }

                if (File.Exists(_tempFilePath))
                {
                    using (var reader = new StreamReader(_tempFilePath))
                    {
                        string inputLine = "";

                        string[] values = null;
                        while ((inputLine = reader.ReadLine()) != null)
                        {
                            values = inputLine.Split('\t');
                            _results.Add(values);
                        }
                    }
                }
                else
                {
                    throw new FileNotFoundException("No results were found.");
                }
            }
            finally
            {
                if (runspace != null)
                    runspace.Dispose();

                if (File.Exists(_tempFilePath))
                    File.Delete(_tempFilePath);
            }
        }

        public static List<CorrelationId> GetResult(List<string[]> data)
        {
            var list = new List<CorrelationId>();

            if (data.Count <= 0)
                return list;

            var count = 0;

            foreach (var result in data)
            {
                if (count != 0)
                {
                    list.Add(new CorrelationId
                    {
                        Time = result[0],
                        Process = result[1],
                        Area = result[2],
                        Category = result[3],
                        Event = result[4],
                        Level = result[5],
                        Message = result[6]
                    });
                }
            }

            return list;
        }
    }
}
