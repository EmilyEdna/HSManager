using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HSManager.UserControls;
using System.Windows.Controls;
using HSManager.Tools;
using XExten.Advance.LinqFramework;

namespace HSManager.ViewModels
{
    public partial class IndexViewModel : ObservableObject
    {

        [ObservableProperty]
        private UserControl _Ctrl;

        [RelayCommand]
        public void Menu(string cate) => Ctrl = cate.AsInt().Resolve();

    }
}
