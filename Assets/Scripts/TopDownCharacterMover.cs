using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool isGrounded;

    private bool isJumping; // for animations later

    private int elapsedFrames;

    private Vector3 _velocity;
    private Vector3 _lastFramePosition;

    private float _backwardForce;
    public float BackwardForce
    {
        get { return _backwardForce; }
        set { _backwardForce = value; }
    }

    public Vector3 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }
    private InputHandler input;

    private Rigidbody rb;

    private float moveSpeed; // calculated from baseSpeed and sprintSpeed

    private void Awake()
    {
        playerInput = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        // initialize moveSpeed to base speed on first frame
        moveSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        var targetVector = new Vector3(playerInput.inputVector.x, 0, playerInput.inputVector.y);
        var movementVector = MoveTowardTarget(targetVector);
        _velocity = _lastFramePosition - transform.position;

        if (_backwardForce > 0)
        {
            // var destination = -transform.forward;
            // destination.y = transform.position.y;
            // transform.position = Vector3.MoveTowards(transform.position, destination, (_backwardForce / 2));
            rb.AddForce(-transform.forward * _backwardForce);
            _backwardForce = _backwardForce - _backwardForce / 2;

        }

        if (!rotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);
        }
        if (rotateTowardMouse)
        {
            RotateFromMouseVector();
        }

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

        CheckGroundStatus();
        if (isGrounded)
        {
            if (playerInput.spacePressed)
            {
                // jump
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                isGrounded = false;
                isJumping = true;
                groundCheckDist = 0.1f; // just before hitting the ground again
            }
        }

        _lastFramePosition = transform.position;
    }

    private void RotateFromMouseVector()
    {
        Ray ray = playerCamera.ScreenPointToRay(playerInput.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        targetVector = Quaternion.Euler(0, playerCamera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (moveSpeed / 10));
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0)
        {
            return;
        }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
    }

    private void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, groundCheckDist))
        {
            isGrounded = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
            isJumping = true;
        }
    }
}
