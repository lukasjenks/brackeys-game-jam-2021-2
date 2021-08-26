using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class RocketLauncher : Weapon.Entity
    {
        public RocketLauncher()
        {
            _name = "ROCKET_LAUNCHER";
            _damage = 300.0f;
            _rateOfFire = 1.0f;
            _range = 100.0f;
            _projectileSpeed = 0.5f;
            _areaOfEffect = 5.0f;
            _type = WeaponType.PROJECTILE;
            _projectile = "ROCKET_PROJECTILE";
        }
    }

}
