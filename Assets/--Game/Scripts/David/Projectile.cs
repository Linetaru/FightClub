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

    private bool isColliding;

    // Fall Management
    [SerializeField]
    private float timeBeforeFall = 0.4f;
    private float timerFall;
    private bool isFalling;

    //SpeedY Acceleration Duration / SpeedX Deceleration Duration
    private float duration = 2f, currentDuration;

    [SerializeField]
    private AnimationCurve speedYAcceleration;
    [SerializeField]
    private AnimationCurve speedXDeceleration;

    private bool isReadyToExplode;



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
        GetComponentInChildren<AttackSubManager>().InitAttack(User);
        GetComponentInChildren<AttackSubManager>().ActionActive();
        speedX = speedMax;
    }

    void Update()
    {
        SpeedManager();
    }

    public void ReadyToExplode()
    {
        Debug.Log("Explode");
        isReadyToExplode = true;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void SpeedManager()
    {
        if (!isColliding)
        {
            rb.UpdateCollision(speedX * direction, -speedY);

            if (rb.CollisionGroundInfo != null || rb.CollisionWallInfo.Collision != null)
                isColliding = true;

            if (!isFalling)
            {
                timerFall += Time.deltaTime;
                if (timerFall >= timeBeforeFall)
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
        else
        {
            if(!isReadyToExplode)
                ReadyToExplode();
        }
    }
}
