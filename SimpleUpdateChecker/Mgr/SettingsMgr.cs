using IPA.Utilities;
using Newtonsoft.Json;
using SimpleUpdateChecker.Data;
using SimpleUpdateChecker.Plugin;
using System;
using System.IO;

namespace SimpleUpdateChecker.Mgr
{
    internal class SettingsMgr
    {
        private static readonly string settingsPath = Path.Combine(UnityGame.UserDataPath, $"SimpleUpdateChecker_{SimpleUpdatePlugin.ModCheckName}.json");
        internal static VersionCheckerData LoadVersionCheckerData()
        {
            if (File.Exists(settingsPath))
            {
                try
                {
                    return JsonConvert.DeserializeObject<VersionCheckerData>(File.ReadAllText(settingsPath));
                }
                catch (Exception ex)
                {
                    SimpleUpdatePlugin.Log?.Error(ex);
                    SimpleUpdatePlugin.Log?.Error($"Unable to load VersionCheckerData for {SimpleUpdatePlugin.ModCheckName} from file. Creating new Profile.");
                }
            }
            else
            {
                SimpleUpdatePlugin.Log?.Error($"Unable to load VersionCheckerData for {SimpleUpdatePlugin.ModCheckName} from file. Creating new Profile.");
            }
            return new VersionCheckerData();
        }

        internal static bool SaveVersionCheckerData(VersionCheckerData versionCheckerData)
        {
            bool saved = true;
            try
            {
                File.WriteAllText(settingsPath, JsonConvert.SerializeObject(versionCheckerData, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                }));
            }
            catch (Exception ex)
            {
                SimpleUpdatePlugin.Log?.Error(ex);
                SimpleUpdatePlugin.Log?.Error("Error saving Config");
                saved = false;
            }
            return saved;
        }
    }
}
