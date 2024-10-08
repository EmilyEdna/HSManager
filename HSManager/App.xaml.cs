﻿using CommunityToolkit.Mvvm.ComponentModel;
using HSManager.Tools;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using XExten.Advance.IocFramework;
using XExten.Advance.LinqFramework;
using XExten.Advance.NetFramework;

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

            NetFactoryExtension.RegisterNetFramework();

            this.GetType().Assembly.GetTypes().Where(t => t.BaseType == typeof(UserControl)).ForEnumerEach(ctrl =>
            {
                IocDependency.RegisterByNamed(ctrl,ctrl.Name);
            });

            this.GetType().Assembly.GetTypes()
                .Where(t => t.BaseType == typeof(ObservableObject))
                .ForEnumerEach(vm =>
                {
                    IocDependency.RegisterByNamed(vm,vm.Name);
                });

            UnpackTools.CreateUnpackPython3D();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            UnpackTools.RemoveUnpackPython3D();
            base.OnExit(e);
        }

    }

}
