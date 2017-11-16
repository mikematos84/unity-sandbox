using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Http : MonoBehaviour
{

    public InputField loginField;
    public InputField passwordField;
    public Text outputField;
    public Button loginButton;
    public Button logoutButton;
    public Button listUsersButton;
    public string accessToken;
    public string refreshToken;


    private string url = "https://api.localhost";

    // Use this for initialization
    void Start()
    {
        loginField.text = "user@domain.com";
        passwordField.text = "password123";

        loginButton.onClick.AddListener(HandleLoginClick);
        logoutButton.onClick.AddListener(HandleLogoutClick);
        listUsersButton.onClick.AddListener(HandleListUsersClick);
    }

    void HandleLoginClick()
    {
        HTTPRequest http = new HTTPRequest(new Uri(url + "/login"), HTTPMethods.Post, (req, resp) =>
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(resp.DataAsText);
            accessToken = (string)o["access_token"];
            refreshToken = (string)o["refresh_token"];

            outputField.text = "Login Success\n\nAccess Token: " + accessToken + "\n\nRefresh Token:" + refreshToken;
        });
        http.AddField("login_id", loginField.text);
        http.AddField("password", passwordField.text);
        http.Send();
    }

    void HandleLogoutClick()
    {
        accessToken = null;
        outputField.text = "";
    }

    void HandleListUsersClick()
    {
        HTTPRequest http = new HTTPRequest(new Uri(url + "/users"), HTTPMethods.Get, (req, resp) =>
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(resp.DataAsText);
            outputField.text = resp.DataAsText;

            foreach(KeyValuePair<string, List<string>> kvp in resp.Headers)
            {
                Debug.Log(kvp.Key);
            }
        });
        http.AddHeader("Authorization", "Bearer " + accessToken);
        http.AddHeader("Refresh-Token", refreshToken);
        http.Send();
    }
}
