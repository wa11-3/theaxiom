using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour
{
    public void OnClickInitButton(string serverType)
    {
        if (serverType == "host")
        {
            NetworkManager.Singleton.StartHost();
            var status = NetworkManager.Singleton.SceneManager.LoadScene("Select", LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {"Game"} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
        else if (serverType == "client")
        {
            //readyBT.SetActive(true);
        }
    }
}
