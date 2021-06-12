using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraSlam : MonoBehaviour
{
    public Camera camera;

    [SerializeField]
    private MeshRenderer background;

    [SerializeField]
    private Material blurMat;

    [SerializeField]
    private float smoothnessForBlur = 0.45f;

    private float currentBlurValue = 1.0f, startValue;

    [SerializeField]
    private float timeToBlur = 1.0f;
    [SerializeField]
    private float timeToClear = 1.0f;

    private float timer;

    private bool setBlur, removeBlur;


    Animator anim;

    public bool watchingScore;
    public bool watchingGame;

    private void Start()
    {
        anim = GetComponent<Animator>();
        camera = GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        blurMat.SetFloat("_Smoothness", 1.0f);
    }

    private void Update()
    {
        if(setBlur)
        {
            if(timer < timeToBlur)
            {
                LerpBlur(startValue, smoothnessForBlur, timeToBlur);
            }
            else
            {
                setBlur = false;
            }
        }

        if(removeBlur)
        {
            if (timer < timeToClear)
            {
                LerpBlur(startValue, 1.0f, timeToClear);
            }
            else
            {
                removeBlur = false;
                blurMat.SetFloat("_Smoothness", 1.0f);
                background.enabled = false;
            }
        }
    }

    public void RotToScore()
    {
        Debug.Log("Rotating To Score");
        watchingGame = false;
        anim.Play("CameraRotScore");
    }

    public void RotToGame()
    {
        Debug.Log("Rotating To Game");
        watchingScore = false;
        anim.Play("CameraRotGame");
    }

    public void WatchScore()
    {
        watchingScore = true;
        ActivateBackgroundBlur();
    }

    public void WatchGame()
    {
        watchingGame = true;
    }

    public void DrawScore()
    {

    }

    public void RemoveScore()
    {

    }

    public void ActivateBackgroundBlur()
    {
        timer = 0f;
        background.enabled = true;
        startValue = currentBlurValue;
        setBlur = true;
    }

    public void RemoveBackgroundBlur()
    {
        timer = 0f;
        startValue = currentBlurValue;
        removeBlur = true;
    }

    public void LerpBlur(float start, float target, float timerLerp)
    {
        currentBlurValue = Mathf.Lerp(start, target, timer / timerLerp);
        blurMat.SetFloat("_Smoothness", currentBlurValue);
        timer += Time.deltaTime;
    }
}
