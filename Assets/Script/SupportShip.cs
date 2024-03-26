using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportShip : Ship
{
    public override string shipName => "Support";
    public override int atkPoints => 3;
    public override int spdPoints => 5;
    public override int hltPoints => 9;

    public Transform bulletPos;

    [SerializeField]
    public GameObject supportUlti;

    protected override void Update()
    {
        base.Update();
        if (_inputs.Ship.Shot.WasPressedThisFrame() && IsOwner)
        {
            NetFunctionsScript.Instance.ShopServerRpc("Fighter", bulletPos.position, NetworkManager.LocalClientId, shipName);
        }

        if (_inputs.Ship.Ulti.WasPressedThisFrame() && IsOwner)
        {
            if (NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerNetScript>().ulti.Value >= 10)
            {
                //Debug.Log("Ulti");
                NetFunctionsScript.Instance.ActivateUltiServerRpc(NetworkManager.LocalClientId);
            }
        }
    }
}
