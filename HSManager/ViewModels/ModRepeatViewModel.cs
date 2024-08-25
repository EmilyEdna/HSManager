using CommunityToolkit.Mvvm.ComponentModel;
using HSManager.ObjectModels;
using System.Collections.ObjectModel;

namespace HSManager.ViewModels
{
    public partial class ModRepeatViewModel : ObservableObject
    {
        public ModRepeatViewModel()
        {
            Mods = [];
        }

        [ObservableProperty]
        private ObservableCollection<ModsInfo> _Mods;
    }
}
