using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlam : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void RotToScore()
    {
        anim.Play("CameraRotScore");
    }

    public void RotToGame()
    {
        anim.Play("CameraRotGame");
    }
}
