using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Manager
{
    private readonly RequestBehaviour requestBehaviour;
    private readonly Dictionary<string, string> headers;
    private readonly int timout;
    private readonly List<Action<DownloadHandler>> defaultCompletes;
    private readonly List<Action<string, int>> defaultError;

    public Manager(RequestBehaviour requestBehaviour, Dictionary<string, string> headers, int timout,
        List<Action<DownloadHandler>> defaultCompletes, List<Action<string, int>> defaultError)
    {
        this.requestBehaviour = requestBehaviour;
        this.timout = timout;
        this.headers = new Dictionary<string, string>(headers);
        this.defaultCompletes = new List<Action<DownloadHandler>>(defaultCompletes);
        this.defaultError = new List<Action<string, int>>(defaultError);
    }

    public void Post(string url, string body, Action<DownloadHandler> onComplete,
        Action<string, int> onError, Dictionary<string, string> headers = null)
    {
        headers ??= new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        headers.Add("accept", "application/json");

        SendRequest(HttpMethod.Post, url, headers, body, onComplete, onError);
    }

    public void Get(string url, Action<DownloadHandler> onComplete,
        Action<string, int> onError, Dictionary<string, string> headers = null)
    {
        SendRequest(HttpMethod.Get, url, headers, null, onComplete, onError);
    }


    public void Put(string url, string body, Action<DownloadHandler> onComplete,
        Action<string, int> onError, Dictionary<string, string> headers = null)
    {
        SendRequest(HttpMethod.Put, url, headers, body, onComplete, onError);
    }

    public void GetTextureWithCache(string url, Action<Texture2D> onComplete, Action<string, int> onError,
        Dictionary<string, string> headers = null)
    {
        if (onError == null) throw new ArgumentNullException(nameof(onError));
        var path = Application.persistentDataPath + '/' + url.Split('/')[^1];
        if (File.Exists(path))
        {
            onComplete?.Invoke(ConvertToTexture(path));
            return;
        }

        GetTexture(url, texture2D =>
        {
            onComplete?.Invoke(texture2D);
            SaveImage(texture2D, path);
        }, onError, headers);
    }

    public void GetTexture(string url, Action<Texture2D> onComplete, Action<string, int> onError,
        Dictionary<string, string> headers = null)
    {
        SendRequest(HttpMethod.Texture, url, headers, null,
            (downloadHandler) => onComplete?.Invoke(((DownloadHandlerTexture)downloadHandler).texture), onError);
    }

    private static Texture2D ConvertToTexture(string path)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            texture.Apply();
            return texture;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static void SaveImage(Texture2D image, string filename)
    {
        string savePath = Application.persistentDataPath;
        try
        {
            // check if directory exists, if not create it
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            File.WriteAllBytes(savePath + "/" + filename, image.EncodeToPNG());
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void SendRequest(HttpMethod method, string url, Dictionary<string, string> headers, string body,
        Action<DownloadHandler> onComplete, Action<string, int> onError)
    {
        requestBehaviour.StartCoroutine(RequestAction(method, url, headers, body, onComplete, onError));
    }

    private IEnumerator RequestAction(HttpMethod method, string url, Dictionary<string, string> headers, string body,
        Action<DownloadHandler> onComplete, Action<string, int> onError)
    {
        var webRequest = CreateUnityWebRequest(method, url, body);

        webRequest.timeout = timout;
        headers ??= new Dictionary<string, string>();
        headers = this.headers.Concat(headers).ToDictionary(s => s.Key, s => s.Value);
        foreach (var (key, value) in headers)
        {
            webRequest.SetRequestHeader(key, value);
        }

        if (body != null)
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(body);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        }

        if (onComplete != null)
        {
            this.defaultCompletes.Add(onComplete);
        }

        if (onError != null)
        {
            defaultError.Add(onError);
        }


        yield return webRequest.SendWebRequest();

        CheckResult(webRequest);
    }

    private static UnityWebRequest CreateUnityWebRequest(HttpMethod method, string url, string body)
    {
        UnityWebRequest webRequest;
        switch (method)
        {
            case HttpMethod.Put:
                webRequest = UnityWebRequest.Put(url, body);
                break;
            case HttpMethod.Get:
                webRequest = UnityWebRequest.Get(url);
                break;
            case HttpMethod.Post:
                webRequest = new
                    UnityWebRequest(url, "Post");
                break;
            case HttpMethod.Delete:
                webRequest = UnityWebRequest.Delete(url);
                break;
            case HttpMethod.Texture:
                webRequest = UnityWebRequestTexture.GetTexture(url);
                break;
            default:
                throw new InvalidEnumArgumentException();
        }

        return webRequest;
    }

    private void CheckResult(UnityWebRequest webRequest)
    {
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            foreach (var action in defaultCompletes)
            {
                action?.Invoke(webRequest.downloadHandler);
            }
        }
        else
        {
            foreach (var action in defaultError)
            {
                action?.Invoke(webRequest.error, (int)webRequest.responseCode);
            }
        }
    }


    public enum HttpMethod
    {
        Put,
        Get,
        Post,
        Delete,
        Texture
    }
}