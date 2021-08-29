using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyType
{
    CREEP,
    CHASER,
    HUNTER
}

public class EnemyMovementHandler : MonoBehaviour
{
    public NPC.Enemy enemyScript;
    public EnemyType type;
    private GameObject _player;
    private Player.TopDownCharacterMover _playerMoveScript;
    private NavMeshAgent _agent;
    private float _tempSpeed;
    private bool _onCoolDown;

    // HUNTER SPECIFIC VARIABLES
    private List<Vector3> _hunterPath;
    private bool _hunterPathCreated = false;
    private bool _hunterCoroutineStarted = false;
    private int _hunterTarget;
    private bool _hunterAttackOnCoolDown = false;

    // CREEP SPECIFIC VARIABLES

    private float _creepCoolDown = 2.0f;
    private bool _creepOnCoolDown = false;

    private const int _DAMAGEABLE_LAYER = 6;
    private const int _TERRAIN_LAYER = 6;

    private const int _DAMAGE_LAYER = 1 << _TERRAIN_LAYER | 1 << _DAMAGEABLE_LAYER;


    void Start()
    {
        _onCoolDown = false;
        _player = GameObject.Find("Player");
        _playerMoveScript = _player.GetComponent<Player.TopDownCharacterMover>();
        _agent = GetComponent<NavMeshAgent>();
        _CreateHandlerScript();
    }

    void Awake()
    {
        _onCoolDown = false;
        _player = GameObject.Find("Player");
        _playerMoveScript = _player.GetComponent<Player.TopDownCharacterMover>();
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
            case EnemyType.HUNTER:
                enemyScript = new NPC.Hunter(_agent);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (type == EnemyType.CHASER)
        {
            if (collision.collider.gameObject.tag == "Player" && !_onCoolDown)
            {
                _onCoolDown = true;
                Player.Stats stats = collision.collider.gameObject.GetComponent<Player.Stats>() != null ? collision.collider.gameObject.GetComponent<Player.Stats>() : collision.collider.gameObject.GetComponentInParent<Player.Stats>();
                stats.GetHit(Player.HitType.CHASER);
                _agent.velocity = collision.collider.gameObject.transform.position - transform.position;
                _tempSpeed = enemyScript.MovementSpeed;
                enemyScript.MovementSpeed = _tempSpeed / 4;
                StartCoroutine(_ChangeSpeedAfterNSeconds(3.5f, _tempSpeed));
            }
        }
        else if (type == EnemyType.HUNTER)
        {
            if (collision.collider.gameObject.tag == "Player" && !_onCoolDown)
            {
                _onCoolDown = true;
                Player.Stats stats = collision.collider.gameObject.GetComponent<Player.Stats>() != null ? collision.collider.gameObject.GetComponent<Player.Stats>() : collision.collider.gameObject.GetComponentInParent<Player.Stats>();
                stats.GetHit(Player.HitType.HUNTER);
                StartCoroutine(_HunterResetCoolDown(3.5f));
            }
        }
    }
    void Update()
    {
        switch (type)
        {
            case EnemyType.CREEP:
                transform.LookAt(_player.transform.position);
                enemyScript.MoveToPosition(_player.transform.position);

                // if we're close enough we throw projectiles
                if (Vector3.Distance(transform.position, _player.transform.position) <= _agent.stoppingDistance)
                {
                    // StartCoroutine(HandleCoolDown(weapon.RateOfFire, weapon.Name));
                    if (!_creepOnCoolDown)
                    {
                        _creepOnCoolDown = true;
                        StartCoroutine(_CreepCooldown());
                        GameObject projectile = (GameObject)Resources.Load("Prefabs/" + enemyScript.Projectile);
                        Weapon.Projectile projectileScript = projectile.GetComponent<Weapon.Projectile>();
                        projectileScript.range = 50f;
                        projectileScript.direction = _player.transform.position - transform.position;
                        projectileScript.speed = 0.05f;
                        projectileScript.type = "CREEPER";
                        projectileScript.damage = 500f;
                        projectileScript.areaOfEffect = 2f;
                        Instantiate(
                            projectile,
                            transform.position + Vector3.Scale(transform.forward, new Vector3(2, 2, 2)),
                             transform.rotation * projectile.transform.rotation
                        );
                    }
                }

                break;

            case EnemyType.HUNTER:
                if (!_hunterPathCreated)
                {
                    _CreateHunterPath();
                    _hunterTarget = 0;
                }
                else
                {
                    // now move toward the path
                    for (int i = 0; i < _hunterPath.Count - 1; i++)
                    {
                        Debug.DrawLine(_hunterPath[i], _hunterPath[i + 1], Color.red);
                    }

                    if (Vector3.Distance(transform.position, _hunterPath[_hunterTarget]) > 6.0f)
                    {
                        transform.LookAt(_hunterPath[_hunterTarget]);
                        enemyScript.MoveToPosition(_hunterPath[_hunterTarget]);

                        if (!_onCoolDown)
                        {
                            _onCoolDown = true;
                            bool hitPlayer = false;
                            // check for a collision here with the player
                            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f, _DAMAGE_LAYER);
                            foreach (var hitCollider in hitColliders)
                            {
                                if (hitCollider.gameObject.tag == "Player")
                                {
                                    Player.Stats stats = hitCollider.gameObject.GetComponent<Player.Stats>() != null ? hitCollider.gameObject.GetComponent<Player.Stats>() : hitCollider.gameObject.GetComponentInParent<Player.Stats>();
                                    stats.GetHit(Player.HitType.HUNTER);
                                    _hunterCoroutineStarted = true;
                                    hitPlayer = true;
                                    StartCoroutine(_WaitThenCalculatePath(3.5f));
                                    break;
                                }
                            }

                            if (!hitPlayer)
                            {
                                _onCoolDown = false;
                            }
                        }
                    }
                    else
                    {
                        if (_hunterTarget < _hunterPath.Count - 1)
                        {
                            _hunterTarget++;
                        }
                        else
                        {
                            // _hunterPathCreated = false;
                            if (!_hunterCoroutineStarted)
                            {
                                _hunterCoroutineStarted = true;
                                StartCoroutine(_WaitThenCalculatePath(3.5f));
                            }
                            else
                            {
                                transform.LookAt(_player.transform.position);
                            }
                        }
                    }
                }

                break;

            case EnemyType.CHASER:
                transform.LookAt(_player.transform.position);
                enemyScript.MoveToPosition(_player.transform.position);

                break;
            default:
                break;
        }
    }

    private void _CreateHunterPath()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, _player.transform.position, NavMesh.AllAreas, path);

        // we should generate another vector in the middle here
        List<Vector3> pathVectors = new List<Vector3>();

        if (path.corners.Length > 0)
        {
            for (int i = 0; i < path.corners.Length; i++)
            {
                pathVectors.Add(path.corners[i]);
            }

            Vector3 temp = new Vector3(
                path.corners[path.corners.Length - 1].x,
                path.corners[path.corners.Length - 1].y,
                path.corners[path.corners.Length - 1].z
            );

            pathVectors.Insert((int)path.corners.Length - 1, Vector3.Scale(temp, new Vector3(0.5f, 0.5f, 0.5f)));

            _hunterPathCreated = true;
            _hunterPath = pathVectors;
        }
        else
        {
            pathVectors.Add(_player.transform.position);
        }
    }

    private IEnumerator _WaitThenCalculatePath(float n)
    {
        yield return new WaitForSeconds(n);
        _hunterPathCreated = false;
        _hunterCoroutineStarted = false;
        _onCoolDown = false;
    }

    private IEnumerator _ChangeSpeedAfterNSeconds(float n, float value)
    {
        yield return new WaitForSeconds(n);
        enemyScript.MovementSpeed = value;
        _onCoolDown = false;
    }

    private IEnumerator _CreepCooldown()
    {
        yield return new WaitForSeconds(_creepCoolDown);
        _creepOnCoolDown = false;
    }

    private IEnumerator _HunterResetCoolDown(float n)
    {
        yield return new WaitForSeconds(_creepCoolDown);
        _onCoolDown = false;
    }
}
