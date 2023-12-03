using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShip : Ship
{
    public override string shipName => "Ghost";
    public override int atkPoints => 6;
    public override int spdPoints => 7;
    public override int hltPoints => 4;
}
