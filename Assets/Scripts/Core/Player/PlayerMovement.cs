using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;

    [Header("settings")]
    [SerializeField] private float movementSpeed = 4.0f;
    [SerializeField] private float turningRate = 30f;
    public Vector2 prevoiusMovmentInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        inputReader.MoveEvent += HandelMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }
        inputReader.MoveEvent += HandelMove;
    }

    public void HandelMove(Vector2 movementInput)
    {
        prevoiusMovmentInput = movementInput;
    }
    private void FixedUpdate()
    {
        if(! IsOwner) { return; }
        rb.velocity = (Vector2)bodyTransform.up * prevoiusMovmentInput.y * movementSpeed;
    }

    private void Update()
    {
        if (!IsOwner) { return; }
        float zRotation = prevoiusMovmentInput.x * -turningRate * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f,zRotation );
    }
}
