using BestHTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogSystem : MonoBehaviour
{
    public TextMeshProUGUI npcText;
    public Transform content;
    public Button buttonPrefab;

    Convo convo;

    // Start is called before the first frame update
    void Start()
    {
        string filePath = Application.streamingAssetsPath + "/dialog.json";
        new HTTPRequest(new Uri(filePath), HTTPMethods.Get, (req, resp) =>
        {
            Debug.Log(string.Format("<color=blue>{0}</color>", resp.StatusCode));
            if (resp.StatusCode == 200)
            {
                convo = JsonConvert.DeserializeObject<Convo>(resp.DataAsText);
                Debug.Log(string.Format("<color=green>Convo loaded</color>"));
                Initialize();
            }
            else
            {
                Debug.Log(string.Format("<color=yellow>Convo not found</color>"));
            }
        }).Send();
    }

    void Initialize()
    {
        SetQuestion();
    }

    void SetQuestion(int dialogId)
    {
        Dialog dialog = convo.dialogs.Where(x => x.id == dialogId).First();
        npcText.text = dialog.text;
        SetOptions(dialog.id);
    }
    void SetQuestion() { SetQuestion(1); }

    void RemoveOptions()
    {
        Button[] buttons = content.GetComponentsInChildren<Button>();
        if (buttons.Length > 0)
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }
        }
    }

    void SetOptions(int dialogId)
    {
        RemoveOptions();
        convo.responses.Where(x => x.dialogId == dialogId).ToList().ForEach(resp =>
        {
            Button option = Instantiate<Button>(buttonPrefab, content);
            option.GetComponentInChildren<TextMeshProUGUI>().text = resp.text;
            option.onClick.AddListener(() =>
            {
                SetQuestion(resp.nextDialogId);
            });
        });
    }
}

public class Dialog
{
    public int id { get; set; }
    public int orderId { get; set; }
    public string npcRef { get; set; }
    public string text { get; set; }
}

public class Response
{
    public int dialogId { get; set; }
    public string text { get; set; }
    public int points { get; set; }
    public int nextDialogId { get; set; }
}

public class Convo
{
    public List<Dialog> dialogs { get; set; }
    public List<Response> responses { get; set; }
}