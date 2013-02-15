namespace DLExt.RestService
{
    using System;    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web.Script.Serialization;
    using log4net;    

    public static class Utils
    {
        private static ILog logger = LogManager.GetLogger(typeof(Utils));        public static string CallRestGet(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Proxy = WebRequest.GetSystemWebProxy();
            request.Method = "GET";
            request.ContentType = "text/json";
            request.PreAuthenticate = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            var response = (HttpWebResponse)request.GetResponse();

            string output;
            using (var streamIn = new StreamReader(response.GetResponseStream()))
            {
                output = streamIn.ReadToEnd();
            }

            return output;
        }

        public static string CallRestPost(string uri, byte[] data)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Proxy = WebRequest.GetSystemWebProxy();
            request.Method = "POST";
            request.ContentType = "application/json";
            request.PreAuthenticate = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            var newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            var response = (HttpWebResponse)request.GetResponse();
            string output;
            using (var streamIn = new StreamReader(response.GetResponseStream()))
            {
                output = streamIn.ReadToEnd();
            }

            return output;
        }

        public static T ParseJson<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        public static string FormatJson(object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
}
