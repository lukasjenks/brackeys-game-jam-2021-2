using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Projectile : MonoBehaviour
    {
        public float range;
        public float speed;
        public Vector3 direction;
        public string type;
        public float damage;
        public float areaOfEffect;
        private Vector3 _startVector;
        private Collider _collider;
        private NPC.Gibber _gibber;
        private bool _dying;

        private const int _DAMAGE_LAYER = 1 << 6;


        void Start()
        {
            _startVector = transform.position;
            _collider = GetComponent<Collider>();
            _gibber = GameObject.Find("Gibber").GetComponent<NPC.Gibber>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Unshootable")
            {
                // check if any nearby enemies
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaOfEffect, _DAMAGE_LAYER);

                foreach (var hitCollider in hitColliders)
                {
                    var distance = Vector3.Distance(transform.position, hitCollider.gameObject.transform.position);
                    if (hitCollider.gameObject.tag == "Enemy" && distance <= areaOfEffect)
                    {
                        EnemyMovementHandler tempEnemyHandler = hitCollider.gameObject.GetComponent<EnemyMovementHandler>();
                        if (tempEnemyHandler.enemyScript.TakeDamage(damage / distance))
                        {
                            _gibber.Activate(transform.position);
                            NPC.Manager.npcDict.Remove(hitCollider.gameObject.GetInstanceID().ToString());
                            Destroy(hitCollider.gameObject);
                        }
                    }
                }
                if (!_dying)
                {
                    _OnDeath();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(_startVector, transform.position) < range && !_dying)
            {
                transform.position += direction * speed;
            }
            else
            {
                if (!_dying)
                {
                    _OnDeath();
                }
            }
        }

        private void _OnDeath()
        {
            switch (type)
            {
                case "ROCKET_LAUNCHER":
                    // Load up the explosion prefab and instantiate it here
                    _dying = true;
                    _TurnInvisible();
                    GameObject explosion = (GameObject)Resources.Load("Prefabs/EXPLOSION");
                    GameObject explosionInstance = Instantiate(explosion, transform.position, explosion.transform.rotation);
                    StartCoroutine(_WaitForExplosionToFinish(explosionInstance, gameObject));
                    break;

                case "MACHINE_GUN" or "SHOT_GUN":
                    Destroy(gameObject);
                    break;

                case "FLAME_THROWER":
                    StartCoroutine(_WaitNSecondsThenDie(3f));
                    break;

                default:
                    Debug.Log("Unknown weapon type!");
                    break;
            }
        }

        private void _TurnInvisible()
        {
            _collider.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Stop();
        }

        private IEnumerator _WaitForExplosionToFinish(GameObject g, GameObject rocket)
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(g);
            Destroy(rocket);
        }

        private IEnumerator _WaitNSecondsThenDie(float value)
        {
            yield return new WaitForSeconds(value);
            Destroy(gameObject);
        }
    }
}
