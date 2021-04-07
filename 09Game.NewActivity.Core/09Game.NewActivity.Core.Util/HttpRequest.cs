using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace _09Game.NewActivity.Core.Util
{
    public class HttpRequest
    {
        #region 同步请求 GET POST
        public static string HttpGet(string url)
        {
            var task = new HttpClient().GetStringAsync(url);
            task.Wait();
            return task.Result;
        }
        public static string HttpPostXml(string url, string xml)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient();

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            HttpContent content = new StreamContent(stream);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var postTask = client.PostAsync(url, content);
            postTask.Wait();
            var readTask = postTask.Result.Content.ReadAsStringAsync();
            readTask.Wait();
            return readTask.Result;
        }
        #endregion

        #region 异步请求 GET POST
        public static async Task<string> HttpGetAsync(string url, int timeout = 5000)
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromSeconds(60),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(20),
                MaxConnectionsPerServer = 2
            };
            var client = new HttpClient(socketsHandler);

            //var client = new HttpClient()
            //{
            //    Timeout = TimeSpan.FromMilliseconds(timeout)
            //};

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            var s = await client.GetStringAsync(url);
            //handler.Dispose();
            client.Dispose();
            return s;
        }

        public static async Task<string> HttpPostXmlAsync(string url, string xml)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            HttpContent content = new StreamContent(stream);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var task = await client.PostAsync(url, content);
            return await task.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
