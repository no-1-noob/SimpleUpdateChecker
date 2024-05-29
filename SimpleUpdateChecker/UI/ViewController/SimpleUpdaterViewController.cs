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
using UnityEngine.SceneManagement;

namespace SimpleUpdateChecker.UI.ViewController
{
    [ViewDefinition("SimpleUpdateChecker.UI.View.SimpleUpdateChecker.bsml")]
    class SimpleUpdateCheckerViewController : IInitializable, IDisposable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const string NameSuffix = "_SimpleUpdateChecker";
        private FloatingScreen floatingScreen;
        public void Initialize()
        {
            CheckVersion();
            SceneManager.activeSceneChanged += ChangedActiveScene;
        }

        private void ChangedActiveScene(Scene arg0, Scene arg1)
        {
            //Hide Update message when navigating away from main menu the first time
            if(arg1.name != "MainMenu" && floatingScreen != null)
            {
                floatingScreen.enabled = false;
                floatingScreen.gameObject.SetActive(false);
                floatingScreen = null;
                SceneManager.activeSceneChanged -= ChangedActiveScene;
            }
        }

        public void Dispose()
        {
            SceneManager.activeSceneChanged -= ChangedActiveScene;
        }

        private async void CheckVersion()
        {
            try
            {
                if ((DateTime.Now - SimpleUpdatePlugin.Version.DtLastVersionCheck).TotalHours >= 12)
                {
                    ModName = SimpleUpdatePlugin.ModCheckName;
                    CurrentVersion = GetVersionStringForComparison(Assembly.GetExecutingAssembly().GetName().Version);
                    string acknowledgedVersion = SimpleUpdatePlugin.Version.AcknowledgedVersion;
                    Version FechedNewVersion = await VersionChecker.VersionChecker.GetCurrentVersionAsync();
                    NewVersion = GetVersionStringForComparison(FechedNewVersion);
                    if (string.IsNullOrEmpty(NewVersion) || NewVersion == CurrentVersion || NewVersion == acknowledgedVersion)
                    {
                        NewVersion = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(NewVersion))
                    {
                        CreateFloatingScreen();
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModName)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentVersion)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewVersion)));
                    SimpleUpdatePlugin.Version.DtLastVersionCheck = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                SimpleUpdatePlugin.Log?.Error($"Error while checking Version: {ex.Message}");
            }
        }

        private string GetVersionStringForComparison(Version version)
        {
            if (version == null) return string.Empty;
            string tmpVersion = version.ToString();
            if(tmpVersion.Length < 5) return string.Empty;
            return $"{tmpVersion.Substring(0, 5)}";
        }

        private void CreateFloatingScreen()
        {
            int count = CountSimpleUpdateChecker();
            int multi = (count % 2) == 0 ? 1 : -1;
            float offset = multi * Mathf.Ceil(count / 2f) * 2.0f;
            floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(75, 100), false, new Vector3(offset, 4, 3), new Quaternion(0, 0, 0, 0));
            floatingScreen.gameObject.name = $"{SimpleUpdatePlugin.ModCheckName}{NameSuffix}";
            floatingScreen.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            floatingScreen.transform.eulerAngles = new Vector3(-35f, 0f, 0f);
            floatingScreen.gameObject.SetActive(false);
            BSMLParser.instance.Parse(Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"{SimpleUpdatePlugin.ModCheckName}.SimpleUpdateChecker.UI.View.SimpleUpdateChecker.bsml"), floatingScreen.gameObject, this);
            floatingScreen.gameObject.SetActive(true);
            floatingScreen.enabled = true;
        }

        private int CountSimpleUpdateChecker()
        {
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            int count = 0;
            foreach (GameObject obj in allGameObjects)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(obj.name, $".*{NameSuffix}"))
                {
                    count++;
                }
            }
            return count;
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