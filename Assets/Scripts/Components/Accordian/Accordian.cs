using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accordian : MonoBehaviour {

    [SerializeField]
    GameObject container;
    [SerializeField]
    ListItemRow prefab;
    List<ListItemRow> items = new List<ListItemRow> { };
    
    string[] copy = new string[]
    {
        "This is what a short description looks like",
        "This is what a short description looks like\nThis is what a short description looks like",
        "This is what a short description looks like\nThis is what a short description looks like\nThis is what a short description looks like"
    };

	// Use this for initialization
	void Start () {
		for(int i=0; i<30; i++)
        {
            ListItemRow item = Instantiate(prefab, container.transform);
            item.title = String.Format("Faq {0}", i);
            item.text = String.Format("{0}", copy[UnityEngine.Random.Range(0, copy.Length)]);
            item.OnClicked += HandleOnClick;
            items.Add(item);
        }
	}

    private void HandleOnClick(ListItemRow obj)
    {   
        if(obj.isOpen)
        {
            obj.Hide();
            return;
        }

        foreach (ListItemRow item in items)
        {
            item.Hide();
        }
        obj.Show();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
