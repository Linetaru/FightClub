using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShadow : MonoBehaviour
{
    [SerializeField]
    Transform ball;

    float posX;

    // Update is called once per frame
    void Update()
    {
        posX = ball.position.x;
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        //newPosition = new Vector3(posX, ball.position.y, ball.position.z);
    }
}
