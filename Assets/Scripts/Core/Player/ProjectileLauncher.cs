using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CoinWallet wallet;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject severProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzelflash;
    [SerializeField] private Collider2D playerCollider;

    [Header("Settings")]

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float muzzelFlashDuration;
    [SerializeField] private int costToFire;


    private bool shouldfire;
    private float timer;
    //private float PreviousFireTime;
    private float muzzelFlashTimer;

    public object Instanstiate { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        inputReader.PrimaryFireEvent += HandelPrimaryFire;
    }

    public void OnNetworkDesspawn()
    {
        if (!IsOwner) { return; }
        inputReader.PrimaryFireEvent -= HandelPrimaryFire;
    }

    private void Update()
    {
        if(muzzelFlashTimer > 0f)
        {
            muzzelFlashTimer -= Time.deltaTime;

            if(muzzelFlashTimer <= 0f)
            {
                muzzelflash.SetActive(false);
            }
        }
        if(!IsOwner) { return; }

        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (!shouldfire) { return; }

        if (timer > 0) { return; }

        if (wallet.TotalCoins.Value < costToFire) { return; }


        PrimaryFireserverRpc(projectileSpawnPoint.position,projectileSpawnPoint.up);

        SpawnDummyProjectile(projectileSpawnPoint.position,projectileSpawnPoint.up);

        timer = 1 / fireRate;
    }

    

    private void HandelPrimaryFire(bool shouldfire)
    {
        this.shouldfire = shouldfire;
    }

    [ServerRpc]
    private void PrimaryFireserverRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (wallet.TotalCoins.Value < costToFire) { return; }

        wallet.SpendCoins(costToFire);

        GameObject ProjectileInstance = Instantiate(
            severProjectilePrefab,
            spawnPos,
            Quaternion.identity);

        ProjectileInstance.transform.up = direction;

        Physics2D.IgnoreCollision(playerCollider, ProjectileInstance.GetComponent<Collider2D>());

        if(ProjectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }

        if (ProjectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }

        SpawnDummyProjectileClientRpc(spawnPos, direction);

    }

    [ClientRpc]

    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner)
        {
            return;
           
        }
        SpawnDummyProjectile(spawnPos, direction);
          
    }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
       muzzelflash.SetActive(true);
       muzzelFlashTimer = muzzelFlashDuration;
       GameObject ProjectileInstance = Instantiate(
           clientProjectilePrefab, 
           spawnPos, 
           Quaternion.identity);
       ProjectileInstance.transform.up = direction;

       Physics2D.IgnoreCollision(playerCollider, ProjectileInstance.GetComponent<Collider2D>());
       
       if(ProjectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }

}
