using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Apache.Ignite.Core.Compute;

namespace Core.Downloader
{
    /// <summary>
    /// Tuple&lt;string, IEnumerable&lt;Range&gt;: FileUrl & list of ranges
    /// </summary>
    public class DownloadTask : ComputeTaskSplitAdapter<Tuple<string, IEnumerable<Range>>, List<Tuple<long, string>>, string>
    {
        string url;

//        protected override ICollection<IComputeJob<ConcurrentDictionary<long, string>>> Split(int gridSize, Tuple<string, IEnumerable<Range>> arg)
        protected override ICollection<IComputeJob<List<Tuple<long, string>>>> Split(int gridSize, Tuple<string, IEnumerable<Range>> arg)
        {
            url = arg.Item1;
            var    ranges = arg.Item2;

            var jobs = new List<IComputeJob<List<Tuple<long, string>>>>(gridSize);

            int count = 0;

            foreach (Range range in ranges)
            {
                int idx = count++ % gridSize;

                DownloadJob job;

                if (idx >= jobs.Count)
                {
                    job = new DownloadJob(url);

                    jobs.Add(job);
                }
                else
                    job = (DownloadJob) jobs[idx];

                job.Add(range);
            }

            return jobs;
        }

//        public override string Reduce(IList<IComputeJobResult<ConcurrentDictionary<long, string>>> results)
        public override string Reduce(IList<IComputeJobResult<List<Tuple<long, string>>>> results)
        {
            var uri = new Uri(url);
            var destinationFolderPath = @"C:\Users\phongth\Desktop\DownloadedFile";
            
            var destinationFilePath = Path.Combine(destinationFolderPath, uri.Segments.Last());

//            var tempFilesDictionary = new Dictionary<long, string>();
//            tempFilesDictionary =
//                results.Select(result => result.Data)
//                .Aggregate(tempFilesDictionary, (current, result) => current.Concat(result)
//                    .GroupBy(i => i.Key)
//                    .ToDictionary(group => @group.Key, group => @group.First().Value));

            var tempFilesDictionary = results.SelectMany(x => x.Data).ToList();

            using (var destinationStream = new FileStream(destinationFilePath, FileMode.Append))
            {

//                foreach (var tempFile in tempFilesDictionary.OrderBy(b => b.Key))
                foreach (var tempFile in tempFilesDictionary.OrderBy(b => b.Item1))
                {
                    var fileName = tempFile.Item2;
                    byte[] tempFileBytes = File.ReadAllBytes(fileName);
                    destinationStream.Write(tempFileBytes, 0, tempFileBytes.Length);
                    File.Delete(fileName);
                }
            }

            return destinationFilePath;
        }
    }
 
}
