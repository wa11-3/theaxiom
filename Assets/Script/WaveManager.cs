using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WaveManager : MonoBehaviour
{
    //[SerializeField] Vector3[] enemiesSpawn;
    [SerializeField] GameObject[] enemiesPrefabs;

    //public Dictionary<string, int> enemiesByName = new Dictionary<string, int>
    //{
    //    { "Wasp", 0 },
    //    { "Cockroach", 1},
    //    { "Drangofly", 2},
    //    { "Stingray", 3}
    //};

    //public enum enemiesByName { Wasp, Cockroach, Drangofly, Stingray }

    public GameObject[] SpawnPoints;

    int[][] wave = new int[][]
    {
        new int[] { 8, 0, 0, 0 }, //0
        new int[] { 16, 0, 0, 0 }, //1
        new int[] { 8, 4, 0, 0 }, //2
        new int[] { 16, 4, 0, 0 }, //3
        new int[] { 8, 8, 0, 0 }, //4
        new int[] { 16, 8, 0, 0 }, //5
        new int[] { 8, 4, 2, 0 }, //6
        new int[] { 16, 4, 2, 0 }, //7
        new int[] { 8, 8, 2, 0 }, //8
        new int[] { 16, 8, 2, 0 }, //9
        new int[] { 8, 8, 4, 0 }, //10
        new int[] { 16, 8, 4, 0 }, //11
        new int[] { 8, 4, 2, 1 }, //12
        new int[] { 16, 4, 2, 1 }, //13
        new int[] { 8, 8, 2, 1 }, //14
        new int[] { 16, 8, 2, 1 }, //15
        new int[] { 8, 4, 4, 1 }, //16
        new int[] { 16, 4, 4, 1 }, //17
        new int[] { 8, 8, 4, 1 }, //18
        new int[] { 16, 8, 4, 1 }, //19
        new int[] { 8, 4, 2, 2 }, //20
        new int[] { 16, 4, 2, 2 }, //21
        new int[] { 8, 8, 2, 2 }, //22
        new int[] { 16, 8, 2, 2 }, //23
        new int[] { 8, 4, 4, 2 }, //24
        new int[] { 16, 4, 4, 2 }, //25
        new int[] { 8, 8, 4, 2 }, //26
        new int[] { 16, 8, 4, 2 }, //27
    };

    public int totalEnemies;
    public bool inWave;

    public static WaveManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        Debug.Log("Starting Wave");
        inWave = false;
        SpawnPoints = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIGameScript>().SpawnPoints;
        StartCoroutine(StartingWave(GameManager.Instance.numberWave));
    }

    private void Update()
    {
        if (inWave && totalEnemies == 0)
        {
            inWave = false;
            GameManager.Instance.numberWave += 1;
            StartCoroutine(StartingWave(GameManager.Instance.numberWave));
        }
    }

    IEnumerator StartingWave(int numWave)
    {
        yield return new WaitForSeconds(5);
        int[] typeWave = wave[numWave];
        totalEnemies = 0;
        inWave = true;
        for (int i = 0; i < enemiesPrefabs.Count(); i++)
        {
            totalEnemies += typeWave[i];
            StartCoroutine(SpawnWave(i, typeWave[i]));

        }
    }

    IEnumerator SpawnWave(int enemyType, int enemyCount)
    {
        int n = 0;
        for (int i = 0; i < enemyCount; i++)
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                if (SpawnPoints[n].GetComponent<SpawnPointScript>().isBusy)
                {
                    n += 1;
                    if (n > SpawnPoints.Count() - 1)
                    {
                        n = 0;
                    }
                }
                else
                {
                    NetworkObject enemyTmp = NetworkObjectPool.Singleton.GetNetworkObject(enemiesPrefabs[enemyType], SpawnPoints[n].transform.position, Quaternion.identity);
                    enemyTmp.Spawn();
                    break;
                }
            }
        }
        yield return new WaitForSeconds(2);
    }
}
