using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HSManager.ObjectModels;
using HSManager.properties;
using HSManager.Tools;
using NStandard;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using XExten.Advance.StaticFramework;

namespace HSManager.ViewModels
{
    public partial class CharaViewModel : ObservableObject
    {
        private string _File;
        public CharaViewModel()
        {
            MissMod = [];
        }

        [ObservableProperty]
        private ObservableCollection<string> _MissMod;
        [ObservableProperty]
        private SKBitmap _Source;
        [ObservableProperty]
        private CharaDataInfo _Chara;

        [RelayCommand]
        public void Export() 
        {
            var data = DependencyTool.Resolve<ModPreViewModel>().MemeryData;
            if (data.Count <= 0) return;
            if (Soft.Default.CharaExportRoute.IsNullOrEmpty()) return;
            if (_File.IsNullOrEmpty()) return;
            var root = SyncStatic.CreateDir(Path.Combine(Soft.Default.CharaExportRoute, Path.GetFileName(_File).Split(".").First()));
            foreach (var item in Chara.ModInfo)
            {
                var source = data.FirstOrDefault(t => t.Guid.ToUpper().Equals(item.ToUpper()));
                if(source==null) continue;
                if (!source.Route.IsNullOrEmpty())
                {
                    File.Copy(source.Route, Path.Combine(SyncStatic.CreateDir(Path.Combine(root, "mod")), $"{item}.zipmod"),true);
                }
                var _3d = source.U3d;
                if (_3d.Count > 0)
                {
                    _3d.ForEach(u =>
                    {
                        SyncStatic.CreateDir(Directory.GetParent(Path.Combine(root, u.SortRoute)).FullName);

                        File.Copy(u.Route, Path.Combine(root, u.SortRoute), true);
                    });
                }
            }

            Process.Start("explorer.exe", root);
        }

        public void LoadPng(string files)
        {
            _File = files;

            var bytes = PngFileTool.PngBytes(files);

            Source = SKBitmap.Decode(bytes).Resize(new SKImageInfo(250, 350), SKFilterQuality.High);

            Chara = PngFileTool.ReadInfo(bytes);

            var data = DependencyTool.Resolve<ModPreViewModel>().MemeryData;

            Chara.ModInfo.ToList().ForEach(item =>
            {
                if (data.Count<=0)
                {
                    MissMod.Add(item);
                    return;
                }
                if (!data.Any(t => t.Guid.ToUpper().Equals(item.ToUpper())))
                {
                    MissMod.Add(item);
                }
            });

            MissMod = [.. MissMod.Distinct()];
        }
    }
}
