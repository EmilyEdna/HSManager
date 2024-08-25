using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSManager.ObjectModels
{
    public partial class SceneInfo : ObservableObject
    {
        [ObservableProperty]
        private string _Author;
        [ObservableProperty]
        private string _Name;
        [ObservableProperty]
        private string _Guid;
        [ObservableProperty]
        private string _IsMiss;
    }
}
