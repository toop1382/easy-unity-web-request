using UnityEditor;
using UnityEngine;

namespace WebRequestManager.Editor
{
    public class HttpConfig : EditorWindow
    {
        [MenuItem("Window/WebRequestManager/HttpConfig")]
        private static void ShowWindow()
        {
            var activeObject = Resources.Load("HttpConfig");
            Selection.activeObject = activeObject;
        }
    }
}