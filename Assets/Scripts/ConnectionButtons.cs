using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void startClinet()
    {
        NetworkManager.Singleton.StartClient();
    }
}
