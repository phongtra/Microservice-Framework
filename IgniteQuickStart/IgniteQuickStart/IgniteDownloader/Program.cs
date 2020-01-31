using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Deployment;
using Apache.Ignite.Log4Net;
using Apache.Ignite.NLog;
using Core.Downloader;

namespace IgniteDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            QuickFixForParallelDownload();

            IgniteConfiguration cfg = new IgniteConfiguration(); //.FromXml();
//            cfg.SpringConfigUrl = "D:\\repos\\apache-ignite-2.7.5-bin\\config\\customConfig.client.xml";
            cfg.SpringConfigUrl = "D:\\repos\\Ignite\\configs\\customConfig.Default.xml";
            cfg.ClientMode              = true;
            //cfg.PeerAssemblyLoadingMode = PeerAssemblyLoadingMode.CurrentAppDomain;
            cfg.JvmOptions              = new List<string> {"-XX:+UseG1GC", "-XX:+DisableExplicitGC"};
            cfg.Logger = new IgniteLog4NetLogger();
            try
            {
                using (var ignite = Ignition.Start(cfg))
                {
                    Console.WriteLine();
                    Console.WriteLine(">>> Task execution example started.");

                    string url = "http://releases.ubuntu.com/16.04/ubuntu-16.04.6-desktop-amd64.iso";
                    var computes = ignite.GetCompute();

                    var result = computes
                        .Execute(new DownloadTask(), new Tuple<string, IEnumerable<Range>>(url, CalculateRanges(url, 8)));




                    Console.WriteLine("DONE");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();
        }

        private static void QuickFixForParallelDownload()
        {
            ServicePointManager.Expect100Continue       = false;
            ServicePointManager.DefaultConnectionLimit  = 1000;
            ServicePointManager.MaxServicePointIdleTime = 10000;
        }

        private static IEnumerable<Range> CalculateRanges(string fileUrl, int numberOfParallelDownloads)
        {
            WebRequest webRequest = HttpWebRequest.Create(fileUrl);  
            webRequest.Method = "HEAD";  
            long totalLength;  
            using (WebResponse webResponse = webRequest.GetResponse())  
            {  
                totalLength = long.Parse(webResponse.Headers.Get("Content-Length"));
            }

            var readRanges = new List<Range>();
            for (var chunk = 0; chunk < numberOfParallelDownloads - 1; chunk++)
            {
                var range = new Range()
                {
                    Start = chunk * (totalLength / numberOfParallelDownloads),
                    End   = ((chunk + 1)         * (totalLength / numberOfParallelDownloads)) - 1
                };
                readRanges.Add(range);
            }


            readRanges.Add(new Range()
            {
                Start = readRanges.Any() ? readRanges.Last().End + 1 : 0,
                End   = totalLength - 1
            });
            return readRanges;
        }
    }
}
