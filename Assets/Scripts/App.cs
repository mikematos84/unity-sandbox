using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BestHTTP;
using UnityEngine.SceneManagement;
using TMPro;

namespace HeavyDev
{
    public class App : MonoBehaviour
    {
        // Configuration
        public static Dictionary<string, object> config = new Dictionary<string, object>();

        public string sceneToLoad = "Main";
        public string loadedScene;

        public TextMeshProUGUI sceneLabel;

        public bool isReady = false;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void Start()
        {
            LoadConfigurationFile();
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            loadedScene = scene.name;
            sceneLabel.text = loadedScene;
        }

        /// <summary>
        /// Load configuration file
        /// </summary>
        private void LoadConfigurationFile()
        {
            Debug.Log(string.Format("<color=green>Loading configuration file</color>"));
            string filePath = Application.streamingAssetsPath + "/config.json";

            new HTTPRequest(new Uri(filePath), HTTPMethods.Get, (req, resp) =>
            {
                Debug.Log(string.Format("<color=blue>{0}</color>", resp.StatusCode));
                if (resp.StatusCode == 200)
                {
                    config.Clear();
                    config = JsonConvert.DeserializeObject<Dictionary<string, object>>(resp.DataAsText);
                    Debug.Log(string.Format("<color=green>Configuration loaded</color>"));
                }
                else
                {
                    Debug.Log(string.Format("<color=yellow>Configuration not found</color>"));
                }
                InitializeApp();
            }).Send();
        }

        /// <summary>
        /// Initialize Application after configuration has been loaded
        /// </summary>
        public void InitializeApp()
        {
            //Turn of App Scene Camera;
            Destroy(Camera.main.gameObject);

            Messenger.ListenTo(Notifications.ServicesReady, HandleServicesReady);
            Messenger.ListenTo(Notifications.AppReady, HandleAppReady);
            Messenger.ListenTo(Notifications.AppExit, HandleAppExit);

            RegisterServices();
        }

        /// <summary>
        /// Registers service necessary for game, in order
        /// </summary>
        private void RegisterServices()
        {
            Debug.Log(string.Format("<color=green>Registering services</color>"));
            ServiceLocator.Register(new List<object>
            {
                // Register App
                this,
                // Register Local Storage
                new LocalStorage(this),
                // Register HttpService
                new HttpService(this), 
                // Canvas Fader
                GetComponentInChildren<CanvasFader>(true)
            }, (services) =>
            {
                var list = services.Keys.ToArray();
                Debug.Log(string.Format("<color=green>Services registered [{0}]</color>", string.Join("], [", list)));
                Messenger.SendNote(Notifications.ServicesReady);
            });
        }


        /// <summary>
        /// At this point, the apps core services have been loaded
        /// but the app has not yet been initialize.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleServicesReady(object obj)
        {
            Debug.Log(string.Format("<color=green>Services Ready</color>"));
            Messenger.SendNote(Notifications.AppReady);
        }
        /// <summary>
        /// Ready to initiate application.
        /// At this point, all necessary core components of the application, such as 
        /// configuration files, services, etc... have been loaded. 
        /// </summary>
        /// <param name="obj"></param>
        private void HandleAppReady(object obj)
        {
            Debug.Log(string.Format("<color=green>Application Ready</color>"));
            isReady = true;
            LoadScene(sceneToLoad);
        }

        private void HandleAppExit(object obj)
        {
            Debug.Log(string.Format("<color=green>Application Excited</color>"));
        }

        public IEnumerator UnloadAsync()
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(loadedScene);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
            Debug.Log(string.Format("{0} Scene Unloaded", loadedScene));
        }

        public IEnumerator LoadAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            Debug.Log(string.Format("{0} Scene Loaded", sceneName));
        }

        public LTDescr LoadScene(string sceneName)
        {
            CanvasFader canvasFader = ServiceLocator.Find<CanvasFader>();

            LTSeq seq = LeanTween.sequence();

            if (loadedScene != "App" && loadedScene != null)
            {
                seq.append(canvasFader.FadeIn());
                seq.append(() => StartCoroutine(UnloadAsync()));
            }

            seq.append(() => StartCoroutine(LoadAsync(sceneName)));
            seq.append(canvasFader.FadeOut());

            return seq.tween;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
    }
}
