using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementJump : MonoBehaviour
{

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
}
