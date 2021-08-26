using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class Gibber : MonoBehaviour
    {
        public List<GameObject> gibletTypes;

        private List<GameObject> _spawnedGiblets;


        void Start()
        {
            _spawnedGiblets = new List<GameObject>();
            // Activate(transform.position);
        }

        public void Activate(Vector3 position)
        {
            for (int i = 0; i < gibletTypes.Count; i++)
            {
                for (int j = 0; j < Random.Range(3, 5); j++)
                {
                    // spawn the giblets here!
                    GameObject giblet = Instantiate(gibletTypes[i], position, _RandomRotation());
                    Rigidbody rb = giblet.GetComponent<Rigidbody>();
                    rb.AddTorque(_RandomDirection(5f));
                    rb.AddForce(_RandomDirection(200f));
                    _spawnedGiblets.Add(giblet);
                }
            }

            List<GameObject> temporary = new List<GameObject>(_spawnedGiblets);
            _spawnedGiblets = new List<GameObject>();
            StartCoroutine(_DestroyAfterNSeconds(7.0f, temporary));
        }


        private Quaternion _RandomRotation()
        {
            return Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        }

        private Vector3 _RandomDirection(float magnitude)
        {
            return new Vector3(Random.Range(-1, 1) * magnitude, Random.Range(-1, 1) * magnitude, Random.Range(-1, 1) * magnitude);
        }

        private IEnumerator _DestroyAfterNSeconds(float n, List<GameObject> gibs)
        {
            yield return new WaitForSeconds(n);

            for (int i = 0; i < gibs.Count; i++)
            {
                Destroy(gibs[i]);
            }
        }
    }
}
