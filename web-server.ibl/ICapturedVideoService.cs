using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_server.ibl
{
    public interface ICapturedVideoService
    {
        Task<IEnumerable<CapturedVideoInfo>> GetVideos(DateTime from, DateTime to, string dvrName, int count);
    }
}
