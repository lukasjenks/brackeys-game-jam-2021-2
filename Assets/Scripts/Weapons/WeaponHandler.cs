using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private int _currentWeapon;

    private List<WeaponEntity> _weapons = new List<WeaponEntity> {
        new RocketLauncher(),
        new MachineGun()
    };

    private Dictionary<string, bool> _weaponCoolDowns = new Dictionary<string, bool>();
    private GameObject _currentFiringParticles;

    void Start()
    {
        _currentWeapon = _GetRandomWeapon();

        //populate our cooldown dictionary for the weapons
        for (int i = 0; i < _weapons.Count; i++)
        {
            _weaponCoolDowns.Add(_weapons[i].Name, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && !_weaponCoolDowns[_weapons[_currentWeapon].Name])
        {
            _HandleFire();
        }

        if (Input.GetButtonUp("Fire1")) // check if they let go of fire, if so we reset the cooldown and switch weapons
        {
            if (_weapons[_currentWeapon].Type == WeaponType.PRECISE)
            {
                Destroy(_currentFiringParticles);
                _currentFiringParticles = null;
            }
            _currentWeapon = _GetRandomWeapon();
        }
    }

    IEnumerator HandleCoolDown(float time, string weaponName)
    {
        yield return new WaitForSeconds((1000f - time) / 1000f);
        _weaponCoolDowns[weaponName] = false;
    }

    private void _HandleFire()
    {
        WeaponEntity weapon = _weapons[_currentWeapon];
        _weaponCoolDowns[weapon.Name] = true;
        GameObject parent = gameObject.transform.parent.gameObject;

        if (weapon.Type == WeaponType.PROJECTILE)
        {
            StartCoroutine(HandleCoolDown(weapon.RateOfFire, weapon.Name));
            GameObject projectile = (GameObject)Resources.Load("Prefabs/" + weapon.Projectile);

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.range = weapon.Range;
            projectileScript.direction = parent.transform.forward;
            projectileScript.speed = weapon.ProjectileSpeed;
            Instantiate(projectile, parent.transform.position, parent.transform.rotation * Quaternion.Euler(0, 90, 0));
        }
        else if (weapon.Type == WeaponType.AREA)
        {
            Debug.Log("NOT YET IMPLEMENTED!");
        }
        else if (weapon.Type == WeaponType.PRECISE)
        {
            // Started down this path, don't think particles are the way to go but not 100% sure as things
            // weren't lining up properly when player moved arouund (got weird artifacts)
            Debug.Log("NOT YET IMPLEMENTED!");

            // if (_currentFiringParticles == null)
            // {
            //     GameObject particleEffect = (GameObject)Resources.Load("Prefabs/" + weapon.ParticleEffect);
            //     _currentFiringParticles = Instantiate(particleEffect, parent.transform.position, parent.transform.rotation);
            //     _currentFiringParticles.transform.SetParent(gameObject.transform.parent.transform);
            // }
            // _weapons[_currentWeapon].Hit(gameObject.transform.position, gameObject.transform.forward);
        }
    }

    private int _GetRandomWeapon()
    {
        int newWeaponIndex = Random.Range(0, _weapons.Count);

        while (newWeaponIndex == _currentWeapon)
        {
            newWeaponIndex = Random.Range(0, _weapons.Count);
        }

        return newWeaponIndex;
    }
}
