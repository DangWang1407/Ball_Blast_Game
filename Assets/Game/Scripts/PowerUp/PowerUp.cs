using Game.Controllers;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Game.PowerUp
{
    public enum PowerUpType
    {
        //weapon
        RapidFire = 0,
        DoubleShot = 1,
        BurstShot = 2,
        DiagonalFire = 3,


        //missile
        Homing = 4,
        BounceShot = 5,
        PierceShot = 6,
        PowerShot = 7,
        DamageBoost = 8,

        //player
        Invisible = 9,
        Shield = 10,
        None = -1
    }

    public enum TargetType
    {
        Player,
        Weapon,
        Missile
    }
}