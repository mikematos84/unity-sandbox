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
            JsonWebToken jwt = JsonConvert.DeserializeObject<JsonWebToken>(resp.DataAsText);
            accessToken = jwt.access_token;
            refreshToken = jwt.refresh_token;

            string output = "Login Success\n\nAccess Token: {0}\n\nRefresh Token:{1}";
            outputField.text = string.Format(output, accessToken, refreshToken);
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
            HTTPResponse httpResponse = JsonConvert.DeserializeObject<HTTPResponse>(resp.DataAsText);
            outputField.text = "";
            foreach (Body item in httpResponse.body)
            {
                string output = "Id:{0} \nLogin Id:{1} \nPassword:{2}";
                outputField.text += string.Format(output,
                    item.id,
                    item.login_id,
                    item.password
                );
            }
        });
        http.AddHeader("Authorization", "Bearer " + accessToken);
        http.AddHeader("Refresh-Token", refreshToken);
        http.Send();
    }
}

public class JsonWebToken
{
    public string access_token;
    public string refresh_token;
}

public class Body
{
    public string id { get; set; }
    public string login_id { get; set; }
    public string password { get; set; }
}

public class HTTPResponse
{
    public int status_code { get; set; }
    public List<Body> body { get; set; }
}

