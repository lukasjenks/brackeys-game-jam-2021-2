using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponEntity
{
    public MachineGun()
    {
        _name = "MACHINE_GUN";
        _damage = 25.0f;
        _rateOfFire = 5000.0f;
        _range = 50.0f;
        _type = WeaponType.PROJECTILE;
        _projectileSpeed = 2.0f;
        _projectile = "MACHINE_GUN_PROJECTILE";
        _areaOfEffect = 0;
    }
}
