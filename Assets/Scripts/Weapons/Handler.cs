using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Weapon namespace
namespace Weapon
{
    public class Handler : MonoBehaviour
    {
        private int _currentWeapon;

        private List<Weapon.Entity> _weapons = new List<Weapon.Entity> {
        new Weapon.RocketLauncher(),
        new Weapon.MachineGun(),
        new Weapon.FlameThrower(),
        new Weapon.Shotgun()
        };

        private Dictionary<string, bool> _weaponCoolDowns = new Dictionary<string, bool>();
        private GameObject _currentFiringParticles;

        private GameObject _player;
        private Player.TopDownCharacterMover _playerControlScript;

        public bool isActive = true;
        private AudioManager audioManager;

        void Awake()
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        void Start()
        {
            _player = GameObject.Find("Player");
            _playerControlScript = _player.GetComponent<Player.TopDownCharacterMover>();
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
            if (isActive)
            {
                if (Input.GetButton("Fire1") && !_weaponCoolDowns[_weapons[_currentWeapon].Name])
                {
                    _HandleFire();
                }

                if (Input.GetButtonUp("Fire1")) // check if they let go of fire, if so we reset the cooldown and switch weapons
                {
                    audioManager.StopGroup("Weapons");
                    _currentWeapon = _GetRandomWeapon();
                    if (_weapons[_currentWeapon].Type == WeaponType.PRECISE)
                    {
                        Destroy(_currentFiringParticles);
                        _currentFiringParticles = null;
                    }
                }
            }
        }

        IEnumerator HandleCoolDown(float time, string weaponName)
        {
            yield return new WaitForSeconds((1000f - time) / 1000f);
            _weaponCoolDowns[weaponName] = false;
        }

        private void _HandleFire()
        {
            Weapon.Entity weapon = _weapons[_currentWeapon];
            _weaponCoolDowns[weapon.Name] = true;
            GameObject parent = gameObject.transform.parent.gameObject;

            if (weapon.Type == Weapon.WeaponType.PROJECTILE)
            {
                if (weapon.Name == "FLAME_THROWER")
                {
                    if (!audioManager.IsPlaying("Flamethrower"))
                        audioManager.Play("Flamethrower");
                    StartCoroutine(HandleCoolDown(weapon.RateOfFire, weapon.Name));
                    GameObject projectile = (GameObject)Resources.Load("Prefabs/" + weapon.Projectile);
                    Weapon.Projectile projectileScript = projectile.GetComponent<Weapon.Projectile>();
                    projectileScript.range = weapon.Range + Random.Range(-2.5f, 2.5f);
                    projectileScript.direction = transform.right;
                    projectileScript.speed = weapon.ProjectileSpeed;
                    projectileScript.type = weapon.Name;
                    projectileScript.damage = weapon.Damage;
                    projectileScript.areaOfEffect = weapon.AreaOfEffect;
                    Instantiate(projectile, transform.position, parent.transform.rotation * projectile.transform.rotation);
                }
                else if (weapon.Name != "SHOT_GUN")
                {
                    if (weapon.Name == "MACHINE_GUN" && !audioManager.IsPlaying("Machine Gun"))
                    {
                        audioManager.Play("Machine Gun");
                    }
                    else if (weapon.Name == "ROCKET_LAUNCHER")
                    {
                        audioManager.Play("Rocket Launch");
                    }
                    StartCoroutine(HandleCoolDown(weapon.RateOfFire, weapon.Name));
                    GameObject projectile = (GameObject)Resources.Load("Prefabs/" + weapon.Projectile);
                    Weapon.Projectile projectileScript = projectile.GetComponent<Weapon.Projectile>();
                    projectileScript.range = weapon.Range;
                    projectileScript.direction = transform.right;
                    projectileScript.speed = weapon.ProjectileSpeed;
                    projectileScript.type = weapon.Name;
                    projectileScript.damage = weapon.Damage;
                    projectileScript.areaOfEffect = weapon.AreaOfEffect;
                    Instantiate(projectile, transform.position, parent.transform.rotation * projectile.transform.rotation);
                }
                else
                {
                    audioManager.Play("Shotgun");
                    // we spawn 3 here at angles
                    StartCoroutine(HandleCoolDown(weapon.RateOfFire, weapon.Name));
                    // instantiate our forward bullet
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 0)
                        {
                            GameObject projectile = (GameObject)Resources.Load("Prefabs/" + weapon.Projectile);
                            Weapon.Projectile projectileScript = projectile.GetComponent<Weapon.Projectile>();
                            projectileScript.range = weapon.Range;
                            projectileScript.direction = transform.right;
                            projectileScript.speed = weapon.ProjectileSpeed;
                            projectileScript.type = weapon.Name;
                            projectileScript.damage = weapon.Damage;
                            projectileScript.areaOfEffect = weapon.AreaOfEffect;
                            Instantiate(projectile, transform.position, parent.transform.rotation * projectile.transform.rotation);
                        }
                        else if (i % 2 == 0)
                        { //even
                            GameObject projectile = (GameObject)Resources.Load("Prefabs/" + weapon.Projectile);
                            Weapon.Projectile projectileScript = projectile.GetComponent<Weapon.Projectile>();
                            projectileScript.range = weapon.Range;
                            projectileScript.direction = transform.right + transform.forward;
                            projectileScript.speed = weapon.ProjectileSpeed;
                            projectileScript.type = weapon.Name;
                            projectileScript.damage = weapon.Damage;
                            projectileScript.areaOfEffect = weapon.AreaOfEffect;
                            Instantiate(projectile, transform.position, parent.transform.rotation * projectile.transform.rotation * Quaternion.Euler(0, 0, 45));
                        }
                        else
                        {  //odd
                            GameObject projectile = (GameObject)Resources.Load("Prefabs/" + weapon.Projectile);
                            Weapon.Projectile projectileScript = projectile.GetComponent<Weapon.Projectile>();
                            projectileScript.range = weapon.Range;
                            projectileScript.direction = transform.right - transform.forward;
                            projectileScript.speed = weapon.ProjectileSpeed;
                            projectileScript.type = weapon.Name;
                            projectileScript.damage = weapon.Damage;
                            projectileScript.areaOfEffect = weapon.AreaOfEffect;
                            Instantiate(projectile, transform.position, parent.transform.rotation * projectile.transform.rotation * Quaternion.Euler(0, 0, -45));
                        }
                    }
                }
                // else
                // {
                //     StartCoroutine(HandleCoolDown(weapon.RateOfFire, weapon.Name));
                //     GameObject projectile = (GameObject)Resources.Load("Prefabs/" + weapon.Projectile);
                //     Weapon.Projectile projectileScript = projectile.GetComponent<Weapon.Projectile>();
                //     projectileScript.range = weapon.Range;
                //     projectileScript.direction = transform.right;
                //     projectileScript.speed = weapon.ProjectileSpeed;
                //     projectileScript.type = weapon.Name;
                //     projectileScript.damage = weapon.Damage;
                //     projectileScript.areaOfEffect = weapon.AreaOfEffect;
                //     Instantiate(projectile, transform.position, parent.transform.rotation * projectile.transform.rotation);

                // }
            }
            else if (weapon.Type == Weapon.WeaponType.AREA)
            {
                Debug.Log("NOT YET IMPLEMENTED!");
            }
            else if (weapon.Type == Weapon.WeaponType.PRECISE)
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

}
