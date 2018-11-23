using System;
using System.Collections.Generic;
using System.Text;

namespace web_server.idal.Domain
{
    public class UploadedVideoFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string CameraName { get; set; }
        public string VideoStartDateLocal { get; set; }
    }
}
