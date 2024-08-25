using CommunityToolkit.Mvvm.ComponentModel;
using HSManager.ObjectModels;
using System.Collections.ObjectModel;

namespace HSManager.ViewModels
{
    public partial class SceneViewModel : ObservableObject
    {
        public SceneViewModel()
        {
            Scene = [];
        }

        [ObservableProperty]
        private ObservableCollection<SceneInfo> _Scene;
    }
}
