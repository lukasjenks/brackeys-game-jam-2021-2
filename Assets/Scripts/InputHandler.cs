using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public Vector2 InputVector { get; private set; }

    public Vector3 MousePosition { get; private set; }

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        var h = Input.getAxis("Horizontal");
        var v = Input.getAxis("Vertical");
        InputVector = new Vector2(h, v);

        MousePosition = Input.mousePosition;
    }
}
