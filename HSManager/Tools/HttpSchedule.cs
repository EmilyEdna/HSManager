using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Net.Http.Headers;
using XExten.Advance.NetFramework;
using XExten.Advance.StaticFramework;

namespace HSManager.Tools
{
    public class HttpSchedule
    {
        public static Action<double, object> ReceiveAction { get; set; }
        public static async void HttpDownload(string uri, string file, object obj = null, Action<HttpRequestHeaders> action = null)
        {
            var ProgressHandler = new ProgressMessageHandler(new HttpClientHandler());
            ProgressHandler.HttpReceiveProgress += (sender, e) =>
            {
                ReceiveAction?.Invoke(double.Parse(e.ProgressPercentage.ToString("F2")), obj);
            };
            HttpClient Client = new HttpClient(ProgressHandler);
            Client.DefaultRequestHeaders.Add(ConstDefault.UserAgent, ConstDefault.UserAgentValue);
            action?.Invoke(Client.DefaultRequestHeaders);
            var stream = await Client.GetStreamAsync(uri);
            SyncStatic.DeleteFile(file);
            using FileStream fs = new FileStream(file, FileMode.CreateNew);
            await stream.CopyToAsync(fs);
        }
    }
}
