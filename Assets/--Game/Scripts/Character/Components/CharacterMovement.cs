using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class CharacterMovement : MonoBehaviour
{
    //[Title("CharacterController")]
    //[SerializeField]
    //protected SpriteRenderer spriteRenderer;
    /*public SpriteRenderer SpriteRenderer
    {
        get { return spriteRenderer; }
    }*/


    //protected CharacterBase character;
    /*public CharacterBase Character
    {
        get { return character; }
    }*/

    [Title("Stats")]
    [SerializeField]
    private float speed;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
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

    /*public void ApplyGravity(float gravity, float gravityMax)
    {
        if (inAir == true)
        {
            speedZ -= ((gravity * Time.deltaTime) * character.MotionSpeed);
            speedZ = Mathf.Max(speedZ, gravityMax);
            spriteRenderer.transform.localPosition += new Vector3(0, (speedZ * Time.deltaTime) * character.MotionSpeed, 0);
            if (spriteRenderer.transform.localPosition.y <= 0 && character.MotionSpeed != 0)
            {
                inAir = false;
                speedZ = 0;
                spriteRenderer.transform.localPosition = new Vector3(spriteRenderer.transform.localPosition.x, 0, spriteRenderer.transform.localPosition.z);
                //OnGroundCollision();
            }
        }
    }*/


    public void Jump(float impulsion)
    {
        //speedZ = impulsion;
    }





    public void MoveForward(float multiplier)
    {
        SetSpeed(speed * multiplier * direction, 0);
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

