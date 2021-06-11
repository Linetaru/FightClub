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
using Sirenix.OdinInspector;

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

        [Space]
        [Title("Optional")]
        [SerializeField]
        Transform shakeTarget = null;

        private IEnumerator shakeCoroutine = null;

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
            if(shakeTarget != null)
                shakeCoroutine = ShakeSpriteCoroutine(shakeTarget, power, time);
            else
                shakeCoroutine = ShakeSpriteCoroutine(this.transform, power, time);
            StartCoroutine(shakeCoroutine);
        }

        [ContextMenu("Shake")]
        public void Shake()
        {
            Shake(defaultForce, defaultTime);
            /*if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            if(shakeTarget != null)
                shakeCoroutine = ShakeSpriteCoroutine(shakeTarget, defaultForce, defaultTime);
            else
                shakeCoroutine = ShakeSpriteCoroutine(this.transform, defaultForce, defaultTime);
            StartCoroutine(shakeCoroutine);*/
        }

        // Pour les animators parce que unity a du mal
        public void ShakeCallback()
        {
            Shake(defaultForce, defaultTime);

        }

        private IEnumerator ShakeSpriteCoroutine(Transform transform, float power, float time)
        {
            float t = 0f;
            while (t < time)
            {
                t += Time.deltaTime;
                transform.localPosition = new Vector3(0 + Random.Range(-power, power), 0 + Random.Range(-power, power), 0 + Random.Range(-power, power));
                yield return null;
            }
            transform.localPosition = new Vector3(0, 0 , 0);
        }




        /*private IEnumerator ShakeSpriteCoroutine(float power, float time)
        {
            float t = 0f;
            while (t < time)
            {
                t += Time.deltaTime;
                this.transform.localPosition = new Vector3(0 + Random.Range(-power, power), this.transform.localPosition.y, this.transform.localPosition.z);
                yield return null;
            }
            this.transform.localPosition = new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z);
        }*/

        #endregion

    } // Shake class
	
}// #PROJECTNAME# namespace
