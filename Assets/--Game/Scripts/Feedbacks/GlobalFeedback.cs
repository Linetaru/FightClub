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
        ParticleSystem guardBreak;
        [SerializeField]
        ScreenShake screenShake;

        [SerializeField]
        CameraZoomController camera;
        [SerializeField]
        Animator cameraAnimator;




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

        public void CameraRotationImpulse(Vector2 impulse, float time)
        {
            camera.CameraRotationImpulse(impulse, time);
            cameraAnimator.SetTrigger("ParryFeedback");
        }

        public void ZoomDramatic(CharacterBase c, float time)
        {
            SuperFeedback();
            guardBreak.Play();
            cameraAnimator.SetTrigger("GuardBreakFeedback");

            TargetsCamera target = new TargetsCamera(c.transform, 1);
            camera.targets.Add(target);
            StartCoroutine(FocusCoroutine(target, time));
        }

        private IEnumerator FocusCoroutine(TargetsCamera target, float time)
        {
            yield return new WaitForSeconds(time);
            camera.targets.Remove(target);
        }
    }
}
