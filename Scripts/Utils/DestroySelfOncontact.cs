using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class DestroySelfOncontact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}
