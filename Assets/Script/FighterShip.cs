using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterShip : Ship
{
    public override string shipName => "Fighter";
    public override int atkPoints => 7;
    public override int spdPoints => 5;
    public override int hltPoints => 5;
}
