using SimpleUpdateChecker.UI.ViewController;
using Zenject;

namespace SimpleUpdateChecker.Installers
{
    internal class SimpleUpdateCheckerInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SimpleUpdateCheckerViewController>().AsSingle();
        }
    }
}
