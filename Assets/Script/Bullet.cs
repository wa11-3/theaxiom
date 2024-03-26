using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public abstract string bulletName { get; }
    public abstract void Activate();

    [SerializeField] protected int speed;
    public string bulletOwner;
    public ulong idOwner;

    private void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Invoke("DestroyBullet", 3);
        }
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NetworkManager.Singleton.IsHost)
        {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            NetFunctionsScript.Instance.AddUltiPointClientRPC(idOwner, 1);
        }

        if (collision.gameObject.TryGetComponent(out Ship ship))
        {
            if (ship.shipName == bulletOwner)
            {
                //Debug.Log("Owner");
            }
            else
            {
                Destroy(gameObject);
                //gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
