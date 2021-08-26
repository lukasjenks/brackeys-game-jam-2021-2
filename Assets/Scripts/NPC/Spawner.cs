using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class Spawner : MonoBehaviour
    {
        public float spawnAmount;
        public float spawnDelay;
        public List<GameObject> spawnTypes;

        public bool active;

        private float _currentTime;

        void Start()
        {
            active = true;
        }

        void Update()
        {
            if (active)
            {
                if (_currentTime < spawnDelay)
                {
                    _currentTime += Time.deltaTime;
                }
                else
                {
                    _currentTime = 0;
                    for (int i = 0; i < spawnAmount; i++)
                    {
                        var index = Random.Range(0, spawnTypes.Count);
                        GameObject newObject = Instantiate(spawnTypes[index], transform.position + new Vector3(i, 0, 0), transform.rotation);
                        NPC.Manager.npcDict[newObject.GetInstanceID().ToString()] = newObject;
                    }
                }
            }
        }
    }
}
