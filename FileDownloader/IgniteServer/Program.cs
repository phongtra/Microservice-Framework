using System;
using System.Collections.Generic;
using System.Net;
using Apache.Ignite.Core;
using Apache.Ignite.Log4Net;



namespace IgniteServer
{
    class Program
    {
        static void Main(string[] args)
        {
            QuickFixForParallelDownload();

            IgniteConfiguration cfg = new IgniteConfiguration(); //.FromXml();
            cfg.SpringConfigUrl = "C:\\Users\\phongth\\Desktop\\IgniteSparkDownloader\\customConfig.Default.xml";
            cfg.ClientMode = false;
            //cfg.PeerAssemblyLoadingMode = PeerAssemblyLoadingMode.CurrentAppDomain;
            cfg.Assemblies = new List<string> { "Core" };
            cfg.JvmOptions = new List<string> { "-XX:+UseG1GC", "-XX:+DisableExplicitGC" };
            cfg.Logger = new IgniteLog4NetLogger();
            System.Environment.SetEnvironmentVariable("java.net.preferIPv4Stack", "true");

            using (var ignite = Ignition.Start(cfg))
            {
                Console.WriteLine("Press Enter to quit");
                Console.ReadLine();
            }
        }

        private static void QuickFixForParallelDownload()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 1000;
            ServicePointManager.MaxServicePointIdleTime = 10000;
        }
    }
}
