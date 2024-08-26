using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HSManager.ObjectModels
{
    public partial class TreeInfo : ObservableObject
    {
        [ObservableProperty]
        private string _Root;
        [ObservableProperty]
        private ObservableCollection<TreeChild> _KeyValue;
    }

    public partial class TreeChild : ObservableObject
    {
        [ObservableProperty]
        private int _Index;
        [ObservableProperty]
        private string _Key;
        [ObservableProperty]
        private string _Value;
        [ObservableProperty]
        private bool _Check;
        [ObservableProperty]
        private double _Process;
        [ObservableProperty]
        private string _FileSize;
        [ObservableProperty]
        private string _State;
        public Visibility Show=> Path.GetExtension(this.Value.ToString()).Contains(".zipmod")?Visibility.Visible:Visibility.Collapsed;
    }
}
