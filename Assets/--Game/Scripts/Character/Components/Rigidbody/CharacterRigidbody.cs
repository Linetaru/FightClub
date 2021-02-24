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

    /// <summary>
    /// Modifie le layerMask utilisé pour les collisions
    /// </summary>
    /// <param name="newLayerMask"></param>
    /// <param name="groundLayerMask"></param>
    public virtual void SetNewLayerMask(LayerMask newLayerMask, bool groundLayerMask = false)
    {

    }

    /// <summary>
    /// Reset le layerMask utilisé pour les collisions à celui de base
    /// </summary>
    public virtual void ResetLayerMask()
    {

    }

    public virtual void UpdateCollision(float speedX, float speedY)
    {

    }



}
