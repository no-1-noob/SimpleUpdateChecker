using SimpleUpdateChecker.Plugin;
using SimpleUpdateChecker.UI.ViewController;
using System.ComponentModel;
using Zenject;

namespace SimpleUpdateChecker.Installers
{
    internal class SimpleUpdateCheckerInstaller : Installer
    {
        public override void InstallBindings()
        {
            SimpleUpdatePlugin.Log.Error("SimpleUpdateCheckerInstaller");
            Container.BindInterfacesAndSelfTo<SimpleUpdateCheckerViewController>().AsSingle();
        }
    }
}
