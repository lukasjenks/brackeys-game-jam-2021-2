using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private WeaponEntity _currentWeapon;
    private bool _fireOnCoolDown;

    void Start()
    {
        _currentWeapon = new MachineGun();
        _fireOnCoolDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && !_fireOnCoolDown)
        {
            HandleFire();
        }
    }

    IEnumerator HandleCoolDown(float time)
    {
        yield return new WaitForSeconds(time / 1000f);
        _fireOnCoolDown = false;
    }

    void HandleFire()
    {
        _fireOnCoolDown = true;
        StartCoroutine(HandleCoolDown(_currentWeapon.RateOfFire));
        GameObject projectile = (GameObject)Resources.Load("Prefabs/" + _currentWeapon.Projectile);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.range = _currentWeapon.Range;
        projectileScript.direction = transform.right;
        projectileScript.speed = _currentWeapon.ProjectileSpeed;
        Instantiate(projectile, transform.position, transform.rotation);
    }
}
