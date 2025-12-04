using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private void LaunchInMode(bool isDedicatedServer)
    {
        if( isDedicatedServer)
        {

        }
        else
        {

        }
    }

    
}
