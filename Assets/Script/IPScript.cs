using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class IPScript : MonoBehaviour
{
    // Script that Manager the IP and the Server Connections
    public GameObject[] ipNumbers;
    public int pointer;
    public Transform[] verticalTransforms;

    public void OnClickVerticalButton(string buttonType)
    {
        int ipValue = int.Parse(ipNumbers[pointer].GetComponent<TMP_Text>().text);

        if (buttonType == "up")
        {
            if (ipValue < 9)
            {
                ipValue += 1;
            }
        }
        else if (buttonType == "down")
        {
            if (ipValue > 0)
            {
                ipValue -= 1;
            }
        }

        ipNumbers[pointer].GetComponent<TMP_Text>().text = $"{ipValue}";
    }

    public void OnClickHorizontalButton(string buttonType)
    {
        if (buttonType == "left" && pointer > 0)
        {
            pointer -= 1;
        }
        else if (buttonType == "right" && pointer < 11)
        {
            pointer += 1;
        }

        foreach (Transform verTransform in verticalTransforms)
        {
            verTransform.position = new Vector3(
                ipNumbers[pointer].transform.position.x - 6.6f,
                verTransform.position.y, 0);
        }
    }

    public void OnClickConnectButton()
    {
        string ip = "";
        bool point = false;
        for (int i = 0; i < ipNumbers.Length; i++)
        {
            if (point && ipNumbers[i].GetComponent<TMP_Text>().text != "0")
            {
                ip += $"{ipNumbers[i].GetComponent<TMP_Text>().text}";
                point = false;
            }
            else if (!point)
            {
                ip += $"{ipNumbers[i].GetComponent<TMP_Text>().text}";
            }

            if (i != 0 && (i + 1) % 3 == 0 && i != 11)
            {
                ip += ".";
                point = true;
            }
        }
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ip;
        NetworkManager.Singleton.StartClient();
    }
}
