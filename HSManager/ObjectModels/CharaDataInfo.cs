using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace HSManager.ObjectModels
{
    public partial class CharaDataInfo : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> _PluginInfo;
        [ObservableProperty]
        private ObservableCollection<string> _ModInfo;
    }
}
