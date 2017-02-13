using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameMode
{
    public string roundName;
    public bool useOnlySpikes;
    public bool useCustomBrickChance;
    public float customBrickChance;
    public bool onlyDestructable;
    public bool instantDestruction;
    public bool useBulletGravity;
    public bool stopBulletFade;
    public bool noFiring;
    public bool killOnNotSwitch;
    public bool stopMidAirJump;
    public float bulletSpeed = 1f;
}

/*
 * The Floor is Lava (all spikes, low probability)
Grounded (can't jump unless on the ground)
Artillery Mode (remove bullet fade, add gravity)
Butts Only (only butt stomps, no indestructable)
Unstable Dimensions (die if remain in same dimension for too long)
*/