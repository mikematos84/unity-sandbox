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
        private readonly string filePath = "tfsave.txt";
        private readonly bool encrypt = true;
        private readonly string password = "dcil";
        private readonly string tag = "TFM";
        private string file
        {
            get
            {
                return filePath + "?tag=" + tag + "&encrypt=" + encrypt.ToString().ToLower() + "&password=" + password;
            }
        }

        public bool isLoaded = false;
        public event Action OnLoadSuccess;
        public event Action OnLoadFailed;

        private Dictionary<string, object> data = new Dictionary<string, object>();

        public LocalStorage()
        {
            Messenger.ListenTo(Notifications.ServicesReady, HandleServicesReady);
        }

        public void HandleServicesReady(object o)
        {
            Messenger.StopListeningTo(Notifications.ServicesReady, HandleServicesReady);
        }

        public void Load()
        {
            if (ES2.Exists(file))
            {
                ES2Data info = ES2.LoadAll(file);
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
                if (OnLoadFailed != null)
                {
                    OnLoadFailed();
                }
            }
        }

        public void Save()
        {
            ES2.Save(data, file);
            Debug.Log("[LocalStorage] Saved");
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
