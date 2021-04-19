using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DentedPixel;

public class SceneLoader : MonoBehaviour
{

    public string loadedScene;
    public CanvasFader canvasFader;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void Awake()
    {
        canvasFader = GetComponentInChildren<CanvasFader>();
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        loadedScene = scene.name;
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
        LTSeq seq = LeanTween.sequence();

        if (string.IsNullOrEmpty(loadedScene) == false && loadedScene != "App")
        {
            seq.append(canvasFader.FadeIn());
            seq.append(() => StartCoroutine(UnloadAsync()));
        }

        seq.append(() => StartCoroutine(LoadAsync(sceneName)));
        seq.append(canvasFader.FadeOut());

        return seq.tween;
    }

    public LTDescr FadeIn() { return canvasFader.FadeIn(); }
    public LTDescr FadeOut() { return canvasFader.FadeOut(); }
}
