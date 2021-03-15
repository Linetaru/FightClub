using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class CharacterMovement : MonoBehaviour
{
    public CharacterParticle characterParticle;

    [Title("Stats")]
    [SerializeField]
    private float speedMax;
    public float SpeedMax
    {
        get { return speedMax; }
        //set { maxSpeed = value; }
    }

    [SerializeField]
    [HorizontalGroup("Acceleration")]
    private AnimationCurve accelerationCurve;
    [SerializeField]
    [HorizontalGroup("Acceleration", Width = 50)]
    [HideLabel]
    private float timeAccelerationMax = 1;
    private float timeAcceleration = 0;

    [SerializeField]
    [HorizontalGroup("Decceleration")]
    private AnimationCurve decelerationCurve;
    [SerializeField]
    [HorizontalGroup("Decceleration", Width = 50)]
    [HideLabel]
    private float timeDeccelerationMax = 3;
    private float timeDecceleration = 0;


    [Title("Aerial")]
    [SerializeField]
    private float jumpForce;
    public float JumpForce
    {
        get { return jumpForce; }
    }

    [SerializeField]
    private float gravity;
    public float Gravity
    {
        get { return gravity; }
    }

    [SerializeField]
    private float gravityMax;
    public float GravityMax
    {
        get { return gravityMax; }
    }


    [SerializeField]
    [ReadOnly]
    protected int direction = 1;
    public int Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    [SerializeField]
    [ReadOnly]
    protected float speedX = 0;
    public float SpeedX
    {
        get { return speedX; }
        set { speedX = value; }
    }

    [SerializeField]
    [ReadOnly]
    protected float speedY = 0;
    public float SpeedY
    {
        get { return speedY; }
        set { speedY = value; }
    }


    protected float motionSpeed = 1;
    public float MotionSpeed
    {
        get { return motionSpeed; }
        set { motionSpeed = value; }
    }


    /*public void Accelerate()
    {
        Accelerate(1);
    }*/
    public void Accelerate()
    {
        if (timeDecceleration > 0)
        {
            timeDecceleration = 0;
            timeAcceleration = (Mathf.Abs(speedX) / speedMax) * timeAccelerationMax;
        }
        timeAcceleration += (Time.deltaTime * motionSpeed);
        timeAcceleration = Mathf.Clamp(timeAcceleration, 0, timeAccelerationMax);
        float newSpeedX = accelerationCurve.Evaluate(timeAcceleration / timeAccelerationMax) * speedMax;
        if(speedX < newSpeedX)
            speedX = accelerationCurve.Evaluate(timeAcceleration / timeAccelerationMax) * speedMax;
    }


    /*public void Decelerate()
    {
        Decelerate(1);
    }*/
    public void Decelerate()
    {
        if(timeAcceleration > 0)
        {
            timeAcceleration = 0;
            timeDecceleration = (Mathf.Abs(speedX) / speedMax) * timeDeccelerationMax;
        }

        timeDecceleration -= (Time.deltaTime * motionSpeed);
        timeDecceleration = Mathf.Clamp(timeDecceleration, 0, timeDeccelerationMax);
        speedX = decelerationCurve.Evaluate(timeDecceleration / timeDeccelerationMax) * speedMax;
    }

    public void ResetAcceleration()
    {
        timeAcceleration = 0;
        timeDecceleration = 0;
    }



    public void MoveForward(float multiplier)
    {
        SetSpeed(speedMax * multiplier, speedY);
    }

    public void SetSpeed(float newSpeedX, float newSpeedY)
    {
        speedX = newSpeedX;
        speedY = newSpeedY;
    }

    public void SetDirection(int newDirection)
    {
        direction = newDirection;
    }

    public void TurnBack()
    {
        SetDirection(-direction);
    }

    public void MoveToPointInstant(Vector3 point)
    {
        Vector2 direction = point - this.transform.position;
        SetSpeed(direction.x / Time.deltaTime, direction.y / Time.deltaTime);
    }


    public bool MoveToPoint(Vector3 point, float speed)
    {
        Vector2 direction = point - this.transform.position;
        if (Mathf.Abs(direction.magnitude) < 0.1f)
        {
            SetSpeed(0, 0);
            return true;
        }
        else
        {
            direction.Normalize();
            SetSpeed(direction.x * speed, direction.y * speed);
            return false;
        }
    }



    public void Jump()
    {
        Jump(jumpForce);
    }

    public void Jump(float jumpForce)
    {
        speedY = jumpForce;
        characterParticle.UseParticle("jump");
    }
    
    public void ApplyGravity()
    {
        ApplyGravity(1);
    }

    public void ApplyGravity(float multiplier)
    {
        speedY -= ((gravity * multiplier) * motionSpeed) * Time.deltaTime;
        speedY = Mathf.Max(speedY, gravityMax);
    }

    // Variante au cas où
    /*public void ApplyGravity(ref float speed)
    {
        speed -= (((gravity) * motionSpeed) * Time.deltaTime);
        speed = Mathf.Max(speed, gravityMax);
    }*/


}

