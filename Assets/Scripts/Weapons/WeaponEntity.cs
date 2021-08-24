using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEntity
{
    void Die();
}

public enum WeaponType
{
    PROJECTILE, // rocket launcher, spear gun
    AREA, // Flame thrower, ice shooter etc
    PRECISE // Machine gun, shotgun
}


public class WeaponEntity
{
    protected float _backwardForce;
    protected string _name;
    protected float _damage;
    protected float _rateOfFire;
    protected float _areaOfEffect;
    protected float _projectileSpeed;
    protected float _range;
    protected float _sphereCastRadius;
    protected string _particleEffectName;
    protected string _projectile;
    protected WeaponType _type;

    public float RateOfFire
    {
        get { return _rateOfFire; }
        set { _rateOfFire = value; }
    }

    public float AreaOfEffect
    {
        get { return _areaOfEffect; }
        set { _areaOfEffect = value; }
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

    public WeaponType Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public string ParticleEffect
    {
        get { return _particleEffectName; }
        set { _particleEffectName = value; }
    }

    public void Hit(Vector3 start, Vector3 dest)
    {
        RaycastHit hit;

        if (Physics.Raycast(start, dest, out hit, _range))
        {
            Debug.Log(hit);
            Debug.Log(hit.collider.gameObject);
        }
    }
}
