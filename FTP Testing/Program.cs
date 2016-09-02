using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace FTPTesting {
    internal static class Program {
        //private const string User = "Robert";
        private static readonly string DekstopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private static void Main(string[] args) {
            var webRequest = WebRequest.Create(new Uri("Address"));
            webRequest.Credentials = new NetworkCredential("name", "pass");

            var source = new StreamReader($"{DekstopPath}/fooBar.txt");

            //Upload
            webRequest.Method = WebRequestMethods.Ftp.UploadFile;
            var fileContents = Encoding.UTF8.GetBytes(source.ReadToEnd());
            source.Close();
            webRequest.ContentLength = fileContents.Length;
            var requestStream = webRequest.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            //Read from Server
            webRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            var response = (FtpWebResponse) webRequest.GetResponse();
            var responseStream = response.GetResponseStream();
            source = new StreamReader(responseStream);
            Console.WriteLine(source.ReadToEnd());

            Console.WriteLine("Download Complete, status {0}", response.StatusDescription);
            source.Close();
            response.Close();
        }
    }
}