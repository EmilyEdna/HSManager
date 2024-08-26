using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HSManager.UserControls;
using System.Windows.Controls;
using HSManager.Tools;
using XExten.Advance.LinqFramework;
using CommunityToolkit.Mvvm.Messaging;

namespace HSManager.ViewModels
{
    public partial class IndexViewModel : ObservableObject
    {
        public IndexViewModel()
        {
            WeakReferenceMessenger.Default.Register<string>(this, (o, s) => {
                var Contrl = 4.Resolve();
                ((CharaViewModel)Contrl.DataContext).LoadPng(s);
                Ctrl = Contrl;
                WeakReferenceMessenger.Default.Unregister<IndexViewModel, string>(this,s);
            });

        }

        [ObservableProperty]
        private UserControl _Ctrl;

        [RelayCommand]
        public void Menu(string cate) => Ctrl = cate.AsInt().Resolve();

    }
}
