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
    protected SceneLoader sceneLoader;
    
    [Header("Prefabs")]
    [SerializeField] SceneLoader m_SceneLoader;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        CheckCoreServices();
    }

    protected virtual void CheckCoreServices()
    {
        sceneLoader = ServiceLocator.Find<SceneLoader>();
        if (!sceneLoader)
        {
            ServiceLocator.Register<SceneLoader>(Instantiate(m_SceneLoader));
            sceneLoader = ServiceLocator.Find<SceneLoader>();
            sceneLoader.FadeOut();
            Debug.Log("Registered missing service");
        }

        LoadSceneData();
    }

    protected virtual void LoadSceneData()
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

    protected virtual void Initialize()
    {
        JToken levels = profile["gameData"]["levels"];
        levels.ToList().ForEach(level =>
        {
            Building building = GameObject.Find((string)level["name"]).GetComponent<Building>();
            building.isLocked = (bool)level["isLocked"];
        });
    }
}
