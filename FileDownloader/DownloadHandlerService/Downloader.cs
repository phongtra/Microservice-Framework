using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DownloadHandlerService
{
    public class Downloader
    {
        public Downloader()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.MaxServicePointIdleTime = 1000;

        }
        public static DownloadResult Download(String fileUrl, String destinationFolderPath, int numberOfParallelDownloads = 0, bool validateSSL = false)
        {
            if (!validateSSL)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            Uri uri = new Uri(fileUrl);

            //Calculate destination path  
            String destinationFilePath = Path.Combine(destinationFolderPath, uri.Segments.Last());

            DownloadResult result = new DownloadResult() { FilePath = destinationFilePath };

            //Handle number of parallel downloads  
            if (numberOfParallelDownloads <= 0)
            {
                numberOfParallelDownloads = Environment.ProcessorCount;
            }

            #region Get file size  
            WebRequest webRequest = HttpWebRequest.Create(fileUrl);
            webRequest.Method = "HEAD";
            long responseLength;
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                responseLength = long.Parse(webResponse.Headers.Get("Content-Length"));
                result.Size = responseLength;
            }
            #endregion

            if (File.Exists(destinationFilePath))
            {
                File.Delete(destinationFilePath);
            }

            using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Append))
            {
                ConcurrentDictionary<long, String> tempFilesDictionary = new ConcurrentDictionary<long, String>();

                var readRanges = CalculateRanges(numberOfParallelDownloads, responseLength);

                DateTime startTime = DateTime.Now;

                #region Parallel download  

                int index = 0;
                Parallel.ForEach(readRanges, new ParallelOptions() { MaxDegreeOfParallelism = numberOfParallelDownloads }, readRange =>
                {
                    var tempFilePath = string.Empty;
                    if (WebRequest.Create(fileUrl) is HttpWebRequest httpWebRequest)
                    {
                        tempFilePath = DownloadChunk(httpWebRequest, readRange);
                    }
                    tempFilesDictionary.TryAdd(readRange.Start, tempFilePath);
                    index++;
                });

                result.ParallelDownloads = index;

                #endregion

                result.TimeTaken = DateTime.Now.Subtract(startTime);

                #region Merge to single file  
                foreach (var tempFile in tempFilesDictionary.OrderBy(b => b.Key))
                {
                    byte[] tempFileBytes = File.ReadAllBytes(tempFile.Value);
                    destinationStream.Write(tempFileBytes, 0, tempFileBytes.Length);
                    File.Delete(tempFile.Value);
                }
                #endregion


                return result;
            }


        }

        private static IEnumerable<Range> CalculateRanges(int numberOfParallelDownloads, long totalLength)
        {
            var readRanges = new List<Range>();
            for (var chunk = 0; chunk < numberOfParallelDownloads - 1; chunk++)
            {
                var range = new Range()
                {
                    Start = chunk * (totalLength / numberOfParallelDownloads),
                    End = ((chunk + 1) * (totalLength / numberOfParallelDownloads)) - 1
                };
                readRanges.Add(range);
            }


            readRanges.Add(new Range()
            {
                Start = readRanges.Any() ? readRanges.Last().End + 1 : 0,
                End = totalLength - 1
            });
            return readRanges;
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
                    Debug.Assert(httpWebResponse != null, nameof(httpWebResponse) + " != null");
                    httpWebResponse.GetResponseStream()?.CopyTo(fileStream);
                }
            }

            return tempFilePath;
        }
    }
}
