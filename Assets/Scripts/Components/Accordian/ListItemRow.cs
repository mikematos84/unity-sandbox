using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItemRow : MonoBehaviour {

    [SerializeField]
    Button button;
    [SerializeField]
    GameObject content;

    public event Action<ListItemRow> OnClicked;

	public string text {
        get {
            return content.GetComponent<Text>().text;
        }
        set {
            content.GetComponent<Text>().text = value;
        }
    }

    public string title
    {
        get
        {
            return button.GetComponentInChildren<Text>().text;
        }
        set
        {
            button.GetComponentInChildren<Text>().text = value;
        }
    }

    public bool isOpen
    {
        get
        {
            return content.activeSelf;
        }
    }

    // Use this for initialization
    void Start()
    {
        button.onClick.AddListener(HandleButtonClick);
    }

    void HandleButtonClick()
    {
        if(OnClicked != null)
        {
            OnClicked(this);
        }
    }

    public void Show()
    {
        content.SetActive(true);
    }

    public void Hide()
    {
        content.SetActive(false);
    }

    public void Toggle()
    {
        content.SetActive(!content.activeSelf);
    }
}
