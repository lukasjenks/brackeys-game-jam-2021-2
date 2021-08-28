using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Shotgun : Weapon.Entity
    {
        public Shotgun()
        {
            _name = "SHOT_GUN";
            _damage = 500.0f;
            _rateOfFire = 300.0f;
            _range = 4.0f;
            _projectileSpeed = 1.5f;
            _areaOfEffect = 8.0f;
            _type = WeaponType.PROJECTILE;
            _projectile = "MACHINE_GUN_PROJECTILE";
            _backwardForce = 1500f;
        }
    }

}
