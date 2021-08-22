using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputHandler input;
    // Start is called before the first frame update

    [SerializeField]
    private float moveSpeed;

    void Start()
    {
        input = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        // 3D vector to 2D vector
        var targetVector = new Vector3(input.InputVector.x, 0, input.InputVector.y);

        // Move in dir of aim
        MoveTowardTarget(targetVector);

        // Move in dir of travel
    }

    private void MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;
        transform.Translate(targetVector * moveSpeed);
    }
}
