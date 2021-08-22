using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponEntity
{
    public MachineGun()
    {
        _name = "MACHINE_GUN";
        _damage = 25.0f;
        _rateOfFire = 50.0f;
        _range = 50.0f;
        _projectileSpeed = 3.5f;
        _type = WeaponType.PROJECTILE;
        _projectile = "ROCKET_PROJECTILE";
    }
}
