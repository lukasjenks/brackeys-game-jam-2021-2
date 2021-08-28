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
                for (int j = 0; j < Random.Range(2, 8); j++)
                {
                    // spawn the giblets here!
                    GameObject giblet = Instantiate(gibletTypes[i], position, _RandomRotation());
                    giblet.transform.localScale = _RandomDirection(Random.Range(1, 3), false);
                    Rigidbody rb = giblet.GetComponent<Rigidbody>();
                    rb.AddTorque(_RandomDirection(2f, true));
                    rb.AddForce(_RandomDirection(200f, true));
                    _spawnedGiblets.Add(giblet);
                }
            }

            List<GameObject> temporary = new List<GameObject>(_spawnedGiblets);
            _spawnedGiblets = new List<GameObject>();
            StartCoroutine(_WaitThenFade(5.0f, temporary));
        }


        private Quaternion _RandomRotation()
        {
            return Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        }

        private Vector3 _RandomDirection(float magnitude, bool canBeNegative)
        {
            if (canBeNegative)
                return new Vector3(Random.Range(-1, 1) * magnitude, Random.Range(-1, 1) * magnitude, Random.Range(-1, 1) * magnitude);
            else
                return new Vector3(Random.Range(0.5f, 1) * magnitude, Random.Range(0.5f, 1) * magnitude, Random.Range(0.5f, 1) * magnitude);
        }

        private IEnumerator _WaitThenFade(float n, List<GameObject> gibs)
        {
            var elapsedTime = 0.0f;

            while (elapsedTime < n)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return _DestroyAfterNSeconds(2.5f, gibs);
        }

        private IEnumerator _DestroyAfterNSeconds(float n, List<GameObject> gibs)
        {

            EasingFunction.Ease ease = EasingFunction.Ease.EaseOutQuart;
            EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);

            var elapsedTime = 0.0f;

            while (elapsedTime < n)
            {
                for (int i = 0; i < gibs.Count; i++)
                {
                    if (gibs[i] != null)
                    {
                        MeshRenderer renderer = gibs[i].GetComponent<MeshRenderer>();
                        Color color = renderer.material.color;
                        color.a = func(color.a, 0.1f, (elapsedTime / n));
                        renderer.material.SetColor("_Color", color);
                        elapsedTime += Time.deltaTime;

                        if (color.a < 0.3f)
                        {
                            Destroy(gibs[i]);
                        }
                    }
                }
                yield return null;
            }
        }
    }
}
