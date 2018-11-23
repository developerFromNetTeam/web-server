using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using web_server.idal.Domain;

namespace web_server.idal.Converters
{
    public static class UploadedVideoFileConverter
    {
        public static UploadedVideoFile ToUploadedVideoFile(this Hashtable hashtable)
        {
            return new UploadedVideoFile
            {
                FileName = hashtable[MongoDbFields.FileName].ToString(),
                CameraName = hashtable[MongoDbFields.CameraName].ToString(),
                FilePath = hashtable[MongoDbFields.FilePath].ToString(),
                VideoStartDateLocal = DateTime.Parse(hashtable[MongoDbFields.VideoStartDateLocal].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
            };
        }
    }
}
