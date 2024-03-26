using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public abstract string enemyName { get; }

    public abstract int dmgPoints { get; }
    public abstract int spdPoints { get; }

    public GameObject sprite;

    public NavMeshAgent agent;

    bool _detectedTarget;
    public int wayPointIndex;
    GameObject shipTarget;

    private void Start()
    {
        _detectedTarget = false;
        wayPointIndex = Random.Range(0,3);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = spdPoints * 2.0f;
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsHost)
        {

            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            if (_detectedTarget)
            {
                if (shipTarget != null)
                {
                    sprite.transform.up = (transform.position - shipTarget.transform.position) * -1;
                    agent.SetDestination(shipTarget.transform.position);
                }
                else
                {
                    _detectedTarget = false;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, WaveManager.Instance.SpawnPoints[wayPointIndex].transform.position) <= 2)
                {
                    if (wayPointIndex == (WaveManager.Instance.SpawnPoints.Length - 1))
                    {
                        wayPointIndex = 0;
                    }
                    else
                    {
                        wayPointIndex++;
                    }
                }

                sprite.transform.up = (transform.position - WaveManager.Instance.SpawnPoints[wayPointIndex].transform.position) * -1;
                agent.SetDestination(WaveManager.Instance.SpawnPoints[wayPointIndex].transform.position);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
            //Debug.Log("Enemy");
            }
            else
            {
                WaveManager.Instance.totalEnemies -= 1;
                NetworkObject netObject = this.gameObject.GetComponent<NetworkObject>();
                netObject.Despawn();
            }
        }

        //if (collision.gameObject.TryGetComponent(out Bullet bullet))
        //{
        //    Debug.Log("Bullet");
        //}
        //else if (collision.gameObject.TryGetComponent(out Ship ship))
        //{
        //    Debug.Log("Ship");
        //}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            if (collision.gameObject.TryGetComponent(out Ship ship))
            {
                _detectedTarget = true;
                shipTarget = collision.gameObject;
            }
        }
    }
}
