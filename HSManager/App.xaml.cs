using CommunityToolkit.Mvvm.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using XExten.Advance.IocFramework;
using XExten.Advance.LinqFramework;

namespace HSManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.GetType().Assembly.GetTypes().Where(t => t.BaseType == typeof(UserControl)).ForEnumerEach(ctrl =>
            {
                IocDependency.RegisterByNamed(ctrl,ctrl.Name);
            });

            this.GetType().Assembly.GetTypes()
                .Where(t => t.BaseType == typeof(ObservableObject))
                .Where(t => !t.FullName.Contains("Index"))
                .ForEnumerEach(vm =>
                {
                    IocDependency.RegisterByNamed(vm,vm.Name);
                });

        }
    }

}
