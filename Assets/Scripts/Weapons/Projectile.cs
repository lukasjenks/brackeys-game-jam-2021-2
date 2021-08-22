using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float range;
    public float speed;
    public Vector3 direction;
    private Vector3 _startVector;
    private Collider _collider;

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

        Destroy(gameObject); // destroy the projectile
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_startVector, transform.position) < range)
        {
            transform.position += direction * speed;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
