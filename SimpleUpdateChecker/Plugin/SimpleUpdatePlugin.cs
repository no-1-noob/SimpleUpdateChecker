using SimpleUpdateChecker.Data;
using SimpleUpdateChecker.Installers;
using SimpleUpdateChecker.Interface;
using SimpleUpdateChecker.Mgr;
using SiraUtil.Zenject;
using System;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;

namespace SimpleUpdateChecker.Plugin
{
    internal class SimpleUpdatePlugin
    {
        public static IPALogger Log { get; private set; }

        public static string ModCheckName { get; private set; }
        public static string UpdateCheckUrl { get; private set; }
        public static string NewVersionURL { get; private set; }
        public static VersionCheckerData Version {get; set; } = new VersionCheckerData();

        public static Type CompareType { get; private set; } = typeof(int);

        public void CreateSimpleUpdateChecker(IPALogger logger, Zenjector zenjector, string updateCheckUrl, string newVersionUrl)
        {
            CreateSimpleUpdateChecker<DefaultNewestVersion>(logger, zenjector, updateCheckUrl, newVersionUrl);
        }

        public void CreateSimpleUpdateChecker<T>(IPALogger logger, Zenjector zenjector, string updateCheckUrl, string newVersionUrl) where T: INewestVersion
        {
            if (string.IsNullOrEmpty(updateCheckUrl)) throw new ArgumentException(updateCheckUrl);
            if (string.IsNullOrEmpty(newVersionUrl)) throw new ArgumentException(newVersionUrl);
            CompareType = typeof(T);
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
