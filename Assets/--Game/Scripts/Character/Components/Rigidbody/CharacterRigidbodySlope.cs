﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class CharacterRigidbodySlope : CharacterRigidbody
{
    [Title("CharacterController")]
    [SerializeField]
    private BoxCollider characterCollider;

    [Title("Collision")]
    [SerializeField]
    private bool collision = true;

    [SerializeField] 
    private LayerMask layerMask;

    [HorizontalGroup("RaycastOffset")]
    [SerializeField]
    private float offsetRaycastX = 0.001f;
    [HorizontalGroup("RaycastOffset")]
    [SerializeField]
    private float offsetRaycastY = 0.001f;

    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    private int numberRaycastVertical = 2;
    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    private int numberRaycastHorizontal = 2;


    [Title("Angle")]
    [SerializeField]
    private float maxSlopeAngle = 45;

    private float actualSpeedX = 0;
    private float actualSpeedY = 0;


    Vector2 bottomLeft;
    Vector2 upperLeft;
    Vector2 bottomRight;
    Vector2 upperRight;

    Transform collisionInfo;


    private Transform collisionWallInfo;
    public override Transform CollisionWallInfo
    {
        get { return collisionWallInfo; }
    }

    private Transform collisionGroundInfo;
    public override Transform CollisionGroundInfo
    {
        get { return collisionGroundInfo; }
    }

    private Transform collisionRoofInfo;
    public override Transform CollisionRoofInfo
    {
        get { return collisionRoofInfo; }
    }

    private bool isGrounded = false;
    public override bool IsGrounded
    {
        get { return isGrounded; }
        //set { isGrounded = value; }
    }


    private void CalculateBounds(Vector2 offset)
    {
        bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y) + offset;
        upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y) + offset;
        bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y) + offset;
        upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y) + offset;
    }



    public override void UpdateCollision(float speedX, float speedY)
    {
        isGrounded = false;
        collisionWallInfo = null;
        collisionGroundInfo = null;
        collisionRoofInfo = null;

        actualSpeedX = speedX;
        actualSpeedY = speedY;
        actualSpeedX *= Time.deltaTime;
        actualSpeedY *= Time.deltaTime;

        if (collision == true)
        {
            CalculateBounds(Vector2.zero);

            float slopeAngle = CheckSlope();
            if (slopeAngle <= maxSlopeAngle) // On climb
            {
                float newSlopeAngle = slopeAngle;
                float speedYSaved = actualSpeedY;
                do // C'est pour géré des pentes successives
                {
                    slopeAngle = newSlopeAngle;
                    UpdatePositionY();
                    Vector2 offset = new Vector2(0, actualSpeedY);
                    CalculateBounds(offset);

                    speedYSaved = actualSpeedY;
                    newSlopeAngle = CheckSlope();

                } while (newSlopeAngle <= maxSlopeAngle && newSlopeAngle != slopeAngle);

                actualSpeedY = speedYSaved;
                UpdatePositionX();
            }
            else // On climb pas ou on touche un mur 
            {
                UpdatePositionX();
                Vector2 offsetX = new Vector2(actualSpeedX, 0);
                CalculateBounds(offsetX);
                UpdatePositionY();
            }



            transform.position = new Vector3(transform.position.x + actualSpeedX, transform.position.y + actualSpeedY, 0);
            Physics.SyncTransforms();
        }
    }



    private float CheckSlope()
    {
        RaycastHit raycastX;
        float directionX = Mathf.Sign(actualSpeedX);
        Vector2 originRaycast = (directionX == -1) ? bottomLeft : bottomRight;
        Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastX, Mathf.Abs(actualSpeedX) + offsetRaycastX, layerMask);
        if (raycastX.collider != null)
        {
            float slopeAngle = Vector2.Angle(raycastX.normal, Vector2.up);
            if (slopeAngle <= maxSlopeAngle)
            {
                float distance = raycastX.distance - offsetRaycastX;
                ClimbSlope(slopeAngle);
                actualSpeedX += distance * directionX;
            }
            return slopeAngle;
        }
        return 9999;
    }


    private void ClimbSlope(float angle)
    {
        float climbSpeedY = Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX);
        if (actualSpeedY <= climbSpeedY)
        {
            actualSpeedY = climbSpeedY;
            actualSpeedX = Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX) * Mathf.Sign(actualSpeedX);
        }
    }




    private void UpdatePositionX()
    {
        if (actualSpeedX == 0)
            return;
         
        RaycastHit raycastX;
        float directionX = Mathf.Sign(actualSpeedX);
        Vector2 originRaycast = (directionX == -1) ? bottomLeft : bottomRight;
        Vector2 originOffset = (upperRight - bottomRight) / (numberRaycastHorizontal - 1);

        for (int i = 0; i < numberRaycastHorizontal; i++)
        {
            Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastX, Mathf.Abs(actualSpeedX) + offsetRaycastX, layerMask);
            Debug.DrawRay(originRaycast, new Vector2(actualSpeedX, 0) , Color.yellow, 0.5f);
            if (raycastX.collider != null)
            {
                float distance = raycastX.distance - offsetRaycastX;
                actualSpeedX = distance * directionX;
                //collisionWallInfo = raycastX.collider.transform;
            }
            originRaycast += originOffset;
        }
    }


    private void UpdatePositionY()
    {
        if (actualSpeedY == 0)
            return;

        RaycastHit raycastY;
        float directionY = Mathf.Sign(actualSpeedY);
        Vector2 originRaycast = (directionY == -1) ? bottomLeft: upperLeft;
        Vector2 originOffset = (upperRight - upperLeft) / (numberRaycastVertical - 1);

        for (int i = 0; i < numberRaycastVertical; i++)
        {
            Physics.Raycast(originRaycast, new Vector2(0, actualSpeedY), out raycastY, Mathf.Abs(actualSpeedY), layerMask);
            Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.red, 0.5f);
            if (raycastY.collider != null)
            {
                float distance = raycastY.distance - offsetRaycastY;
                actualSpeedY = distance * directionY;

                if(directionY == -1)
                {
                    collisionGroundInfo = raycastY.collider.transform;
                }
                else
                {
                    Debug.Log("Woof");
                    collisionRoofInfo = raycastY.collider.transform;
                }
            }

            originRaycast += originOffset;
        }

        if (directionY == -1)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }




}
