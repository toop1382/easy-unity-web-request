using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using WebRequestManager.Scripts;
namespace WebRequestManager.Scripts
{
    public static class Http
    {
        private static RequestBehaviour requestBehaviour;
        private static HttpConfig config;

        private static List<Header> headers = new();

        private static List<Action<DownloadHandler>> defaultComplete = new();
        private static List<Action<string, int>> defaultError = new();

        static Http()
        {
            InitGameObject();
            GetConfig();
        }

        private static void GetConfig()
        {
            config = Resources.Load<HttpConfig>("HttpConfig");
        }

        public static void AddDefaultHeader(string key, string value)
        {
            headers.Add(new Header() { Key = key, Value = value });
        }

        public static void AddDefaultError(Action<string, int> onError)
        {
            defaultError.Add(onError);
        }

        public static void RemoveDefaultError(Action<string, int> onError)
        {
            defaultError.Remove(onError);
        }

        public static void AddDefaultComplete(Action<DownloadHandler> onComplete)
        {
            defaultComplete.Add(onComplete);
        }

        public static void RemoveDefaultComplete(Action<DownloadHandler> onError)
        {
            defaultComplete.Remove(onError);
        }

        private static void InitGameObject()
        {
            GameObject gameObject = new GameObject("Http");
            requestBehaviour = gameObject.AddComponent<RequestBehaviour>();
        }

        public static void Post(string url, string body, Action<DownloadHandler> onComplete,
            Action<string, int> onError = null, Dictionary<string, string> header = null)
        {
            CreateManager().Post(url, body, onComplete, onError, header);
        }

        public static void Get(string url, Action<DownloadHandler> onComplete,
            Action<string, int> onError = null, Dictionary<string, string> headers = null)
        {
            CreateManager().Get(url, onComplete, onError, headers);
        }

        public static void Put(string url, string body, Action<DownloadHandler> onComplete,
            Action<string, int> onError = null, Dictionary<string, string> headers = null)
        {
            CreateManager().Put(url, body, onComplete, onError, headers);
        }


        public static void GetTexture(string url, Action<Texture2D> onComplete,
            Action<string, int> onError = null, Dictionary<string, string> headers = null)
        {
            CreateManager().GetTexture(url, onComplete, onError, headers);
        }

        public static void GetTextureWithCache(string url, Action<Texture2D> onComplete,
            Action<string, int> onError, Dictionary<string, string> headers = null)
        {
            CreateManager().GetTextureWithCache(url, onComplete, onError, headers);
        }

        private static Manager CreateManager()
        {
            var allHeaders = new List<Header>(headers);
            allHeaders.AddRange(config.Headers);
            var dictionary = allHeaders.ToDictionary(s => s.Key, s => s.Value);
            return new Manager(requestBehaviour, dictionary, config.Timout, defaultComplete, defaultError);
        }
    }
}