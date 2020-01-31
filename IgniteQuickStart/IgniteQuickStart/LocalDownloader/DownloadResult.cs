using System;

namespace LocalDownloader
{
    public class DownloadResult  
    {  
        public long     Size              { get; set; }  
        public String   FilePath          { get; set; }  
        public TimeSpan TimeTaken         { get; set; }  
        public int      ParallelDownloads { get; set; }  
    }
}