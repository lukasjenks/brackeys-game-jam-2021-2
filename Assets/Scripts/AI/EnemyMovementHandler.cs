using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyType
{
    CREEP,
    CHASER
}

public class EnemyMovementHandler : MonoBehaviour
{
    public NPC.Enemy enemyScript;
    public EnemyType type;
    private GameObject _player;
    private TopDownCharacterMover _playerMoveScript;
    private NavMeshAgent _agent;

    void Start()
    {
        _player = GameObject.Find("Player");
        _playerMoveScript = _player.GetComponent<TopDownCharacterMover>();
        _agent = GetComponent<NavMeshAgent>();
        _CreateHandlerScript();
    }

    void Awake()
    {
        _player = GameObject.Find("Player");
        _playerMoveScript = _player.GetComponent<TopDownCharacterMover>();
        _agent = GetComponent<NavMeshAgent>();
        _CreateHandlerScript();
    }

    private void _CreateHandlerScript()
    {
        switch (type)
        {
            case EnemyType.CREEP:
                enemyScript = new NPC.Creep(_agent);
                break;
            case EnemyType.CHASER:
                enemyScript = new NPC.Chaser(_agent);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    void Update()
    {
        switch (type)
        {
            case EnemyType.CREEP:
                transform.LookAt(_player.transform.position);
                enemyScript.MoveToPosition(_player.transform.position);
                break;

            case EnemyType.CHASER:
                // get player velocity vector
                var playerVelocity = _playerMoveScript.Velocity;
                var distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

                if (distanceToPlayer > 10)
                {
                    Vector3 destination = _player.transform.position - (playerVelocity * (distanceToPlayer - 10));
                    enemyScript.MoveToPosition(destination);
                }
                else
                {
                    enemyScript.MoveToPosition(_player.transform.position);
                }
                break;
            default:
                break;
        }
    }
}
