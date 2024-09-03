using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSManager.ObjectModels
{
    public partial class RouteModel : ObservableObject
    {
        [ObservableProperty]
        private string _Game;
        [ObservableProperty]
        private string _Save;
        [ObservableProperty]
        private string _CharaExport;
        [ObservableProperty]
        private string _CharaImport;
        [ObservableProperty]
        private bool _UnityUnpack;
    }
}
