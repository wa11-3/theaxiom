using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class PlayerNetScript : NetworkBehaviour
{
    public NetworkVariable<int> shipSelected = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public int amountPlayers;

    private void Start()
    {
        NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        amountPlayers = 1;
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        if (sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted)
        {
            if (SceneManager.GetActiveScene().name == "Game" && IsOwner)
            {
                //Instanciar las Naves en ambos jugadores
            }
            else if (SceneManager.GetActiveScene().name == "GameOver" && IsHost)
            {
                //Eliminar y resetear todas las variables
            }
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        amountPlayers += 1;
        NewPlayerClientRPC(amountPlayers);
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {

    }

    #region ServerRPC Functions
    [ServerRpc]
    public void GotoSceneServerRPC(string sceneName)
    {
        var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load {"Game"} " +
                  $"with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }
    #endregion

    #region ClientRPC Functions
    [ClientRpc]
    public void NewPlayerClientRPC(int amount)
    {
        amountPlayers = amount;
    }
    #endregion
}
