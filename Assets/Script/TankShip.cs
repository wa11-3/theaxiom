using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShip : Ship
{
    public override string shipName => "Tank";
    public override int atkPoints => 6;
    public override int spdPoints => 3;
    public override int hltPoints => 8;
}
