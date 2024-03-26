using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetFunctionsScript : NetworkBehaviour
{
    public static NetFunctionsScript Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    #region ServerRPC Functions
    [ServerRpc(RequireOwnership = false)]
    public void GotoSceneServerRPC(string sceneName)
    {
        var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load {sceneName} " +
                  $"with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShopServerRpc(string typeBullet, Vector2 bulletPos, ulong netId, string shipName)
    {
        GameObject.FindWithTag("BulletFactory").GetComponent<BulletFactory>().CreateBullet(
            typeBullet,
            netId,
            bulletPos,
            shipName);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActivateUltiServerRpc(ulong playerId)
    {
        UltiManager.Instance.SupportUlti();
    }
    #endregion

    #region ClientRPC Functions
    [ClientRpc]
    public void NewPlayerClientRPC(int amount)
    {
        PlayerNetScript playerNet = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerNetScript>();
        playerNet.amountPlayers = amount;
    }

    [ClientRpc]
    public void AddUltiPointClientRPC(ulong idOwner, int points)
    {
        PlayerNetScript playerNet = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerNetScript>();
        if (playerNet.OwnerClientId == idOwner)
        {
            if (playerNet.ulti.Value < 10)
            {
                playerNet.ulti.Value += points;
            }
        }
    }
    #endregion
}
