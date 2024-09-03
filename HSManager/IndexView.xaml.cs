using CandyControls;
using System.Windows;
using System.Windows.Controls;

namespace HSManager
{
    /// <summary>
    /// IndexView.xaml 的交互逻辑
    /// </summary>
    public partial class IndexView : CandyWindow
    {
        public IndexView() : base()
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
    }
}
