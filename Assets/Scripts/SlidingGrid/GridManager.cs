using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DentedPixel;

public class GridManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] HorizontalLayoutGroup grid;
    [SerializeField] GridElement prefab;
    [SerializeField] GridElement[] elements;

    // Use this for initialization
    void Start()
    {
        InstantiateGridElements();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InstantiateGridElements(int count = 4)
    {
        if (prefab == null)
            return;

        var list = elements.ToList();
        for (int i = 0; i < count; i++)
        {
            GridElement element = Instantiate(prefab, grid.transform);
            element.name = "Grid Element " + (i + 1).ToString();
            element.SetMessage(element.name);

            // set element events
            element.OnClick += HandleClick;

            // add element to list
            list.Add(element);
        }
        elements = list.ToArray();
    }

    private void HandleClick(bool state, GridElement element)
    {
        int index = elements.ToList().IndexOf(element);
        if (index < elements.Length - 1)
            index++;
        else
            index = 0;

        if (state)
            SlideTo(elements[index]);
        else
            MoveToBack(element);
    }

    void MoveToBack(GridElement element)
    {
        var list = elements.ToList();
        int index = list.IndexOf(element);
        var temp = elements[index];
        var last = elements[elements.Length - 1];

        LeanTween
            .moveLocalX(element.gameObject, last.transform.localPosition.x + element.GetComponent<RectTransform>().rect.width, 1f)
            .setEase(LeanTweenType.easeInOutBack)
            .setOnComplete(() =>
            {
                list.RemoveAt(index);
                temp.transform.SetParent(transform);
                list.Add(temp);
                temp.transform.SetParent(grid.transform);
                elements = list.ToArray();
            });

        LeanTween
            .moveLocalX(elements[index + 1].gameObject, element.GetComponent<RectTransform>().rect.width, 1f)
            .setEase(LeanTweenType.easeInOutBack)
            .setOnComplete(() =>
            {
            });
    }

    LTDescr SlideTo(GridElement element)
    {
        RectTransform rt = prefab.GetComponent<RectTransform>();
        int index = elements.ToList().IndexOf(element);

        return LeanTween
            .moveLocalX(gameObject, -(rt.rect.width * index), 1f)
            .setEase(LeanTweenType.easeInOutBack);
    }
}
