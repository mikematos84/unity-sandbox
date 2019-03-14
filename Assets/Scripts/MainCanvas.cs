using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HeavyDev;
using UnityEngine.SceneManagement;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] App app;
    [SerializeField] Button homeButton;

    // Start is called before the first frame update
    void Start()
    {
        homeButton.onClick.AddListener(HandleHomeButtonClicked);
        app = GetComponentInParent<App>();
    }

    void HandleHomeButtonClicked()
    {
        //app = ServiceLocator.Find<App>();
        Debug.Log("App: " + app);
        app.sceneLabel.text = app.name;
        app.LoadScene("Main");
    }
}
