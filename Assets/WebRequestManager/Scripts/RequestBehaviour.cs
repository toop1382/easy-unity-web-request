using System;
using UnityEngine;

public class RequestBehaviour : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}