using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FighterShip : Ship
{
    public override string shipName => "Fighter";
    public override int atkPoints => 7;
    public override int spdPoints => 5;
    public override int hltPoints => 5;

    public Transform bulletPos;

    protected override void Update()
    {
        base.Update();
        if (_inputs.Ship.Shot.WasPressedThisFrame() && IsOwner)
        {
            NetFunctionsScript.Instance.ShopServerRpc(shipName, bulletPos.position, NetworkManager.LocalClientId, shipName);
        }
    }
}
