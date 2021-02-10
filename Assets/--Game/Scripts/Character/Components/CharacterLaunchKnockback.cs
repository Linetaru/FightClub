using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLaunchKnockback : MonoBehaviour
{
    private float knockbackMaxTime;
    public float KnockbackMaxTime
    {
        get { return knockbackMaxTime; }
        //set { knockbackMaxTime = value; }
    }

    private float knockbackTime;
    public float KnockbackTime
    {
        get { return knockbackTime; }
        //set { knockbackMTime = value; }
    }

    private Vector2 knockbackPower;
    public Vector2 KnockbackPower
    {
        get { return knockbackPower; }
        //set { knockbackPower = value; }
    }

    private float baseKnockbackTime;
    public float BaseKnockbackTime
    {
        get { return baseKnockbackTime; }
        //set { baseKnockbackTime = value; }
    }

    private float knockbackPowerForWallBounce;
    public float KnockbackPowerForWallBounce
    {
        get { return knockbackPowerForWallBounce; }
        //set { knockbackPowerForWallBounce = value; }
    }

    public CharacterAnimation characterAnimation;

    public event System.Action OnKnockback;


    private float currentSpeedX;
    public float CurrentSpeedX
    {
        get { return currentSpeedX; }
        //set { currentSpeedX = value; }
    }

    private float currentSpeedY;
    public float CurrentSpeedY
    {
        get { return currentSpeedY; }
        //set { currentSpeedY = value; }
    }

    public float GetMotionSpeed()
    {
        return 1;// characterAnimation.speedDelta;
    }

    private void Knockback(AttackController attack, Vector3 knockbackAngle, Vector3 knockbackPower)
    {
        if (attack == null)
            return;

        currentSpeedX = 0;
        currentSpeedY = 0;

        Vector2 direction = this.transform.position - (attack.transform.position + knockbackAngle);
        direction *= knockbackPower;
        knockbackPower = direction;
        //attack.HasHit(this);

        knockbackTime = 0;
        knockbackMaxTime = baseKnockbackTime + (direction.magnitude);

        OnKnockback.Invoke();
    }

    protected void UpdateKnockback()
    {
        if (GetMotionSpeed() == 0)
        {
            return;
        }
        knockbackTime += Time.deltaTime * GetMotionSpeed();
        knockbackPower = Vector2.Lerp(knockbackPower, Vector2.zero, knockbackTime / knockbackMaxTime);

        if (knockbackPower.magnitude < knockbackPowerForWallBounce)
        {
            currentSpeedX = knockbackPower.x;
            currentSpeedY = knockbackPower.y;
            knockbackPower = Vector2.zero;
        }

        if (knockbackPower.magnitude < 1f)
        {
            knockbackTime = 0;
            knockbackMaxTime = 0;
            knockbackPower = Vector2.zero;
        }
    }

    public void AddForce(float forceX, float forceY)
    {
        knockbackMaxTime += new Vector2(forceX, forceY).magnitude;
        knockbackPower += new Vector2(forceX, forceY);
    }
}
