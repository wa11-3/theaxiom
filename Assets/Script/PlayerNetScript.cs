using UnityEngine;
using Unity.Netcode;

public class PlayerNetScript : NetworkBehaviour
{
    public NetworkVariable<int> shipSelected = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> damage = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> shield = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> ulti = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public GameObject shipObject;

    public int amountPlayers;

    private void Start()
    {
        shipSelected.OnValueChanged += ShipSelected;
        if (IsHost && IsOwner)
        {
            GameManager.Instance.HostStarted();
        }
    }

    private void ShipSelected(int previous, int current)
    {
        //damage.Value = GameManager.Instance.ships[shipSelected.Value].GetComponent<Ship>().hltPoints;
    }
}
