using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SelectScript : MonoBehaviour
{
    public TMP_Text shipNameTX;
    public TMP_Text ipTX;
    public TMP_Text amountTX;

    public Image atkIM;
    public Image spdIM;
    public Image hltIM;
    public Image shipIM;
    public Image selectedIM;

    public Button nextBT;
    public Button prevBT;
    public Button selectBT;
    public Button readyBT;

    Ship[] ships =
    {
        new FighterShip(),
        new TankShip(),
        new SupportShip(),
        new GhostShip(),
    };
    public Sprite[] shipSprite;

    public int shipSelected = 0;

    private void Start()
    {
        ipTX.text = GetLocalIPAddress();
        SetShipInfo();
    }

    private void Update()
    {
        int amountPlayers = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetScript>().amountPlayers;
        amountTX.text = $"{amountPlayers}/4";
    }

    public void OnClickChangeButton(string buttonType)
    {
        if (buttonType == "prev" && shipSelected > 0)
        {
            shipSelected -= 1;
        }
        else if (buttonType == "next" && shipSelected < 3)
        {
            shipSelected += 1;
        }
        selectedIM.gameObject.SetActive(false);
        SetShipInfo();
    }

    public void OnClickSelectButton()
    {
        List<int> playerShips = new List<int> { };
        GameObject[] players = GameObject.FindGameObjectsWithTag("NetPlayer");
        foreach (GameObject player in players)
        {
            if (player.TryGetComponent<PlayerNetScript>(out PlayerNetScript playerNet))
            {
                playerShips.Add(playerNet.shipSelected.Value);
            }
        }
        if (playerShips.Contains(shipSelected))
        {
            selectedIM.gameObject.SetActive(true);
        }
        else
        {
            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetScript>().shipSelected.Value = shipSelected;
            nextBT.interactable = false;
            prevBT.interactable = false;
            selectBT.interactable = false;
            if (NetworkManager.Singleton.IsHost)
            {
                readyBT.gameObject.SetActive(true);
            }
        }
    }

    public void OnClickReadyButton()
    {
        PlayerNetScript playerNet = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetScript>();
        playerNet.GotoSceneServerRPC("Game");
    }

    string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    void SetShipInfo()
    {
        shipNameTX.text = ships[shipSelected].shipName;
        shipIM.sprite = shipSprite[shipSelected];
        atkIM.rectTransform.sizeDelta = new Vector2(ships[shipSelected].atkPoints, atkIM.rectTransform.sizeDelta.y);
        spdIM.rectTransform.sizeDelta = new Vector2(ships[shipSelected].spdPoints, atkIM.rectTransform.sizeDelta.y);
        hltIM.rectTransform.sizeDelta = new Vector2(ships[shipSelected].hltPoints, atkIM.rectTransform.sizeDelta.y);
    }
}
