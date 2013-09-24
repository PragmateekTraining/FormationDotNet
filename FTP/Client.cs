using System.Collections.Generic;
using System.Net;
using System.IO;

namespace FtpSamples
{
    class Client
    {
        internal string Url { get; private set; }

        // Stack<string> workingDirectory = new Stack<string>();

        internal IList<string> Connect(string url)
        {
            Url = url;

            return List("/");
        }

        internal IList<string> List(string directory)
        {
            IList<string> list = new List<string>();

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Url + "/" + directory);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                // request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }

            return list;
        }

        internal void Download(string file)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Url + "/" + file);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream fileStream = File.OpenWrite(Path.GetFileName(file)))
            {
                responseStream.CopyTo(fileStream);
            }
        }
    }
}
