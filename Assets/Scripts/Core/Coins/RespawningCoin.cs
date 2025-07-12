using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> onCollected;
    private Vector3 previousPostion;

    private void Update()
    {
        if (previousPostion != transform.position) 
        { 
            Show(true);
        }

        previousPostion = transform.position;
    }
    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }

        if (alreadyCollected)
        {
            return 0;
        }
        
        alreadyCollected = true;

        onCollected?.Invoke(this);

        return coinValue;
    }

    public void Reset()
    {
       alreadyCollected=false; 
    }


}
