using System;
using System.Collections.Generic;
using UnityEngine;

namespace WebRequestManager.Scripts
{
    public class HttpConfig : ScriptableObject
    {
        public List<Header> Headers;
        public int Timout=10;
    }

    [Serializable]
    public class Header
    {
        public string Key;
        public string Value;
    }
}