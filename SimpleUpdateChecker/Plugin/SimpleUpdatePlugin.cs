using SimpleUpdateChecker.Installers;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace SimpleUpdateChecker.Plugin
{
    public class SimpleUpdatePlugin
    {
        public static IPALogger Log { get; private set; }

        public void Init(IPALogger logger, Zenjector zenjector)
        {
            Log = logger;
            zenjector.Install<SimpleUpdateCheckerInstaller>(Location.Menu);
        }
    }
}
