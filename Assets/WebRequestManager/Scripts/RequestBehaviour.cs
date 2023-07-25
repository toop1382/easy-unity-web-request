using System;
using UnityEngine;

namespace WebRequestManager.Scripts
{
    public class RequestBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}