using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(InputHandler))]
    public class TopDownCharacterMover : MonoBehaviour
    {
        [SerializeField]
        private bool rotateTowardMouse;

        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private Camera playerCamera;

        [SerializeField]
        private float baseSpeed;

        [SerializeField]
        private float sprintSpeed;

        [SerializeField]
        private float speedShiftMultiplier;

        [SerializeField]
        private float jumpForce;

        [SerializeField]
        private int interpolationRatio;

        [SerializeField]
        private float groundCheckDist = 1.0f;

        private InputHandler playerInput;

        private bool sprinting;

        private bool isJumping; // for animations later

        private int elapsedFrames;

        private Vector3 _velocity;
        private Vector3 _lastFramePosition;

        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        private InputHandler input;

        private Rigidbody rb;
        private CharacterController _cc;

        public bool isActive = true;
        [SerializeField]
        private float _gravity = -9.8f;

        private Vector3 _jumpVelocity;
        private AudioManager audioManager;
        private Animator _animator;
        private float moveSpeed; // calculated from baseSpeed and sprintSpeed
        private Vector3 _aimVelocity;
        private Vector3 _aimVector;

        private void Awake()
        {
            Time.timeScale = 1;
            playerInput = GetComponent<InputHandler>();
            rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3();
            _cc = GetComponent<CharacterController>();
            moveSpeed = baseSpeed;
            _jumpVelocity = new Vector3();
            audioManager = FindObjectOfType<AudioManager>();
            _animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                Vector3 mouseLocation = _GetMousePositionWorldSpace();
                if (!mouseLocation.Equals(new Vector3()))
                {
                    _aimVelocity = _aimVector - mouseLocation;
                }

                var targetVector = new Vector3(playerInput.inputVector.x, 0, playerInput.inputVector.y);
                _velocity = _cc.velocity;

                float deltaX = Input.GetAxis("Horizontal") * moveSpeed;
                float deltaZ = Input.GetAxis("Vertical") * moveSpeed;

                Vector3 movement = new Vector3();
                movement += (deltaX * new Vector3(1, 0, 0));
                movement += (deltaZ * new Vector3(0, 0, 1));
                movement.y = _gravity;

                movement = Vector3.ClampMagnitude(movement, moveSpeed);
                movement *= Time.deltaTime;

                if (deltaX != 0 || deltaZ != 0)
                {
                    if (!audioManager.IsPlaying("Walking"))
                    {
                        audioManager.Play("Walking");
                    }
                }
                else
                {
                    audioManager.Pause("Walking");
                }

                _cc.Move(movement);
                RotateFromMouseVector();

                if (_cc.velocity.magnitude > 0.0f)
                {
                    var forward = (transform.forward - _aimVelocity).magnitude;
                    var backward = ((-transform.forward) - _aimVelocity).magnitude;
                    var right = (transform.right - _aimVelocity).magnitude;
                    var left = ((-transform.right) - _aimVelocity).magnitude;

                    var multiplier = 10000;

                    _animator.SetFloat("ForwardSpeed", forward * multiplier);
                    _animator.SetFloat("BackwardSpeed", backward * multiplier);
                    _animator.SetFloat("RightSpeed", right * multiplier);
                    _animator.SetFloat("LeftSpeed", left * multiplier);

                    // Debug.Log("FORWARD: " + (forward * multiplier));
                    // Debug.Log("BACKWARD: " + (backward * multiplier));
                    // Debug.Log("RIGHT: " + (right * multiplier));
                    // Debug.Log("LEFT: " + (left * multiplier));
                }
                else
                {
                    _animator.SetFloat("ForwardSpeed", 0.0f);
                    _animator.SetFloat("BackwardSpeed", 0.0f);
                    _animator.SetFloat("RightSpeed", 0.0f);
                    _animator.SetFloat("LeftSpeed", 0.0f);
                }
            }
        }

        bool IsCBetweenAB(Vector3 A, Vector3 B, Vector3 C)
        {
            return Vector3.Dot((B - A).normalized, (C - B).normalized) < 0f && Vector3.Dot((A - B).normalized, (C - A).normalized) < 0f;
        }

        private Vector3 _GetMousePositionWorldSpace()
        {
            Ray ray = playerCamera.ScreenPointToRay(playerInput.mousePosition);
            Vector3 target = new Vector3();

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                target = hitInfo.point;
            }

            return target;
        }

        private void RotateFromMouseVector()
        {
            Vector3 target = _GetMousePositionWorldSpace();

            if (!target.Equals(new Vector3()))
            {
                target.y = transform.position.y;

                var qTo = Quaternion.LookRotation(target - transform.position);
                qTo = Quaternion.Slerp(transform.rotation, qTo, 15f * Time.deltaTime);
                rb.MoveRotation(qTo);
                _aimVector = target;
            }
        }
    }
}
