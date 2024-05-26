using SimpleUpdateChecker.Data;
using SimpleUpdateChecker.Installers;
using SimpleUpdateChecker.Mgr;
using SiraUtil.Zenject;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;

namespace SimpleUpdateChecker.Plugin
{
    public class SimpleUpdatePlugin
    {
        public static IPALogger Log { get; private set; }

        public static string ModCheckName { get; private set; }
        public static string UpdateCheckUrl { get; private set; }
        public static string NewVersionURL { get; private set; }
        public static VersionCheckerData Version {get; set; } = new VersionCheckerData();

        public void CreateSimpleUpdateChecker(IPALogger logger, Zenjector zenjector, string updateCheckUrl, string newVersionUrl)
        {
            ModCheckName = Assembly.GetExecutingAssembly().GetName().Name;
            UpdateCheckUrl = updateCheckUrl;
            NewVersionURL = newVersionUrl;
            Version = SettingsMgr.LoadVersionCheckerData();
            Log = logger;
            zenjector.Install<SimpleUpdateCheckerInstaller>(Location.Menu);
        }

        public void OnApplicationQuit()
        {
            SettingsMgr.SaveVersionCheckerData(Version);
        }
    }
}
