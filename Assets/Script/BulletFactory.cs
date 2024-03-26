using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [SerializeField] private Bullet[] bullets;
    private Dictionary<string, Bullet> bulletsByName;

    private void Awake()
    {
        bulletsByName = new Dictionary<string, Bullet>();
        foreach (var bullet in bullets)
        {
            bulletsByName.Add(bullet.bulletName, bullet);
        }
    }

    public Bullet CreateBullet(string bulletName, ulong netId, Vector2 bulletPos, string shipName)
    {
        GameObject shipObject = NetworkManager.Singleton.ConnectedClients[netId].PlayerObject.GetComponent<PlayerNetScript>().shipObject;
        if (bulletsByName.TryGetValue(bulletName, out Bullet bulletPrefab))
        {
            Bullet bulletInstance = Instantiate(bulletPrefab, bulletPos, shipObject.transform.rotation);
            bulletInstance.GetComponent<Bullet>().bulletOwner = shipName;
            bulletInstance.GetComponent<Bullet>().idOwner = netId;
            bulletInstance.GetComponent<NetworkObject>().Spawn();
            return bulletInstance;
        }
        else
        {
            Debug.LogWarning($"Bullet doesnt exist");
            return null;
        }
    }
}
