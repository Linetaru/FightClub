using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class CharacterRigidbodySimple : CharacterRigidbody
{
    /*public delegate void ActionCollision(Transform transform);
    public event ActionCollision OnWallCollision;
    public event ActionCollision OnGroundCollision;*/

    [Header("CharacterController")]
    [SerializeField]
    private BoxCollider characterCollider;

    [Header("Collision")]
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


    /*protected float characterMotionSpeed = 1;
    public float CharacterMotionSpeed
    {
        get { return characterMotionSpeed; }
        set { characterMotionSpeed = value; }
    }*/


    /*private float speedX = 0;
    public float SpeedX
    {
        get { return speedX; }
        set { speedX = value; }
    }

    private float speedY = 0;
    public float SpeedY
    {
        get { return speedY; }
        set { speedY = value; }
    }*/

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
            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            UpdatePositionX();

            Vector2 offsetX = new Vector2(actualSpeedX, 0);
            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y) + offsetX;
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y) + offsetX;
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y) + offsetX;
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y) + offsetX;

            UpdatePositionY();
            transform.position = new Vector3(transform.position.x + actualSpeedX, transform.position.y + actualSpeedY, 0);
            Physics.SyncTransforms();
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
            Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastX, Mathf.Abs(actualSpeedX), layerMask);
            Debug.DrawRay(originRaycast, new Vector2(actualSpeedX, 0), Color.yellow, 0.5f);
            if (raycastX.collider != null)
            {
                float distance = raycastX.distance - offsetRaycastX;
                actualSpeedX = distance * directionX;
                collisionWallInfo = raycastX.collider.transform;
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
                    isGrounded = true;
                }
                else
                {
                    collisionRoofInfo = raycastY.collider.transform;
                }
            }

            originRaycast += originOffset;
        }

        if (directionY == 1)
        {
            isGrounded = false;
        }
    }
}
