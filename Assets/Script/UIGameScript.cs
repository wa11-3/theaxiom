using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScript : MonoBehaviour
{
    public Image healthBar;
    public Image healthStatus;
    public Image shieldStatus;
    public Image ultiStatus;
    int totalHealth;
    PlayerNetScript playerNet;

    public GameObject[] SpawnPoints;

    private void Start()
    {
        playerNet = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetScript>();
        totalHealth = GameManager.Instance.ships[playerNet.shipSelected.Value].GetComponent<Ship>().hltPoints;
        healthBar.rectTransform.sizeDelta = new Vector2(totalHealth * 100, healthBar.rectTransform.sizeDelta.y);
    }

    private void Update()
    {
        healthStatus.rectTransform.sizeDelta = new Vector2(playerNet.damage.Value * -100, healthStatus.rectTransform.sizeDelta.y);
        ultiStatus.rectTransform.sizeDelta = new Vector2((playerNet.ulti.Value * 100), ultiStatus.rectTransform.sizeDelta.y);
        //shieldStatus.rectTransform.sizeDelta = new Vector2((playerNet.shield.Value * 100), shieldStatus.rectTransform.sizeDelta.y);
    }
}
