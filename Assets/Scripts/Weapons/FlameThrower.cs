using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class FlameThrower : Weapon.Entity
    {
        public FlameThrower()
        {
            _name = "FLAME_THROWER";
            _damage = 300.0f;
            _rateOfFire = 5000.0f;
            _range = 4.0f;
            _projectileSpeed = 0.2f;
            _areaOfEffect = 8.0f;
            _type = WeaponType.PROJECTILE;
            _projectile = "FLAMETHROWER_PROJECTILE";
            _backwardForce = 150f;
        }
    }

}
