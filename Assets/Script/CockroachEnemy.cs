using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachEnemy : Enemy
{
    public override string enemyName => "Cockroach";

    public override int dmgPoints => 2;
    public override int spdPoints => 5;
}
