using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            SimpleUpdateChecker.Plugin.SimpleUpdatePlugin.Log.Error($"Assembly1 {Assembly.GetCallingAssembly().GetName().Name} Assembly2 {Assembly.GetExecutingAssembly().GetName().Name}");
            floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(75, 100), true, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            floatingScreen.gameObject.name = "BSMLFloatingScreen_PPPredictor";
            floatingScreen.gameObject.SetActive(true);
            floatingScreen.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            floatingScreen.handle.transform.localScale = new Vector2(60, 70);
            floatingScreen.handle.transform.localPosition = new Vector3(0, 0, .1f);
            floatingScreen.handle.transform.localRotation = Quaternion.identity;
            floatingScreen.handle.hideFlags = HideFlags.HideInHierarchy;
            floatingScreen.ShowHandle = true;
            MeshRenderer floatingScreenMeshRenderer = floatingScreen.handle.GetComponent<MeshRenderer>();
            //if (floatingScreenMeshRenderer) floatingScreenMeshRenderer.enabled = false; //Make it invisible ;)

            BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"{Assembly.GetExecutingAssembly().GetName().Name}.SimpleUpdateChecker.UI.View.SimpleUpdateChecker.bsml"), floatingScreen.gameObject, this);
        }

        public void Dispose()
        {

        }

    }
}