using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public enum HitType
    {
        CHASER,
        CREEPER,
        HUNTER
    }

    public class Stats : MonoBehaviour
    {
        private float _numHits = 0f;
        private float _rechargeTime = 2.0f;

        void Update()
        {
            // Debug.Log(_numHits);
            if (_IsDead())
            {
                Debug.Log("IM DEAD");
            }
        }

        private bool _IsDead()
        {
            return _numHits >= 8 ? true : false;
        }

        public void GetHit(HitType t)
        {
            switch (t)
            {
                case HitType.CHASER:
                    _numHits += 0.5f;
                    Debug.Log("HIT BY CHASER");
                    StartCoroutine(_GiveLifeBack(0.5f));
                    break;
                case HitType.CREEPER:
                    _numHits += 1.0f;
                    Debug.Log("HIT BY CREEPER");
                    StartCoroutine(_GiveLifeBack(1.0f));
                    break;
                case HitType.HUNTER:
                    _numHits += 1.5f;
                    Debug.Log("HIT BY HUNTER");
                    StartCoroutine(_GiveLifeBack(1.5f));
                    break;
                default:
                    break;
            }
        }

        private IEnumerator _GiveLifeBack(float value)
        {
            yield return new WaitForSeconds(_rechargeTime);
            _numHits -= value;
        }
    }
}
