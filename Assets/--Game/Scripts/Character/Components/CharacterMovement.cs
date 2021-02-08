using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class CharacterMovement : MonoBehaviour
{


    [Title("Stats")]
    [SerializeField]
    private float speed;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    private float maxSpeed;
    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }

    [SerializeField]
    private float acceleration;
    public float Acceleration
    {
        get { return acceleration; }
        set { acceleration = value; }
    }

    [SerializeField]
    private float Deceleration;
    public float deceleration
    {
        get { return deceleration; }
        set { deceleration = value; }
    }


    /*protected bool inAir = false;
    public bool InAir
    {
        get { return inAir; }
    }*/

    protected int direction = 1;
    public int Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    protected float speedX = 0;
    public float SpeedX
    {
        get { return speedX; }
        set { speedX = value; }
    }

    protected float speedY = 0;
    public float SpeedY
    {
        get { return speedY; }
        set { speedY = value; }
    }


    [SerializeField]
    float jumpForce = 10f;

    [SerializeField]
    float gravity = 1f;
    /*protected float speedZ = 0;
    public float SpeedZ
        {
            get { return speedZ; }
        }
    */
    //protected float actualSpeedX = 0;
    //protected float actualSpeedY = 0;

    /*public void InitializeComponent(CharacterBase characterBase)
    {
        character = characterBase;
    }*/




    public float Accelerate()
    {
        if (speed < maxSpeed)
        {
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            speed = maxSpeed;
        }

        return speed;
    }

    public void Decelerate()
    {
        speed -= deceleration * Time.deltaTime;
    }




    public void MoveForward(float multiplier)
    {
        SetSpeed(speed * multiplier * direction, 0) ;
    }

    public void SetSpeed(float newSpeedX, float newSpeedY)
    {
        speedX = newSpeedX;
        speedY = newSpeedY;
    }

    public void SetDirection(int newDirection)
    {
        direction = newDirection;
        /*if (direction == 1)
            spriteRenderer.flipX = false;
        else if (direction == -1)
            spriteRenderer.flipX = true;*/
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
    }
    
    public void Gravity()
    {
        SpeedY -= gravity;
    }

    /*public void SetCharacterMotionSpeed(float newSpeed, float time = 0)
    {
        characterMotionSpeed = newSpeed;
        characterAnimator.speed = characterMotionSpeed;
        if (currentAttackController != null)
            currentAttackController.AttackMotionSpeed(newSpeed);
        if (time > 0)
        {
            StartCoroutine(MotionSpeedCoroutine(time));
        }
    }


    private IEnumerator MotionSpeedCoroutine(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        characterMotionSpeed = defaultMotionSpeed;
        characterAnimator.speed = characterMotionSpeed;
        if (currentAttackController != null)
            currentAttackController.AttackMotionSpeed(characterMotionSpeed);
    }*/


}

