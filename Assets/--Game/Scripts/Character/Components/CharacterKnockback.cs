using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Feedbacks;

public class CharacterKnockback : MonoBehaviour
{
    private Vector3 contactPoint;
    public Vector3 ContactPoint
    {
        get { return contactPoint; }
        set { contactPoint = value; }
    }

    [Title("FeedbackComponents")]
    [SerializeField]
    private ShakeEffect shakeEffect;
    public ShakeEffect ShakeEffect
    {
        get { return shakeEffect; }
    }

    //================================================================================

    Vector2 angleKnockback;
    public float knockBackPower;

    public Vector2 GetAngleKnockback()
    {
        return angleKnockback;
    }

    public float GetPowerKnockback()
    {
        return knockBackPower;
    }

    public void Launch(Vector2 angle)
    {
        angleKnockback =  angle;
    }

    public void UpdateKnockback()
    {
        Vector2 tmp = angleKnockback;
        if (angleKnockback.x > 0.25)
            tmp.x -= Time.deltaTime * knockBackPower;
        else if (angleKnockback.x < -0.25)
            tmp.x += Time.deltaTime * knockBackPower;
        else
            tmp.x = 0;
        tmp.y -= Time.deltaTime * knockBackPower;
        angleKnockback = tmp;
    }
}
