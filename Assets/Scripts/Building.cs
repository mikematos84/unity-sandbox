using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using HeavyDev;

public class Building : MonoBehaviour
{
    App app;
    [SerializeField] string sceneToLoad;
    bool _isLocked = false;
    public bool isLocked {
        get {
            return _isLocked;
        }
        set {
            _isLocked = value;
            GetComponent<Renderer>().material.SetColor("_Color", _isLocked ? Color.red : Color.green);
        }
    }

    private Vector3 defaultPosition;
    private float time = .1f;
    private LeanTweenType easeType = LeanTweenType.easeInOutBack;

    // Start is called before the first frame update
    void Start()
    {
        app = ServiceLocator.Find<App>();
        defaultPosition = gameObject.transform.localPosition;
    }

    private void OnMouseEnter()
    {
        LeanTween.moveLocalY(gameObject, defaultPosition.y + .25f, time)
            .setEase(easeType);
    }

    private void OnMouseExit()
    {
        LeanTween.moveLocalY(gameObject, defaultPosition.y, time)
            .setEase(easeType);
    }

    private void OnMouseDown()
    {
        if(app != null && !isLocked)
            app.LoadScene(sceneToLoad);
    }
}
