using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class LinkInfo
{
    public string id;
    public string url;
}

public class MyTextMeshLinkHandler : MonoBehaviour, IPointerClickHandler
{
    TextMeshProUGUI textMeshPro;
    Canvas canvas;
    Camera camera;

    public List<LinkInfo> links;

    void Awake()
    {
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        canvas = gameObject.GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            camera = null;
        }
        else
        {
            camera = canvas.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, camera);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string id = linkInfo.GetLinkID();
            foreach (LinkInfo link in links)
            {
                if (link.id == id)
                {
                    Application.OpenURL(link.url);
                }
            }
        }
    }
}