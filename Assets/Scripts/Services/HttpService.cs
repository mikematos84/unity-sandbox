using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace HeavyDev
{
    public class HttpService
    {
        protected MonoBehaviour mono;
        protected string baseUrl;

        protected LocalStorage localStorage;
        
        public HttpService(MonoBehaviour mono)
        {
            this.mono = mono;
            Messenger.ListenTo(Notifications.ServicesReady, HandleServicesReady);
        }

        private void HandleServicesReady(object o)
        {
            baseUrl = (string)App.config["base_url"];
            Debug.Log(String.Format("[HttpService] Base Url => {0}", baseUrl));
            localStorage = ServiceLocator.Find<LocalStorage>();
            Messenger.StopListeningTo(Notifications.ServicesReady, HandleServicesReady);
        }

        public void UpdateTokens(HTTPResponse resp)
        {
            if (resp.Headers.ContainsKey("set-authorization") == false && resp.Headers.ContainsKey("set-refresh") == false)
                return;

            localStorage.SetItems(new Dictionary<string, object> {
                { "access_token", resp.Headers["set-authorization"][0] },
                { "refresh_token", resp.Headers["set-refresh"][0] }
            });
            Debug.Log("[HttpService] Updated Tokens");
        }

        public HTTPRequest Request(string url, string data, HTTPMethods method, Action<HTTPRequest, HTTPResponse> callback)
        {
            var http = new HTTPRequest(new Uri(baseUrl + url), method, (req, resp) =>
            {
                // Update/refresh tokens if found in header
                UpdateTokens(resp);

                // send callback
                callback(req, resp);
            });

            // set Json Web Token header information to allow access
            http.SetHeader("Authorization", "Bearer " + localStorage.GetItem<string>("access_token"));
            http.SetHeader("Refresh-Token", localStorage.GetItem<string>("refresh_token"));
            http.SetHeader("Content-Type", "application/json");

            // send serialized json data if found
            if (data != null)
            {
                http.RawData = Encoding.UTF8.GetBytes(data);
                Debug.Log("Passing Data => " + data);
            }

            return http;
        }

        public HTTPRequest Login(string username, string password, Action<HTTPRequest, HTTPResponse> callback)
        {
            var http = new HTTPRequest(new Uri(baseUrl + "/login"), HTTPMethods.Post, (req, resp) =>
            {
                // Read response code
                if (resp.StatusCode == 200)
                {
                    // deserialize the Json Web Token information returned
                    JWTResponse jwt = JsonConvert.DeserializeObject<JWTResponse>(resp.DataAsText);

                    // save the access token and refresh token
                    localStorage.SetItems(new Dictionary<string, object> {
                        { "access_token", jwt.access_token },
                        { "refresh_token", jwt.refresh_token }
                    });
                }

                // send callback
                callback(req, resp);
            });
            http.AddField("username", username);
            http.AddField("password", password);
            return http;
        }

        public HTTPRequest Post(string url, string data, Action<HTTPRequest, HTTPResponse> callback)
        {
            return Request(url, data, HTTPMethods.Post, callback);
        }

        public HTTPRequest Get(string url, Action<HTTPRequest, HTTPResponse> callback)
        {
            return Request(url, null, HTTPMethods.Get, callback);
        }

        public HTTPRequest Delete(string url, Action<HTTPRequest, HTTPResponse> callback)
        {
            return Request(url, null, HTTPMethods.Delete, callback);
        }

        public HTTPRequest Put(string url, string data, Action<HTTPRequest, HTTPResponse> callback)
        {
            return Request(url, null, HTTPMethods.Put, callback);
        }
    }

    public class JWTResponse
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string not_before { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
