using CommunityToolkit.Mvvm.ComponentModel;
using HSManager.UserControls;
using HSManager.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows.Controls;
using XExten.Advance.IocFramework;
using XExten.Advance.NetFramework;
using XExten.Advance.StaticFramework;
using System.Net.Http.Handlers;

namespace HSManager.Tools
{
    public static class DependencyTool
    {
        public static UserControl Resolve(this int ctrlType)
        {
            Type UCType = null;
            Type VMType = null;
            if (ctrlType == 1)
            {
                UCType = typeof(ModPreControl);
                VMType = typeof(ModPreViewModel);
            }
            if (ctrlType == 2)
            {
                UCType = typeof(ModRepeatControl);
                VMType = typeof(ModRepeatViewModel);
            }
            if (ctrlType == 3)
            {
                UCType = typeof(ModDownControl);
                VMType = typeof(ModDownViewModel);
            }
            if (ctrlType == 4)
            {
                UCType = typeof(CharaControl);
                VMType = typeof(CharaViewModel);
            }
            if (ctrlType == 5)
            {
                UCType = typeof(SceneControl);
                VMType = typeof(SceneViewModel);
            }
            if (ctrlType == 6)
            {
                UCType = typeof(SoftControl);
                VMType = typeof(SoftViewModel);
            }
            var US = (UserControl)IocDependency.ResolveByNamed(UCType, UCType.Name);
            var VM = IocDependency.ResolveByNamed(VMType, VMType.Name);
            US.DataContext = VM;
            return US;
        }

        public static string GetRoute()
        {
            OpenFolderDialog dialog = new OpenFolderDialog()
            {
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
                return dialog.FolderName;
            else return null;
        }

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
