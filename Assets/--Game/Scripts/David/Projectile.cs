using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Projectile : MonoBehaviour
{
    [Title("Projectile Parameters")]
    [SerializeField]
    private float speedMax = 8.0f;
    public float speedX = 0.0f;
    public float speedY = 0.0f;

    // Fall Management
    [SerializeField]
    private float timeBeforeFall = 0.4f;
    private float timer;
    private bool isFalling;

    //SpeedY Acceleration Duration / SpeedX Deceleration Duration
    private float duration = 2f, currentDuration;

    [SerializeField]
    private AnimationCurve speedYAcceleration;
    [SerializeField]
    private AnimationCurve speedXDeceleration;


    private CharacterBase user;
    public CharacterBase User
    {
        get { return user; }
        set { user = value; }
    }

    private int direction;
    public int Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private CharacterRigidbodySlope rb;


    void Start()
    {
        rb = GetComponent<CharacterRigidbodySlope>();
        speedX = speedMax;
    }

    void Update()
    {
        rb.UpdateCollision(speedX * direction, -speedY);

        if(!isFalling)
        {
            timer += Time.deltaTime;
            if(timer >= timeBeforeFall)
            {
                isFalling = true;
            }
            
        }
        else
        {
            if (currentDuration <= duration)
            {
                //SpeedY Accel
                currentDuration += Time.deltaTime;
                float percent = Mathf.Clamp01(currentDuration / duration);
                float curvePercentY = speedYAcceleration.Evaluate(percent);
                speedY = Mathf.Lerp(0f, speedMax, curvePercentY);

                //SpeedX Decel
                float curvePercentX = speedXDeceleration.Evaluate(percent);
                speedX = Mathf.Lerp(speedMax, 0f, curvePercentX);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
