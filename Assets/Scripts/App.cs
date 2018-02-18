using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeavyDev
{
    public class App : MonoBehaviour
    {
        // Configuration
        public static Dictionary<string, object> config = new Dictionary<string, object>();

        private void Awake()
        {
            LoadConfigurationFile();
        }

        /// <summary>
        /// Load Services 
        /// </summary>
        private void Start()
        {
            Messenger.ListenTo(Notifications.AppReady, HandleAppReady);
            Messenger.ListenTo(Notifications.AppExit, HandleAppExit);
            RegisterServices();
        }

        /// <summary>
        /// Load configuration file
        /// </summary>
        private void LoadConfigurationFile()
        {
            Debug.Log(string.Format("<color=green>* Loading configuration file</color>"));
            TextAsset asset = Resources.Load("config") as TextAsset;
            if (asset)
            {
                config.Clear();
                config = JsonConvert.DeserializeObject<Dictionary<string, object>>(asset.text);
            }
            Debug.Log(string.Format("<color=green>* Configuration loaded</color>"));
        }

        /// <summary>
        /// Registers service necessary for game, in order
        /// </summary>
        private void RegisterServices()
        {
            Debug.Log(string.Format("<color=green>* Registering services</color>"));
            ServiceLocator.Register(new List<object>
            {
                //Register Local Storage
                new LocalStorage(this),
                //Register HttpService
                new HttpService(this) 
            }, (services) =>
            {
                var list = services.Keys.ToArray();
                Debug.Log(string.Format("<color=green>* Services registers {0}</color>", string.Join(",", list)));
                Messenger.SendNote(Notifications.ServicesReady);
                Messenger.SendNote(Notifications.AppReady);
            });            
        }

        /// <summary>
        /// Ready to initiate application.
        /// At this point, all necessary core components of the application, such as 
        /// configuration files, services, etct... have been loaded. 
        /// </summary>
        /// <param name="obj"></param>
        private void HandleAppReady(object obj)
        {
            Debug.Log(string.Format("<color=green>* Application Ready</color>"));
        }

        private void HandleAppExit(object obj)
        {
            Debug.Log(string.Format("<color=green>* Application Excited</color>"));
        }
    }
}
