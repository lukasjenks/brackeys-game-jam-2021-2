using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler playerInput;

    [SerializeField]
    private bool rotateTowardMouse;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Camera playerCamera;

    private InputHandler input;

    private Rigidbody rb;
    // Start is called before the first frame update

    private float moveSpeed;

    [SerializeField]
    private float baseSpeed;

    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float speedShiftMultiplier;

    [SerializeField]
    private float jumpForce;

    private bool sprinting;

    [SerializeField]
    private int interpolationRatio;

    private int elapsedFrames;

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

        if (!rotateTowardMouse) {
            RotateTowardMovementVector(movementVector);
        }
        if (rotateTowardMouse) {
            RotateFromMouseVector();
        }

        if (playerInput.shiftPressed) {
            if (sprinting) {
                moveSpeed = baseSpeed;
                sprinting = false;
            } else {
                moveSpeed = sprintSpeed;
                sprinting = true;
            }
        }

        if (playerInput.spacePressed) {
            rb.velocity = new Vector3 (rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void RotateFromMouseVector()
    {
        Ray ray = playerCamera.ScreenPointToRay(playerInput.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f)) {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, playerCamera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { 
            return; 
        }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
    }
}
