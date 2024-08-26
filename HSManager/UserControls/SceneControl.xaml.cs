using HSManager.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using XExten.Advance.LinqFramework;

namespace HSManager.UserControls
{
    /// <summary>
    /// SceneControl.xaml 的交互逻辑
    /// </summary>
    public partial class SceneControl : UserControl
    {
        public SceneControl()
        {
            InitializeComponent();
        }

        private void DropImgEvent(object sender, DragEventArgs e)
        {
            var files = ((string[])e.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();
            if (files.IsNullOrEmpty()) return;
            if (!Path.GetExtension(files).ToLower().Contains("png")) return;
            (this.DataContext as SceneViewModel).LoadPng(files);
        }
    }
}
