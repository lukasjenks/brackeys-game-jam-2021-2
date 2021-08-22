using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float range;
    public float speed;
    public Vector3 direction;
    private Vector3 _startVector;

    void Start()
    {
        _startVector = transform.position;
        Debug.Log(_startVector);
        Debug.Log(direction);
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
