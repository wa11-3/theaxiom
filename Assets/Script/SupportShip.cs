using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportShip : Ship
{
    public override string shipName => "Support";
    public override int atkPoints => 3;
    public override int spdPoints => 5;
    public override int hltPoints => 9;
}
