using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        private GameObject _player;
        private Vector3 _offset;
        void Awake()
        {
            _player = GameObject.Find("Player");
            _offset = transform.position - _player.transform.position;
        }

        void LateUpdate() // this is needed or else unity ceases to act like a normal game engine
        {
            transform.position = _player.transform.position + _offset;
        }
    }

}
