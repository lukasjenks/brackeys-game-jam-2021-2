using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class WallJob
    {

        private MeshRenderer _renderer;
        private string _type;
        private Color _originalColor;

        public WallJob(MeshRenderer r, string type, Color original)
        {
            _renderer = r;
            _type = type;
            _originalColor = original;
        }

        public void Run()
        {
            if (_type == "FADE")
            {
                _renderer.material.color = new Color(
                    _originalColor.r,
                    _originalColor.g,
                    _originalColor.b,
                    0.2f
                );
            }
            else if (_type == "APPEAR")
            {
                _renderer.material.color = new Color(
                    _originalColor.r,
                    _originalColor.g,
                    _originalColor.b,
                    1.0f
                );
            }
        }
    }

    public class PlayerCameraController : MonoBehaviour
    {
        private GameObject _player;
        private Vector3 _offset;
        public Material transparent;
        private Material _originalWallMaterial;
        private MeshRenderer _wall;

        private string _lastHit;
        private Material _lastColor;
        private List<MeshRenderer> _wallRenderers;
        private Queue<WallJob> _wallQueue;
        void Awake()
        {
            _wallRenderers = new List<MeshRenderer>();
            _wallQueue = new Queue<WallJob>();
            _player = GameObject.Find("Player");
            _offset = transform.position - _player.transform.position;
        }

        void Update()
        {
            throughWall();
            if (_wallQueue.Count > 0)
            {
                WallJob j = _wallQueue.Dequeue();
                j.Run();
            }
        }

        void LateUpdate() // this is needed or else unity ceases to act like a normal game engine
        {
            transform.position = _player.transform.position + _offset;
        }

        void throughWall()
        {
            RaycastHit hit;

            if (Physics.SphereCast(
                transform.position,
                2f,
                _player.transform.position - transform.position,
                out hit,
                _offset.magnitude
            ))
            {
                if (hit.collider.gameObject.tag == "Terrain")
                {
                    // we hit a new wall
                    if (_lastHit != hit.collider.gameObject.GetInstanceID().ToString() && _wall != null)
                    {
                        _wallQueue.Enqueue(new WallJob(_wall, "APPEAR", _lastColor.color));
                        _lastColor = null;
                        _wall = null;
                    }

                    if (_wall == null)
                    {
                        _lastHit = hit.collider.gameObject.GetInstanceID().ToString();
                        _wall = hit.collider.GetComponent<MeshRenderer>();

                        if (_lastColor == null)
                        {
                            _lastColor = _wall.material;
                        }

                        _wallQueue.Enqueue(new WallJob(_wall, "FADE", _lastColor.color));
                    }
                }
                else
                {
                    if (_wall)
                    {
                        _wallQueue.Enqueue(new WallJob(_wall, "APPEAR", _lastColor.color));
                        _wall = null;
                        _lastColor = null;
                    }
                }
            }
        }
    }
}
