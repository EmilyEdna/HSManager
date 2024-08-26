using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HSManager.properties;
using HSManager.Tools;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using XExten.Advance.LinqFramework;

namespace HSManager.ViewModels
{
    public partial class CharasViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _Pngs;

        [RelayCommand]
        public void Watch(string file) => WeakReferenceMessenger.Default.Send(file);

        [RelayCommand]
        public void Load() => LoadChara();

        [RelayCommand]
        public void Batch()
        {
            var VM = DependencyTool.Resolve<CharaViewModel>();
            Pngs.ForEnumerEach(item =>
            {
                Task.Run(() =>
                {
                    Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        VM.LoadPng(item);
                        VM.ExportPng();
                    });
                });
            });
            if (!Soft.Default.CharaExportRoute.IsNullOrEmpty())
                Process.Start("explorer.exe", Soft.Default.CharaExportRoute);
        }

        private void EachFolder(string dir, List<string> files)
        {
            var Dirs = Directory.GetDirectories(dir);
            if (Dirs.Count() > 0)
            {
                Dirs.ForArrayEach<string>(node =>
                {

                    var Mods = Directory.GetFiles(node).Where(t => Path.GetExtension(t).ToLower().Contains("png"));
                    if (Mods.Count() > 0)
                        files.AddRange(Mods);
                    EachFolder(node, files);
                });
            }
        }

        private void LoadChara()
        {
            if (Soft.Default.CharaImportRoute.IsNullOrEmpty()) return;
            List<string> files = [];
            Directory.GetFiles(Soft.Default.CharaImportRoute)
                .Where(t => Path.GetExtension(t).ToLower().Contains("png")).ForEnumerEach(files.Add);
            EachFolder(Soft.Default.CharaImportRoute, files);
            Pngs = new(files);
        }
    }
}
