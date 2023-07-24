using System.Collections.Generic;
using UnityEngine;
using WebRequestManager.Scripts;


public class Test : MonoBehaviour
{
    private void Start()
    {
        Http.AddDefaultHeader("App-Version", Application.version);
        Http.AddDefaultHeader("Device-Id", SystemInfo.deviceUniqueIdentifier);
        Http.AddDefaultError((s, y) => Debug.Log(s));
    }

    public void SetAction()
    {
        Http.Get("http://worldtimeapi.org/api/timezone/Europe/Helsinki", (s) => { Debug.Log(s.text); });
    }
}