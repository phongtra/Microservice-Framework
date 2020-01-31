using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LocalDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;

            string url = "http://releases.ubuntu.com/16.04/ubuntu-16.04.6-desktop-amd64.iso";
            var result = Downloader.Download(url, @"D:\temp\", 4);  
              
            Console.WriteLine($"Location: {result.FilePath}");  
            Console.WriteLine($"Size: {result.Size}bytes");  
            Console.WriteLine($"Time taken: {result.TimeTaken.Milliseconds}ms");  
            Console.WriteLine($"Parallel: {result.ParallelDownloads}");  
  
            Console.ReadKey();  
        }

    }
}
