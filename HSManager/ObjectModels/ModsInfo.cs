using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSManager.ObjectModels
{
    public partial class ModsInfo : ObservableObject
    {
        [ObservableProperty]
        private string _Route;
        [ObservableProperty]
        private string _Guid;
        [ObservableProperty]
        private string _Author;
        [ObservableProperty]
        private string _Name;
        [ObservableProperty]
        private string _Version;
        [ObservableProperty]
        private int _Index;
        [ObservableProperty]
        private SKBitmap _PreImage;
        public List<U3d> U3d { get; set; }
        [ObservableProperty]
        private ObservableCollection<SKBitmap> _Assets;
    }
    public class U3d
    {
        public string Route { get; set; }
        public string SortRoute { get; set; }
    }

}
