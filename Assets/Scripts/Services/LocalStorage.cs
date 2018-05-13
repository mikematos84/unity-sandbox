using System.Collections.Generic;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HeavyDev
{
    public class LocalStorage
    {
        protected static readonly Regex pattern = new Regex("[ -]");
        protected static readonly string evaluator = "_";

        protected MonoBehaviour mono;
        protected ES2Settings settings;

        private string file = String.Format("{0}.es2", pattern.Replace(Application.productName, evaluator));

        public bool isLoaded = false;
        public DateTime lastSave;
        public event Action OnLoadSuccess;
        public event Action OnLoadError;

        private Dictionary<string, object> dict = new Dictionary<string, object>();

        public LocalStorage(MonoBehaviour mono)
        {
            this.mono = mono;
            Messenger.ListenTo(Notifications.ServicesReady, HandleServicesReady);
            ConfigureSettings();
        }

        private void HandleLoadSuccess()
        {
            Debug.Log(String.Format("<color=yellow>{0}</color>", String.Join(", ", dict.Keys.ToArray<string>())));
        }

        public void HandleServicesReady(object o)
        {
            Messenger.StopListeningTo(Notifications.ServicesReady, HandleServicesReady);
        }

        /// <summary>
        /// Sanitizing incoming tag to return lowercase version with spaces " "
        /// and dashes "-" replaced by evaluator
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string SanitizeTag(string tag)
        {
            return pattern.Replace(tag, evaluator).ToLower();
        }
        /// <summary>
        /// Handles ES2 Configuration
        /// </summary>
        private void ConfigureSettings()
        {
            settings = new ES2Settings(file)
            {
                tag = SanitizeTag("AppData"),
                encrypt = true,
                encryptionType = ES2Settings.EncryptionType.AES128,
                encryptionPassword = "password"
            };
        }

        public void Load()
        {
            if (ES2.Exists(file))
            {
                ES2Data es2data = ES2.LoadAll(file, settings);
                dict = es2data.LoadDictionary<string, object>(settings.tag);
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
            var key = SanitizeTag("Last Save");
            lastSave = DateTime.Now;
            dict[key] = lastSave;
            ES2.Save(dict, file, settings);
            Debug.Log(String.Format("<color=green>LocalStorage Saved : <b>{0}</b></color>", lastSave));
        }

        public void Delete(string file = null)
        {
            if (file != null)
                this.file = file;

            if (ES2.Exists(this.file))
                ES2.Delete(this.file);
        }

        public T GetItem<T>(string key)
        {
            key = SanitizeTag(key);
            if (dict.ContainsKey(key))
                return (T)dict[key];

            return default(T);
        }

        public object GetItem(string key)
        {
            key = SanitizeTag(key);
            if (dict.ContainsKey(key))
                return dict[key];

            return default(object);
        }

        public void SetItem(string key, object value)
        {
            key = SanitizeTag(key);
            dict[key] = value;
            Save();
        }

        public void SetItems(Dictionary<string, object> items)
        {
            items.ToList().ForEach((kvp) =>
            {
                var key = SanitizeTag(kvp.Key);
                dict[key] = kvp.Value;
            });
            Save();
        }

        public void RemoveItem(string key)
        {
            key = SanitizeTag(key);
            if (!dict.ContainsKey(key))
                return;

            dict.Remove(key);
            Save();
        }

        public void RemoveItems(string[] keys)
        {
            bool atLeastOne = false;
            keys.ToList().ForEach((key) =>
            {
                key = SanitizeTag(key);
                if (dict.ContainsKey(key))
                {
                    atLeastOne = true;
                    dict.Remove(key);
                }
            });

            if (atLeastOne)
                Save();
        }
    }
}
