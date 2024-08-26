using HSManager.Tools;
using HSManager.ViewModels;
using NStandard;
using Org.BouncyCastle.Utilities;
using SkiaSharp;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HSManager.UserControls
{
    /// <summary>
    /// CharaControl.xaml 的交互逻辑
    /// </summary>
    public partial class CharaControl : UserControl
    {
        public CharaControl()
        {
            InitializeComponent();
        }

        private void DropImgEvent(object sender, DragEventArgs e)
        {
            var files = ((string[])e.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();
            if (files.IsNullOrEmpty()) return;
            if (!Path.GetExtension(files).ToLower().Contains("png")) return;
            (this.DataContext as CharaViewModel).LoadPng(files);
        }
    }
}
