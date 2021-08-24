using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float range;
    public float speed;
    public Vector3 direction;
    public string type;
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
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit enemy!");
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
