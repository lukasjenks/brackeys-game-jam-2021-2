using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovementHandler : MonoBehaviour
{
    public NPC.Enemy enemyScript;
    private GameObject _player;
    private NavMeshAgent _agent;

    void Start()
    {
        _player = GameObject.Find("Player");
        _agent = GetComponent<NavMeshAgent>();
        enemyScript = new NPC.Creep(_agent);
    }

    void Awake()
    {
        _player = GameObject.Find("Player");
        _agent = GetComponent<NavMeshAgent>();
        enemyScript = new NPC.Creep(_agent);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    void Update()
    {
        enemyScript.MoveToPosition(_player.transform.position);
    }
}
