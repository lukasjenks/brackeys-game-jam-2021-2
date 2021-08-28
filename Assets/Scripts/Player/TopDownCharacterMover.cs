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

        private float moveSpeed; // calculated from baseSpeed and sprintSpeed

        private void Awake()
        {
            Time.timeScale = 1;
            playerInput = GetComponent<InputHandler>();
            rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3();
            _cc = GetComponent<CharacterController>();
            moveSpeed = baseSpeed;
            _jumpVelocity = new Vector3();
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {


                if (_cc.isGrounded && playerInput.spacePressed)
                {
                    _jumpVelocity.y = jumpForce;
                    isJumping = true;
                }
                else if (!_cc.isGrounded && isJumping)
                {
                    _jumpVelocity.y += _gravity * Time.deltaTime;
                }
                else
                {
                    _jumpVelocity.y = _gravity;
                }


                // else
                // {
                //     _jumpVelocity.y += _gravity * Time.deltaTime;
                // }

                var targetVector = new Vector3(playerInput.inputVector.x, 0, playerInput.inputVector.y);
                _velocity = _cc.velocity;

                float deltaX = Input.GetAxis("Horizontal") * moveSpeed;
                float deltaZ = Input.GetAxis("Vertical") * moveSpeed;

                Vector3 movement = new Vector3();
                movement += (deltaX * new Vector3(1, 0, 0));
                movement += (deltaZ * new Vector3(0, 0, 1));
                movement.y = _jumpVelocity.y;

                movement = Vector3.ClampMagnitude(movement, moveSpeed);
                movement *= Time.deltaTime;

                _cc.Move(movement);
                RotateFromMouseVector();

                if (playerInput.shiftPressed)
                {
                    if (sprinting)
                    {
                        moveSpeed = baseSpeed;
                        sprinting = false;
                    }
                    else
                    {
                        moveSpeed = sprintSpeed;
                        sprinting = true;
                    }
                }
            }
        }

        private void RotateFromMouseVector()
        {
            Ray ray = playerCamera.ScreenPointToRay(playerInput.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                var target = hitInfo.point;
                target.y = transform.position.y;

                var qTo = Quaternion.LookRotation(target - transform.position);
                qTo = Quaternion.Slerp(transform.rotation, qTo, 15f * Time.deltaTime);
                rb.MoveRotation(qTo);
            }
        }
    }
}
