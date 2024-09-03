using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HSManager.ObjectModels;
using HSManager.properties;
using HSManager.Tools;
using System.IO;
using XExten.Advance.LinqFramework;

namespace HSManager.ViewModels
{
    public partial class SoftViewModel : ObservableObject
    {
        private readonly string _GameName = "HoneySelect2.exe";

        public SoftViewModel()
        {
            Model = new();

            Model.Game = Soft.Default.GameRoute;
            Model.CharaImport = Soft.Default.CharaImportRoute;
            Model.CharaExport = Soft.Default.CharaExportRoute;
            Model.Save = Soft.Default.SaveRoute;
            Model.UnityUnpack = Soft.Default.UnpackUnity3D;
        }

        [ObservableProperty]
        private RouteModel _Model;

        [RelayCommand]
        public void Route(string cate)
        {
            var type = cate.AsInt();
            if (type != 5)
            {
                var Path = DependencyTool.GetRoute();
                if (Path.IsNullOrEmpty()) return;

                if (type == 1)
                {
                    if (Directory.GetFiles(Path).Any(t => t.Contains(_GameName)))
                        Model.Game = Path;
                }
                if (type == 2) Model.CharaExport = Path;
                if (type == 3) Model.CharaImport = Path;
                if (type == 4) Model.Save = Path;
            }
            Soft.Default.GameRoute = Model.Game;
            Soft.Default.CharaImportRoute = Model.CharaImport;
            Soft.Default.CharaExportRoute = Model.CharaExport;
            Soft.Default.SaveRoute = Model.Save;
            Soft.Default.UnpackUnity3D= Model.UnityUnpack;
            Soft.Default.Save();
        }

    }
}
