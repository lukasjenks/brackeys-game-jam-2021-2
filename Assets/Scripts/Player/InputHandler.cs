using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class InputHandler : MonoBehaviour
    {
        public Vector2 inputVector { get; private set; }

        public Vector3 mousePosition { get; private set; }

        public bool shiftPressed { get; private set; }

        public bool spacePressed { get; private set; }
        // Update is called once per frame
        void Update()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            inputVector = new Vector2(h, v);

            mousePosition = Input.mousePosition;
            shiftPressed = Input.GetKey(KeyCode.LeftShift);
            spacePressed = Input.GetKeyDown(("space"));
        }
    }
}
