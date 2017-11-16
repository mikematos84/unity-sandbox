using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DentedPixel;
using UnityEditor;

[CustomEditor(typeof(Carousel))]
public class CarouselEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Carousel script = (Carousel)target;

        if (GUILayout.Button("Update"))
        {
            script.UpdateGridLayout();
        }
    }
}

[ExecuteInEditMode]
public class Carousel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform layoutGroup;
    public RectTransform center;
    [HideInInspector]
    public RectTransform[] items;
    public int index;
    private float[] distance;
    public float margin;

    private bool dragging = false;
    public int itemDistance;

    // Use this for initialization
    IEnumerator Start()
    {
        distance = new float[items.Length];
        index = 0;

        yield return new WaitForSeconds(2f);
        //MoveToItem(index);
    }

    void Update()
    {
        UpdateGridLayout();
    }

    public void UpdateGridLayout()
    {
        RectTransform[] temp = layoutGroup.GetComponentsInChildren<RectTransform>();
        if(temp.Length > 0)
        { 
            var list = items.ToList();
            list.Clear();
            foreach (RectTransform t in temp)
            {
                if (t.transform != layoutGroup.transform)
                {
                    list.Add(t);
                }
            }
            items = list.ToArray<RectTransform>();

            if (items.Length > 1)
                itemDistance = (int)Mathf.Abs(items[1].anchoredPosition.x - items[0].anchoredPosition.x);

            for (int i = 1; i < items.Length; i++)
            {
                float width = items[i].rect.width;
                items[i].transform.localPosition = new Vector2((i * width) + (i * margin), items[i].transform.localPosition.y);
            }
        }
    }

    private int IndexOfClosestItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.x - items[i].transform.position.x);
        }

        float minDistance = Mathf.Min(distance);

        for (int a = 0; a < items.Length; a++)
        {
            if (minDistance == distance[a])
            {
                return a;
            }
        }

        return -1;
    }

    private LTDescr MoveToItem(int index, float time = .25f)
    {
        if (index > items.Length - 1 || items.Length < 2)
            return null;

        return LeanTween
            .value(layoutGroup.anchoredPosition.x, index * -itemDistance, time)
            .setOnUpdate((float value) =>
            {
                Vector2 newPosition = new Vector2(value, layoutGroup.anchoredPosition.y);
                layoutGroup.anchoredPosition = newPosition;
            })
            .setOnStart(() =>
            {
                GetComponent<ScrollRect>().enabled = false;
            })
            .setOnComplete(() =>
            {
                this.index = index;
                GetComponent<ScrollRect>().enabled = true;
            })
            .setEase(LeanTweenType.easeOutBack);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        index = IndexOfClosestItem();
        MoveToItem(index);        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancelAll();
    }
}
