using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        public GameObject player;

        [SerializeField]
        private float xOffset;
        [SerializeField]
        private float yOffset;
        [SerializeField]
        private float zOffset;

        // Update is called once per frame
        void Update()
        {
            Vector3 playerPos = player.transform.position;
            playerPos.x = playerPos.x - xOffset;
            playerPos.y = playerPos.y - yOffset;
            playerPos.z = playerPos.z - zOffset;
            // adjust camera pos with calc'ed vals
            //transform.position = playerPos;
            transform.position = Vector3.MoveTowards(transform.position, playerPos, 1);
        }
    }

}
