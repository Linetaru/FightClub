using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feedbacks 
{

    public class GlobalFeedback : MonoBehaviour
    {

        [SerializeField]
        ParticleSystem speedlines;
        [SerializeField]
        ScreenShake screenShake;

        private static GlobalFeedback _instance;
        public static GlobalFeedback Instance { get { return _instance; } }


        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }


        public void SuperFeedback()
        {
            speedlines.Play();
            screenShake.StartScreenShake(0.1f, 0.1f);
    }

    }
}
