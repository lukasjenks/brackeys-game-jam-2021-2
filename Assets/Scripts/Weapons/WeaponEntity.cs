using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    PROJECTILE,
    AREA
}

public class WeaponEntity
{
    protected float _backwardForce;
    protected string _name;
    protected float _damage;
    protected float _rateOfFire;
    protected float _projectileSpeed;
    protected float _range;
    protected string _projectile;
    protected WeaponType _type;

    public float RateOfFire
    {
        get { return _rateOfFire; }
        set { _rateOfFire = value; }
    }

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public float BackwardForce
    {
        get { return _backwardForce; }
        set { _backwardForce = value; }
    }

    public string Projectile
    {
        get { return _projectile; }
        set { _projectile = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public float ProjectileSpeed
    {
        get { return _projectileSpeed; }
        set { _projectileSpeed = value; }
    }

    public float Range
    {
        get { return _range; }
        set { _range = value; }
    }
}
