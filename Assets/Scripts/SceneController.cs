using BestHTTP;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SceneController : MonoBehaviour
{
    JObject profile;    

    // Start is called before the first frame update
    void Start()
    {
        string filePath = Application.streamingAssetsPath + "/profile.json";
        new HTTPRequest(new Uri(filePath), HTTPMethods.Get, (req, resp) =>
        {
            Debug.Log(string.Format("<color=blue>{0}</color>", resp.StatusCode));
            if (resp.StatusCode == 200)
            {
                profile = JsonConvert.DeserializeObject<JObject>(resp.DataAsText);
                Debug.Log(string.Format("<color=green>Profile loaded</color>"));
                Initialize();
            }
            else
            {
                Debug.Log(string.Format("<color=yellow>Profile not found</color>"));
            }
        }).Send();
    }

    void Initialize()
    {
        JToken levels = profile["gameData"]["levels"];
        levels.ToList().ForEach(level =>
        {
            Building building = GameObject.Find((string)level["name"]).GetComponent<Building>();
            building.isLocked = (bool)level["isLocked"];
        });
    }
}
