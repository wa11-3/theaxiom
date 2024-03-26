using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public GameObject[] ships;
    public Vector3[] shipsPos;

    [SerializeField]
    int playersCounter;

    [SerializeField]
    GameObject enemyPool;
    [SerializeField]
    GameObject bulletFactory;

    public int numberWave;
    public GameObject waveManager;

    bool gameStarted = false;
    bool invokeCalled = false;

    public static GameManager Instance { get; private set; }
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

    private void Update()
    {
        if (IsHost && IsOwner)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (!invokeCalled)
                {
                    invokeCalled = true;
                    Invoke("SetGameStarted", 5);
                }

                if (gameStarted)
                {
                    bool allDeath = true;
                    foreach (var ship in GameObject.FindObjectsOfType<Ship>())
                    {
                        if (!ship.isDeath.Value)
                        {
                            allDeath = false;
                        }
                    }

                    if (allDeath)
                    {
                        NetFunctionsScript.Instance.GotoSceneServerRPC("Over");
                    }
                }
            }
        }
    }

    private void SetGameStarted()
    {
        gameStarted = true;
    }

    public void HostStarted()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
        playersCounter = 1;
        PlayerNetScript playerNet = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerNetScript>();
        playerNet.amountPlayers = playersCounter;
    }

    public void HostShuntdown()
    {

    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        if (sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (IsHost && IsOwner)
                {
                    foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                    {
                        int shipSelected = client.PlayerObject.GetComponent<PlayerNetScript>().shipSelected.Value;
                        GameObject newShip = Instantiate(
                            ships[shipSelected],
                            shipsPos[shipSelected],
                            Quaternion.identity);
                        newShip.GetComponent<NetworkObject>().SpawnWithOwnership(client.ClientId);
                        client.PlayerObject.GetComponent<PlayerNetScript>().shipObject = newShip;
                    }
                    GameObject enemyPoolObject = Instantiate(enemyPool);
                    enemyPoolObject.GetComponent<NetworkObject>().Spawn();
                    Instantiate(bulletFactory);
                    Instantiate(waveManager);
                }
            }
            else if (SceneManager.GetActiveScene().name == "GameOver" && IsHost)
            {
                //Eliminar y resetear todas las variables
            }
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        playersCounter += 1;
        NetFunctionsScript.Instance.NewPlayerClientRPC(playersCounter);
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {

    }
}
