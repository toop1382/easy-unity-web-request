using UnityEngine;
using UnityEngine.UI;
using WebRequestManager.Scripts;

namespace WebRequestManager.Sample
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;

        private void Start()
        {
            Http.AddDefaultHeader("App-Version", Application.version);
            Http.AddDefaultHeader("Device-Id", SystemInfo.deviceUniqueIdentifier);
            Http.AddDefaultError((errorText, errorCode) => Debug.Log(errorText));
            Http.AddDefaultComplete(handler => Debug.Log(handler.text));
        }

        public void SetAction()
        {
            Http.Post("https://*****.***", "body", (s) => { Debug.Log(s.text); });
            Http.Put("https://*****.***", "RequestBody", (downloadHandler) => Debug.Log(downloadHandler.text),
                (errorText, errorCode) => Debug.Log(errorText + errorCode));
            Http.GetTextureWithCache("https://*****.***", (texture2D => rawImage.texture = texture2D),
                (errorText, errorCode) => Debug.Log(errorText + errorCode));
        }
    }
}