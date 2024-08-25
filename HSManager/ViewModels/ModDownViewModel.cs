using CommunityToolkit.Mvvm.ComponentModel;
using HSManager.ObjectModels;
using HSManager.Tools;

namespace HSManager.ViewModels
{
    public partial class ModDownViewModel : ObservableObject
    {
        public ModDownViewModel()
        {
            LoadModWebSite();
        }

        [ObservableProperty]
        private TreeInfo _Tree;
        private string _Filter;
        public string Filter {
            get => _Filter;
            set { SetProperty(ref _Filter, value); }
        }

        private async void LoadModWebSite()
        {
            Tree = await HS2Betterrepack.GetRoot();
        }
    }
}
