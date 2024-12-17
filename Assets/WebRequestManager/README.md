# easy-unity-web-request

## Description
This is a simplified version of UnityWebRequest which can use Actions instead of Unity Coroutines.

## How to use
It has most of the methods that UnityWebRequest has such as Get, Post, Put and etc... which can be called statically using the HTTP class. each of these functions will take an Action which will be called upon the method's completetion.


## Usage sample
### Get method 
 ```cs 
 Http.Get("https://*****.***", (downloadHandler) =>Debug.Log(downloadHandler.text),(errorText,errorCode)=>Debug.Log(errorText+errorCode));
 ```
### Post method
 ```cs 
 Http.Post("https://*****.***", "RequestBody",(downloadHandler) =>Debug.Log(downloadHandler.text),(errorText,errorCode)=>Debug.Log(errorText+errorCode));
 ```
### Put method
 ```cs 
 Http.Put("https://*****.***", "RequestBody",(downloadHandler) =>Debug.Log(downloadHandler.text),(errorText,errorCode)=>Debug.Log(errorText+errorCode));
 ```
### GetTexture and GetTextureWithCache
 ```cs 
  Http.GetTexture("https://*****.***", (texture2D => rawImage.texture = texture2D),(errorText, errorCode) => Debug.Log(errorText + errorCode));
  Http.GetTextureWithCache("https://*****.***", (texture2D => rawImage.texture = texture2D),(errorText, errorCode) => Debug.Log(errorText + errorCode));
 ```
### AddDefaultHeader
This function add a header to all requests
```cs
  Http.AddDefaultHeader("App-Version", Application.version);
 ```
### AddDefaultError
This function add a OnError to all requests
 ```cs
  Http.AddDefaultError((errorText, errorCode) => Debug.Log(errorText));
 ```
### AddDefaultComlpete
This function add a OnComplete to all requests
 ```cs
  Http.AddDefaultComplete(handler => Debug.Log(handler.text) );
 ```
