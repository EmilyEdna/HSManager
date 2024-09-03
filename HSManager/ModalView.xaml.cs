using CandyControls;
using System.Windows.Controls;
using System.Windows;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace HSManager
{
    /// <summary>
    /// ModalView.xaml 的交互逻辑
    /// </summary>
    public partial class ModalView : CandyWindow
    {
        public ModalView()
        {
            InitializeComponent();
            Width = MinWidth = 800d;
            Height = MinHeight = 450d;
            this.Loaded += delegate
            {
                ((Button)this.Template.FindName("Setting", this)).Visibility = Visibility.Collapsed;
                ((Button)this.Template.FindName("Maximize", this)).Visibility = Visibility.Collapsed;
                ((Button)this.Template.FindName("Minimize", this)).Visibility = Visibility.Collapsed;
            };
        }

        public void SetListData(ObservableCollection<SKBitmap> data)
        {
            Views.ItemsSource = data;
        }
    }
}
