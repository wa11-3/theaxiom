using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspEnemy : Enemy
{
    public override string enemyName => "Wasp";

    public override int dmgPoints => 1;
    public override int spdPoints => 4;
}
