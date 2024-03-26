using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShip : Ship
{
    public override string shipName => "Tank";
    public override int atkPoints => 6;
    public override int spdPoints => 3;
    public override int hltPoints => 8;

    public Transform bulletPosLeft;
    public Transform bulletPosRight;

    bool lastPosLeft = false;

    protected override void Update()
    {
        base.Update();
        if (_inputs.Ship.Shot.WasPressedThisFrame() && IsOwner)
        {
            if (lastPosLeft)
            {
                lastPosLeft = false;
                NetFunctionsScript.Instance.ShopServerRpc(shipName, bulletPosRight.position, NetworkManager.LocalClientId, shipName);
            }
            else
            {
                lastPosLeft = true;
                NetFunctionsScript.Instance.ShopServerRpc(shipName, bulletPosLeft.position, NetworkManager.LocalClientId, shipName);
            }
        }
    }
}
