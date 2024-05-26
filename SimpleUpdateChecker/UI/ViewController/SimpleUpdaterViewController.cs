using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.Parser;
using SimpleUpdateChecker.Plugin;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
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
            System.Random r = new System.Random(Guid.NewGuid().GetHashCode());
            float randomPos = 5f * ((float)r.NextDouble() - 0.5f);
            floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(75, 100), false, new Vector3(randomPos, 3.5f, 4), new Quaternion(0, 0, 0, 0));
            floatingScreen.gameObject.name = $"{SimpleUpdatePlugin.ModCheckName}_SimpleUpdateChecker";
            floatingScreen.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            floatingScreen.gameObject.SetActive(false);
            BSMLParser.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"{SimpleUpdatePlugin.ModCheckName}.SimpleUpdateChecker.UI.View.SimpleUpdateChecker.bsml"), floatingScreen.gameObject, this);
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

#pragma warning disable 0649
        [UIParams]
        private readonly BSMLParserParams bsmlParserParams;
#pragma warning restore
        [UIValue("modName")]
        private string ModName { get; set; } = string.Empty;
        [UIValue("newVersion")]
        private string NewVersion { get; set; } = string.Empty;
        [UIValue("currentVersion")]
        private string CurrentVersion { get; set; } = string.Empty;
        [UIValue("hide-update")]
        private bool HideUpdate { get; set; } = false;
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