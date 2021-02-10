/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Feedbacks
{
	public class ShakeEffect : MonoBehaviour
	{
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        float defaultForce = 0.02f;
        [SerializeField]
        float defaultTime = 0.2f;


        Transform spriteTransform;
        private IEnumerator shakeCoroutine;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

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
                this.transform.localPosition = new Vector3(0 + Random.Range(-power, power), this.transform.localPosition.y, this.transform.localPosition.z);
                yield return null;
            }
            this.transform.localPosition = new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z);
        }

        #endregion

    } // Shake class
	
}// #PROJECTNAME# namespace
