using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Feedbacks
{
    public class ShakeRectEffect : MonoBehaviour
    {
        [SerializeField]
        float defaultForce = 0.02f;
        [SerializeField]
        float defaultTime = 0.2f;

        Vector2 origin;

        RectTransform rectTransform;
        private IEnumerator shakeCoroutine;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            origin = rectTransform.anchoredPosition;
        }

        public void ShakeEvent()
        {
            Shake();
        }

        public void Shake(float power, float time)
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            shakeCoroutine = ShakeSpriteCoroutine(power, time);
            StartCoroutine(shakeCoroutine);
        }

        [ContextMenu("Shake")]
        public void Shake()
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            shakeCoroutine = ShakeSpriteCoroutine(defaultForce, defaultTime);
            StartCoroutine(shakeCoroutine);
        }

        private IEnumerator ShakeSpriteCoroutine(float power, float time)
        {
            float t = 0f;
            while (t < time)
            {
                t += Time.deltaTime;
                rectTransform.anchoredPosition = new Vector3(origin.x + Random.Range(-power, power), origin.y + Random.Range(-power, power));
                yield return null;
            }
            rectTransform.anchoredPosition = origin;
        }
    }
}
