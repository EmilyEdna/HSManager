using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HSManager.ObjectModels;
using System.Collections.ObjectModel;
using System.IO;

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

        [RelayCommand]
        public void Delete(ModsInfo mod) 
        {
            File.Delete(mod.Route);

            if (Mods.Count(t => t.Guid == mod.Guid && t.Route != mod.Route) >= 2)
                Mods.RemoveAt(Mods.ToList().IndexOf(mod));
            else
            {
                var temp = Mods.ToList();
                temp.RemoveAll(t => t.Guid == mod.Guid);
                Mods = new(temp);
            }
        }
    }
}
