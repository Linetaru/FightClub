using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class CharacterRigidbody : MonoBehaviour
{
    public virtual Transform CollisionWallInfo
    {
        get { return null; }
    }
    public virtual Transform CollisionGroundInfo
    {
        get { return null; }
    }
    public virtual Transform CollisionRoofInfo
    {
        get { return null; }
    }

    public virtual bool IsGrounded
    {
        get { return true; }
    }

    public virtual void UpdateCollision(float speedX, float speedY)
    {

    }



}
