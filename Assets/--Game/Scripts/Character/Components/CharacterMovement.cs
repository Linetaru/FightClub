﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class CharacterMovement : MonoBehaviour
{


    [Title("Stats")]
    [SerializeField]
    private float maxSpeed;
    public float MaxSpeed
    {
        get { return maxSpeed; }
        //set { maxSpeed = value; }
    }

    [SerializeField]
    private float acceleration;
    public float Acceleration
    {
        get { return acceleration; }
        //set { acceleration = value; }
    }

    [SerializeField]
    private float deceleration;
    public float Deceleration
    {
        get { return deceleration; }
        //set { deceleration = value; }
    }



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


    public void Accelerate()
    {
        Accelerate(1);
    }
    public void Accelerate(float multiplier)
    {
        if (speedX < maxSpeed)
        {
            speedX += ((acceleration * multiplier) * motionSpeed) * Time.deltaTime;
        }
        else
        {
            speedX = maxSpeed;
        }
    }


    public void Decelerate()
    {
        Decelerate(1);
    }
    public void Decelerate(float multiplier)
    {
        speedX -= ((deceleration * multiplier) * motionSpeed) * Time.deltaTime;
        speedX = Mathf.Max(0, speedX); 
    }



    public void MoveForward(float multiplier)
    {
        SetSpeed(maxSpeed * multiplier * direction, 0);
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

