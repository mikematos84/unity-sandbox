using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeavyDev
{
    public class App : MonoBehaviour
    {
        public static Dictionary<string, object> config = new Dictionary<string, object>();

        private void Awake()
        {
            // load config file
            TextAsset asset = Resources.Load("config") as TextAsset;
            if (asset)
            {
                config.Clear();
                config = JsonConvert.DeserializeObject<Dictionary<string, object>>(asset.text);
            }
        }

        // Use this for initialization
        void Start()
        {
            
        }
        
        // Update is called once per frame
        void Update()
        {

        }
    }
}
