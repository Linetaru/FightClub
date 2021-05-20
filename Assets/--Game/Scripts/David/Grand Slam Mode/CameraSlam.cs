using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraSlam : MonoBehaviour
{
    [SerializeField]
    private RawImage image;

    Animator anim;

    public bool watchingScore;
    public bool watchingGame;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
    }

    public void WatchGame()
    {
        watchingGame = true;
    }
}
