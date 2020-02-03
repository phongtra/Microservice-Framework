using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Apache.Ignite.Core.Compute;

namespace Core.Downloader
{
//    public class DownloadJob:ComputeJobAdapter<ConcurrentDictionary<long,string>>
    public class DownloadJob:ComputeJobAdapter<List<Tuple<long, string>>>
    {
        private int numberOfParallelDownloads = 2;
        public string Url { get; }
        private readonly List<Range> _downloadRanges = new List<Range>();

        public DownloadJob(string url)
        {
            Url = url;
        }

        public void Add(Range range)
        {
            _downloadRanges.Add(range);
        }

        public override List<Tuple<long, string>> Execute()
        {
            var tempFilesDictionary = new ConcurrentDictionary<long, string>();

            Parallel.ForEach(_downloadRanges, new ParallelOptions { MaxDegreeOfParallelism = numberOfParallelDownloads }, readRange =>  
            {
                var tempFilePath = string.Empty;
                if (WebRequest.Create(Url) is HttpWebRequest httpWebRequest)
                {
                    tempFilePath = DownloadChunk(httpWebRequest, readRange);
                }
                tempFilesDictionary.TryAdd(readRange.Start, tempFilePath);
            });


            var list = tempFilesDictionary
                .Select(kv => Tuple.Create(kv.Key, kv.Value))
                .OrderBy(e => e.Item1)
                .ToList();
//            return tempFilesDictionary;
            return list;
        }

        private static string DownloadChunk(HttpWebRequest httpWebRequest, Range readRange)
        {
            httpWebRequest.Method = "GET";
            httpWebRequest.AddRange(readRange.Start, readRange.End);
            var tempFilePath = Path.GetTempFileName();
            using (var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
            {
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write,
                    FileShare.Write))
                {
                    httpWebResponse.GetResponseStream()?.CopyTo(fileStream);
                }
            }

            return tempFilePath;
        }
    }
}
