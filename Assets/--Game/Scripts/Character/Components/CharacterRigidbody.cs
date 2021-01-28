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

    public virtual void UpdateCollision(float speedX, float speedY)
    {

    }



}
