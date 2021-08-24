using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputHandler input;

    private Rigidbody rb;
    // Start is called before the first frame update

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float speedShiftMultiplier;

    [SerializeField]
    private float jumpForce;

    void Start()
    {
        input = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 3D vector to 2D vector
        var targetVector = new Vector3(input.inputVector.x, 0, input.inputVector.y);

        // Move in dir of aim
        MoveTowardTarget(targetVector);

        if (input.shiftPressed) {
            moveSpeed = moveSpeed * speedShiftMultiplier;
        }

        if (input.spacePressed) {
            rb.velocity = new Vector3 (rb.velocity.x, jumpForce, rb.velocity.z);
        }

        // Move in dir of travel
    }

    private void MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;
        transform.Translate(targetVector * moveSpeed);
    }
}
