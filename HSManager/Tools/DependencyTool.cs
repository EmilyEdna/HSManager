using HSManager.UserControls;
using HSManager.ViewModels;
using Microsoft.Win32;
using System.Windows.Controls;
using XExten.Advance.IocFramework;

namespace HSManager.Tools
{
    public static class DependencyTool
    {
        public static UserControl Resolve(this int ctrlType)
        {
            Type UCType = null;
            Type VMType = null;
            if (ctrlType == 1)
            {
                UCType = typeof(ModPreControl);
                VMType = typeof(ModPreViewModel);
            }
            if (ctrlType == 2)
            {
                UCType = typeof(ModRepeatControl);
                VMType = typeof(ModRepeatViewModel);
            }
            if (ctrlType == 3)
            {
                UCType = typeof(ModDownControl);
                VMType = typeof(ModDownViewModel);
            }
            if (ctrlType == 4)
            {
                UCType = typeof(CharaControl);
                VMType = typeof(CharaViewModel);
            }
            if (ctrlType == 5)
            {
                UCType = typeof(SceneControl);
                VMType = typeof(SceneViewModel);
            }
            if (ctrlType == 6)
            {
                UCType = typeof(SoftControl);
                VMType = typeof(SoftViewModel);
            }
            var US = (UserControl)IocDependency.ResolveByNamed(UCType, UCType.Name);
            var VM = IocDependency.ResolveByNamed(VMType, VMType.Name);
            US.DataContext = VM;
            return US;
        }

        public static string GetRoute()
        {
            OpenFolderDialog dialog = new OpenFolderDialog()
            {
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
                return dialog.FolderName;
            else return null;
        }

        public static T Resolve<T>() where T : class
        {
            var type = typeof(T);
            return (T)IocDependency.ResolveByNamed(type, type.Name);
        }
    }
}
