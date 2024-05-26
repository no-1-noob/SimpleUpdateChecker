using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.Parser;
using SimpleUpdateChecker.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace SimpleUpdateChecker.UI.ViewController
{
    [ViewDefinition("SimpleUpdateChecker.UI.View.SimpleUpdateChecker.bsml")]
    class SimpleUpdateCheckerViewController : IInitializable, IDisposable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private FloatingScreen floatingScreen;
        public void Initialize()
        {
            floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(75, 100), false, new Vector3(0, 2, 1), new Quaternion(0, 0, 0, 0));
            floatingScreen.gameObject.name = $"{SimpleUpdatePlugin.ModCheckName}_SimpleUpdateChecker";
            floatingScreen.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            floatingScreen.gameObject.SetActive(false);
            floatingScreen.enabled = false;
            
            BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"{SimpleUpdatePlugin.ModCheckName}.SimpleUpdateChecker.UI.View.SimpleUpdateChecker.bsml"), floatingScreen.gameObject, this);
            CheckVersion();
        }

        public void Dispose()
        {

        }

        private async void CheckVersion()
        {
            try
            {
                if ((DateTime.Now - SimpleUpdatePlugin.Version.DtLastVersionCheck).TotalHours >= 12)
                {
                    ModName = SimpleUpdatePlugin.ModCheckName;
                    CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    CurrentVersion = $"{CurrentVersion.Substring(0, CurrentVersion.Length - 2)}";
                    string acknowledgedVersion = SimpleUpdatePlugin.Version.AcknowledgedVersion;
                    NewVersion = await VersionChecker.VersionChecker.GetCurrentVersionAsync();
                    SimpleUpdatePlugin.Log.Error($"New version: {NewVersion}, currentVersion: {CurrentVersion}");
                    if (string.IsNullOrEmpty(NewVersion) || NewVersion == CurrentVersion || NewVersion == acknowledgedVersion)
                    {
                        NewVersion = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(NewVersion))
                    {
                        floatingScreen.gameObject.SetActive(true);
                        floatingScreen.enabled = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModName)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentVersion)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewVersion)));
                    SimpleUpdatePlugin.Version.DtLastVersionCheck = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                SimpleUpdatePlugin.Log?.Error(ex.ToString());
            }
        }

        [UIParams]
        private readonly BSMLParserParams bsmlParserParams;
        [UIValue("modName")]
        private string ModName { get; set; }
        [UIValue("newVersion")]
        private string NewVersion { get; set; }
        [UIValue("currentVersion")]
        private string CurrentVersion { get; set; }
        [UIValue("hide-update")]
        private bool HideUpdate { get; set; }
#pragma warning disable IDE0051 // Remove unused private members
        [UIAction("click-close-update-modal")]
        private void CloseUpdateClicked()
        {
            bsmlParserParams.EmitEvent("close-update-display-modal");
            if (HideUpdate)
            {
                SimpleUpdatePlugin.Version.AcknowledgedVersion = NewVersion;
                NewVersion = string.Empty;
            }
            floatingScreen.gameObject.SetActive(false);
            floatingScreen.enabled = false;
        }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning disable IDE0051 // Remove unused private members
        [UIAction("click-close-github")]
        private void CloseGithubClicked()
        {
            bsmlParserParams.EmitEvent("close-github-notification");
        }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning disable IDE0051 // Remove unused private members
        [UIAction("click-open-github")]
        private void OpenGithubClicked()
        {
            Process.Start(SimpleUpdatePlugin.NewVersionURL);
            bsmlParserParams.EmitEvent("show-github-notification");
        }
#pragma warning restore IDE0051 // Remove unused private members
    }
}