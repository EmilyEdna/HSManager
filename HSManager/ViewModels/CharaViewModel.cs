using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace HSManager.ViewModels
{
    public partial class CharaViewModel : ObservableObject
    {
        public CharaViewModel()
        {
            MissMod = [];
        }

        [ObservableProperty]
        private ObservableCollection<string> _MissMod;
    }
}
