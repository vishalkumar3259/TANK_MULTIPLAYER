using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

}
