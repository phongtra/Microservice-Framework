using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Apache.Ignite.Core;
using Apache.Ignite.Log4Net;
using Core.Downloader;


namespace IgniteDownloader
{
    public class Download
    {
        public static string Initiate(string url)
        {
            QuickFixForParallelDownload();

            IgniteConfiguration cfg = new IgniteConfiguration(); //.FromXml();
            //            cfg.SpringConfigUrl = "D:\\repos\\apache-ignite-2.7.5-bin\\config\\customConfig.client.xml";
            cfg.SpringConfigUrl = "C:\\Users\\phongth\\Desktop\\IgniteSparkDownloader\\customConfig.Default.xml";
            cfg.ClientMode = true;
            //cfg.PeerAssemblyLoadingMode = PeerAssemblyLoadingMode.CurrentAppDomain;
            cfg.JvmOptions = new List<string> { "-XX:+UseG1GC", "-XX:+DisableExplicitGC" };
            cfg.Logger = new IgniteLog4NetLogger();
            string output = "";
            try
            {
                using (var ignite = Ignition.Start(cfg))
                {
                    Console.WriteLine();
                    Console.WriteLine(">>> Task execution example started.");
                    var computes = ignite.GetCompute();

                    var result = computes
                        .Execute(new DownloadTask(), new Tuple<string, IEnumerable<Core.Downloader.Range>>(url, CalculateRanges(url, 8)));

                    Console.WriteLine("result: " + result);

                    output = "Download sucessful";
                    Console.WriteLine("DONE");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                output = "Download unsucessful";
            }
            Console.WriteLine(output);
            return output;
        }
        private static void QuickFixForParallelDownload()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 1000;
            ServicePointManager.MaxServicePointIdleTime = 10000;
        }

        private static IEnumerable<Core.Downloader.Range> CalculateRanges(string fileUrl, int numberOfParallelDownloads)
        {
            WebRequest webRequest = HttpWebRequest.Create(fileUrl);
            webRequest.Method = "HEAD";
            long totalLength;
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                totalLength = long.Parse(webResponse.Headers.Get("Content-Length"));
            }

            var readRanges = new List<Core.Downloader.Range>();
            for (var chunk = 0; chunk < numberOfParallelDownloads - 1; chunk++)
            {
                var range = new Core.Downloader.Range()
                {
                    Start = chunk * (totalLength / numberOfParallelDownloads),
                    End = ((chunk + 1) * (totalLength / numberOfParallelDownloads)) - 1
                };
                readRanges.Add(range);
            }


            readRanges.Add(new Core.Downloader.Range()
            {
                Start = readRanges.Any() ? readRanges.Last().End + 1 : 0,
                End = totalLength - 1
            });
            return readRanges;
        }
    }
}
