using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool _dying;

    void Start()
    {
        _startVector = transform.position;
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (areaOfEffect > 0) // make sure there's even an effect to calculate here
        {

            // check if any nearby enemies
            List<GameObject> objectList = new List<GameObject>(NPC.Manager.npcDict.Values);

            foreach (var value in objectList)
            {
                // if its within area of effect for this weapon type, we deal dmg based on dmg/distance
                var distance = Vector3.Distance(transform.position, value.transform.position);
                Debug.Log(distance);

                if (distance <= areaOfEffect && value.GetInstanceID() != other.gameObject.GetInstanceID())
                {
                    EnemyMovementHandler tempEnemyHandler = value.GetComponent<EnemyMovementHandler>();
                    Debug.Log("Dealt splash damage of: " + damage / distance);
                    if (tempEnemyHandler.enemyScript.TakeDamage(damage / distance))
                    {
                        NPC.Manager.npcDict.Remove(value.GetInstanceID().ToString());
                        Destroy(value);
                    }
                }
            }
        }

        if (other.gameObject.tag == "Enemy")
        {
            EnemyMovementHandler enemyHandler = other.gameObject.GetComponent<EnemyMovementHandler>();
            if (enemyHandler.enemyScript.TakeDamage(damage))
            {
                NPC.Manager.npcDict.Remove(other.gameObject.GetInstanceID().ToString());
                Destroy(other.gameObject);
            }
        }

        if (!_dying)
        {
            _OnDeath();
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

            case "MACHINE_GUN":
                Destroy(gameObject);
                break;

            default:
                Debug.Log("Unknown weapon type!");
                break;
        }
    }

    private void _TurnInvisible()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponentInChildren<Light>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();
    }

    private IEnumerator _WaitForExplosionToFinish(GameObject g, GameObject rocket)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(g);
        Destroy(rocket);
    }
}
