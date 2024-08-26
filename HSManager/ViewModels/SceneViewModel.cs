using CommunityToolkit.Mvvm.ComponentModel;
using HSManager.ObjectModels;
using HSManager.Tools;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using XExten.Advance.LinqFramework;

namespace HSManager.ViewModels
{
    public partial class SceneViewModel : ObservableObject
    {
        private string _File;
        public SceneViewModel()
        {
            Scene = [];
        }

        [ObservableProperty]
        private ObservableCollection<SceneInfo> _Scene;
        [ObservableProperty]
        private SKBitmap _Source;

        public void LoadPng(string files)
        {
            Scene = [];
            _File = files;

            var bytes = PngFileTool.PngBytes(files);

            Source = SKBitmap.Decode(bytes).Resize(new SKImageInfo(300, 200), SKFilterQuality.High);

            var data = PngFileTool.ReadSceneInfo(bytes);

            var pageSize = Math.Ceiling(data.Count / 3d);

            for (int i = 0; i < pageSize; i++)
            {
                SceneInfo sinfo = new SceneInfo();
                data.Skip(i * 3).Take(3).ForEnumerEach(node =>
                {
                    if (node.ContainsKey("ModId"))
                        sinfo.Guid = node["ModId"];
                    if (node.ContainsKey("Author"))
                        sinfo.Author = node["Author"];
                    if (node.ContainsKey("Name"))
                        sinfo.Name = node["Name"];
                });
                if (!sinfo.Guid.IsNullOrEmpty())
                {
                    sinfo.IsMiss = DependencyTool.Resolve<ModPreViewModel>().MemeryData
                         .FirstOrDefault(t => t.Guid.ToUpper() == sinfo.Guid.ToUpper()) == null ? "缺失" : "存在";
                    Scene.Add(sinfo);
                }
            }

            Scene = new(Scene.GroupBy(t => t.Guid).Select(t => t.First()).ToList());
        }
    }
}
