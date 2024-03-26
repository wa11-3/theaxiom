using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UltiManager : MonoBehaviour
{
    [SerializeField]
    GameObject supportUlti;


    public static UltiManager Instance { get; private set; }
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


    public void SupportUlti()
    {
        GameObject supportShip = GameObject.FindObjectOfType<SupportShip>().gameObject;

        supportShip.GetComponent<SupportShip>().supportUlti.SetActive(true);

        //GameObject ultiTemp = Instantiate(supportUlti, supportShip.transform);
        //ultiTemp.GetComponent<NetworkObject>().Spawn();
        //StartCoroutine(SupportUltiDuration(ultiTemp));
    }

    IEnumerator SupportUltiDuration(GameObject ulti)
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(ulti);
    }
}
