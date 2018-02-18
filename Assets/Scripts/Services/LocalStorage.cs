using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json;

namespace HeavyDev
{
    public class LocalStorage
    {
        protected MonoBehaviour mono;
        protected ES2Settings settings;
        protected readonly string tag = Application.productName.Replace(" ", "_");

        private string file
        {
            get
            {
                return String.Format("{0}.es", tag); 
            }
        }

        public bool isLoaded = false;
        public event Action OnLoadSuccess;
        public event Action OnLoadError;

        private Dictionary<string, object> data = new Dictionary<string, object>();

        public LocalStorage(MonoBehaviour mono)
        {
            this.mono = mono;
            Messenger.ListenTo(Notifications.ServicesReady, HandleServicesReady);
            ConfigureSettings();
            SetItem("Developer", "Mike Matos");
        }

        public void HandleServicesReady(object o)
        {
            Messenger.StopListeningTo(Notifications.ServicesReady, HandleServicesReady);
        }

        private void ConfigureSettings()
        {
            settings = new ES2Settings(file)
            {
                encrypt = true,
                encryptionPassword = "password"
            };
        }

        public void Load()
        {
            if (ES2.Exists(file))
            {
                ES2Data info = ES2.LoadAll(file, settings);
                data = info.LoadDictionary<string, object>(tag);
                isLoaded = true;

                if (OnLoadSuccess != null)
                {
                    OnLoadSuccess();
                }
            }
            else
            {
                isLoaded = false;
                if (OnLoadError != null)
                {
                    OnLoadError();
                }
            }
        }

        public void Save()
        {
            ES2.Save(data, file, settings);
            Debug.Log(String.Format("<color=green>* LocalStorage Saved</color>"));
        }

        public void Delete(string path = null)
        {
            if (path == null)
                path = file;

            if (ES2.Exists(path))
            {
                ES2.Delete(path);
            }
        }

        public T GetItem<T>(string key)
        {
            if (data.ContainsKey(key))
                return (T)data[key];

            return default(T);
        }

        public object GetItem(string key)
        {
            if (data.ContainsKey(key))
                return data[key];

            return default(object);
        }

        public void SetItem(string key, object value)
        {
            data[key] = value;
            Save();
        }

        public void SetItems(Dictionary<string, object> items)
        {
            foreach (KeyValuePair<string, object> kvp in items)
            {
                data[kvp.Key] = kvp.Value;
            }
            Save();
        }

        public void RemoveItem(string key)
        {
            if (!data.ContainsKey(key))
                return;

            data.Remove(key);
            Save();
        }

        public void RemoveItems(string[] key)
        {
            bool atLeastOne = false;
            for (int i = 0; i < key.Length; i++)
            {
                if (data.ContainsKey(key[i]))
                {
                    atLeastOne = true;
                    data.Remove(key[i]);
                }
            }

            if (atLeastOne)
                Save();
        }
    }
}
