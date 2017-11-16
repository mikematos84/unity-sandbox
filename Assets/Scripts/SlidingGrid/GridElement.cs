using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridElement : MonoBehaviour
{
    [SerializeField] Text txtField;
    [SerializeField] Button buttonTrue;
    [SerializeField] Button buttonFalse;

    public delegate void GridElementClickEvent (bool state, GridElement element);
    public event GridElementClickEvent OnClick;
    
    // Use this for initialization
    void Start()
    {
        buttonTrue.onClick.AddListener(() => OnClick(true, this));
        buttonFalse.onClick.AddListener(() => OnClick(false, this));
    }

    void HandleClick(bool state)
    {
        if (OnClick != null)
            OnClick(state, this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMessage(string str)
    {
        txtField.text = str;
    }
}
