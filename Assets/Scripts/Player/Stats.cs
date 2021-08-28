using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// If I'm being honest, I have no idea how this works, but it does - Zach

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
        private float _rechargeTime = 5.0f;
        private bool _recentTakenDamage = false;
        private List<string> _recentDamageIndicators = new List<string>();
        private CanvasGroup _canvasG;
        void Start()
        {
            _canvasG = GetComponentInChildren<CanvasGroup>();
        }

        void Update()
        {

            // Debug.Log(_canvasG.alpha);
            // Debug.Log(_numHits);
            // if (_IsDead())
            // {
            //     Debug.Log("IM DEAD");
            // }

            if (_numHits > 0)
            {
                EasingFunction.Ease ease = EasingFunction.Ease.Linear;
                EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);
                _canvasG.alpha = func(_canvasG.alpha, (_numHits / 10), (_numHits / 10) * Time.deltaTime);
            }
            else
            {
                EasingFunction.Ease ease = EasingFunction.Ease.Linear;
                EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);
                _canvasG.alpha = func(_canvasG.alpha, 0.0f, 0.25f * Time.deltaTime);
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
                    _recentDamageIndicators.Add("HIT");
                    StartCoroutine(_TakeDamageCanvas(1.5f));
                    StartCoroutine(_GiveLifeBack(1.5f));
                    break;
                case HitType.CREEPER:
                    _recentDamageIndicators.Add("HIT");
                    _numHits += 1.0f;
                    StartCoroutine(_TakeDamageCanvas(1.5f));
                    StartCoroutine(_GiveLifeBack(1.5f));
                    break;
                case HitType.HUNTER:
                    _recentDamageIndicators.Add("HIT");
                    _numHits += 1.5f;
                    StartCoroutine(_TakeDamageCanvas(1.5f));
                    StartCoroutine(_GiveLifeBack(1.5f));
                    break;
                default:
                    break;
            }
        }

        private IEnumerator _TakeDamageCanvas(float value)
        {

            yield return new WaitForSeconds(value);
            _recentTakenDamage = false;
            _recentDamageIndicators.RemoveAt(0);
        }

        private IEnumerator _RemoveDamageCanvas(float value)
        {
            EasingFunction.Ease ease = EasingFunction.Ease.Linear;
            EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);

            var elapsedTime = 0.0f;
            var convertedValue = value / 10;
            var originalValue = _canvasG.alpha;

            while (elapsedTime < 1.5f)
            {
                _canvasG.alpha = func(_canvasG.alpha, 0.0f, (elapsedTime / 1.5f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator _GiveLifeBack(float value)
        {
            var elapsedTime = 0.0f;
            while (elapsedTime < _rechargeTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (_recentDamageIndicators.Count == 0)
            {
                _numHits = 0;
                yield return _RemoveDamageCanvas(0);
            }
        }
    }
}
