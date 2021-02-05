using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class CharacterRigidbodyLine : CharacterRigidbody
{
    public delegate void ActionCollision(Transform transform);
    public event ActionCollision OnWallCollision;
    public event ActionCollision OnGroundCollision;

    [Header("CharacterController")]
    [SerializeField]
    private BoxCollider characterCollider;


    [Header("Collision")]
    [SerializeField]
    private bool collision = true;

    [HorizontalGroup("RaycastOffset")]
    [SerializeField]
    private float offsetRaycastX = 0.0001f;
    [HorizontalGroup("RaycastOffset")]
    [SerializeField]
    private float offsetRaycastY = 0.0001f;

    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    private int numberRaycastVertical = 2;
    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    private int numberRaycastHorizontal = 2;

    [SerializeField] private LayerMask[] collisionMasks;


    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    private float maxAngle = 45;

    /*protected float characterMotionSpeed = 1;
    public float CharacterMotionSpeed
    {
        get { return characterMotionSpeed; }
        set { characterMotionSpeed = value; }
    }*/


    private float speedX = 0;
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
    }

    private float actualSpeedX = 0;
    private float actualSpeedY = 0;


    int layerMask;
    Vector2 bottomLeft;
    Vector2 upperLeft;
    Vector2 bottomRight;
    Vector2 upperRight;

    Transform collisionInfo;

    float climbingAngle = 0;
    bool climbingSlope = false;
    bool descendingSlope = false;


    private Transform collisionWallInfo;
    public override Transform CollisionWallInfo
    {
        get { return collisionWallInfo; }
    }

    private Transform collisionGroundInfo;
    public Transform CollisionGroundInfo
    {
        get { return collisionGroundInfo; }
    }

    private Transform collisionRoofInfo;
    public Transform CollisionRoofInfo
    {
        get { return collisionRoofInfo; }
    }


    private bool isGrounded = false;
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }

    private void Start()
    {
        for (int i = 0; i < collisionMasks.Length; i++)
        {
            layerMask |= collisionMasks[i];
        }
    }

    /*protected void Update()
    {
        UpdateCollision();
    }*/

    public override void UpdateCollision(float speedX, float speedY)
    {
        climbingAngle = 0;
        climbingSlope = false;
        descendingSlope = false;
        collisionWallInfo = null;

        actualSpeedX = speedX;
        actualSpeedY = speedY;
        actualSpeedX *= Time.deltaTime;
        actualSpeedY *= Time.deltaTime;
        //actualSpeedX *= characterMotionSpeed;
        //actualSpeedY *= characterMotionSpeed;

        if (collision == true)
        {
            //layerMask = 1 << 15 | 1 << 13;

            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            if (actualSpeedY < 0)
            {
                DescendSlope();
            }

            UpdatePositionX();
            //transform.position = new Vector3(transform.position.x + (actualSpeedX * Time.deltaTime), transform.position.y, 0);
            //Physics.SyncTransforms();

            Vector2 offsetX = new Vector2(actualSpeedX, 0);
            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y) + offsetX;
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y) + offsetX;
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y) + offsetX;
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y) + offsetX;

            UpdatePositionY();
            transform.position = new Vector3(transform.position.x + actualSpeedX, transform.position.y + actualSpeedY, 0);
            Physics.SyncTransforms();
        }

        //transform.position = new Vector3(transform.position.x + (actualSpeedX * Time.deltaTime), transform.position.y + (actualSpeedY * Time.deltaTime), 0);
    }

    private void UpdatePositionX()
    {
        RaycastHit raycastX;
        Vector2 originRaycast;

        if (actualSpeedX < 0)
        {
            // ======================================================================================================
            originRaycast = bottomLeft;// - new Vector2(offsetRaycastX, 0);
            for (int i = 0; i < numberRaycastHorizontal; i++)
            {
                Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastX, Mathf.Abs(actualSpeedX), layerMask);
                Debug.DrawRay(originRaycast, new Vector2(actualSpeedX, 0), Color.yellow, 1f);
                if (raycastX.collider != null)
                {
                    float slopeAngle = Vector2.Angle(raycastX.normal, Vector2.up);
                    if (i == 0 && slopeAngle <= maxAngle)
                    {
                        if (descendingSlope)
                        {
                            descendingSlope = false;
                        }
                        float distance2 = -raycastX.distance + offsetRaycastX;
                        ClimbSlope(slopeAngle);
                        actualSpeedX += distance2;
                    }
                    else
                    {
                        collisionInfo = raycastX.collider.transform;
                        float distance = -raycastX.distance + offsetRaycastX;
                        actualSpeedX = distance;
                        collisionWallInfo = collisionInfo;
                        OnWallCollision?.Invoke(collisionInfo);
                        //return;
                    }
                }
                originRaycast += new Vector2(0, Mathf.Abs(upperLeft.y - bottomLeft.y) / (numberRaycastHorizontal - 1));
            }
            // ======================================================================================================

        }
        else if (actualSpeedX > 0)
        {
            // ======================================================================================================
            originRaycast = bottomRight;// + new Vector2(offsetRaycastX, 0);
            for (int i = 0; i < numberRaycastHorizontal; i++)
            {
                Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastX, Mathf.Abs(actualSpeedX), layerMask);
                Debug.DrawRay(originRaycast, new Vector2(actualSpeedX, 0), Color.yellow, 0.5f);
                if (raycastX.collider != null)
                {

                    float slopeAngle = Vector2.Angle(raycastX.normal, Vector2.up);
                    if (i == 0 && slopeAngle <= maxAngle)
                    {
                        if (descendingSlope)
                        {
                            descendingSlope = false;
                        }
                        float distance2 = raycastX.distance - offsetRaycastX;
                        ClimbSlope(slopeAngle);
                        actualSpeedX += distance2;
                    }
                    else
                    {
                        collisionInfo = raycastX.collider.transform;
                        float distance = raycastX.distance - offsetRaycastX;
                        actualSpeedX = distance;
                        collisionWallInfo = collisionInfo;
                        OnWallCollision?.Invoke(collisionInfo);
                        //return;
                    }
                }
                originRaycast += new Vector2(0, Mathf.Abs(upperRight.y - bottomRight.y) / (numberRaycastHorizontal - 1));
            }
            // ======================================================================================================

        }
    }


    private void UpdatePositionY()
    {
        RaycastHit raycastY;
        Vector2 originRaycast;
        bool test = false; // a supprimer
        //float minDistance = 999999;

        if (actualSpeedY < 0)
        {
            // ======================================================================================================
            originRaycast = bottomLeft;// - new Vector2(0, offsetRaycastY);
            for (int i = 0; i < numberRaycastVertical; i++)
            {
                Physics.Raycast(originRaycast, new Vector2(0, actualSpeedY), out raycastY, Mathf.Abs(actualSpeedY), layerMask);
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.red, 0.5f);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y - originRaycast.y;
                    distance += offsetRaycastY; 
                    actualSpeedY = distance;
                    if (climbingSlope) // Pour gérer les plafond dans les pentes
                    {
                        actualSpeedX = actualSpeedY / Mathf.Tan(climbingAngle * Mathf.Deg2Rad) * Mathf.Sign(actualSpeedX);
                    }
                    test = true;
                    /*if (Mathf.Abs(distance) < minDistance)
                    {
                        minDistance = Mathf.Abs(distance);
                        actualSpeedY = distance;
                        isGrounded = true;
                        //OnWallCollision?.Invoke(collisionInfo);
                    }*/
                    //return;
                }
                originRaycast += new Vector2(Mathf.Abs(bottomRight.x - bottomLeft.x) / (numberRaycastVertical - 1), 0);
            }
            if (test == false)
                Debug.Log("Je ne touche pas le sol");
            isGrounded = false;
            // ======================================================================================================

        }
        else if (actualSpeedY > 0)
        {
            isGrounded = false;
            // ======================================================================================================
            originRaycast = upperLeft;// + new Vector2(0, offsetRaycastY);
            for (int i = 0; i < numberRaycastVertical; i++)
            {
                Physics.Raycast(originRaycast, new Vector2(0, actualSpeedY), out raycastY, Mathf.Abs(actualSpeedY), layerMask);
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.yellow, 0.5f);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y - originRaycast.y;
                    distance -= offsetRaycastY;
                    actualSpeedY = distance;
                    OnWallCollision?.Invoke(collisionInfo);

                    if (climbingSlope) // Pour gérer les plafond dans les pentes
                    {
                        actualSpeedX = actualSpeedY / Mathf.Tan(climbingAngle * Mathf.Deg2Rad) * Mathf.Sign(actualSpeedX);
                    }

                    return;
                }
                originRaycast += new Vector2(Mathf.Abs(upperRight.x - upperLeft.x) / (numberRaycastVertical - 1), 0);
            }
        }
        // ======================================================================================================
        // Cas où on check si on prend une pente avec un nouvelle angle
        if (climbingSlope)
        {
            float directionX = Mathf.Sign(actualSpeedX);
            //rayLength = Mathf.Abs(velocity.x) + skinWidth;
            originRaycast = ((directionX == -1) ? bottomLeft : bottomRight) + Vector2.up * actualSpeedY;
            //RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastY, Mathf.Abs(actualSpeedX), layerMask);
            if (raycastY.collider != null)
            {
                float slopeAngle = Vector2.Angle(raycastY.normal, Vector2.up);
                if (slopeAngle != climbingAngle)
                {
                    actualSpeedX = (raycastY.distance - offsetRaycastX) * directionX;
                    climbingAngle = slopeAngle;
                }
            }
        }
        // ======================================================================================================
    }

    private void ClimbSlope(float angle)
    {
        float climbSpeedY = Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX);
        if (actualSpeedY <= climbSpeedY)
        {
            actualSpeedY = climbSpeedY;
            actualSpeedX = Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX) * Mathf.Sign(actualSpeedX);
            climbingAngle = angle;
            climbingSlope = true;
        }
    }

    void DescendSlope()
    {
        /*RaycastHit2D maxSlopHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(velocity.y) + skinWidth, collisionMask);
        RaycastHit2D maxSlopHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(velocity.y) + skinWidth, collisionMask);
        if (maxSlopHitLeft ^ maxSlopHitRight)
        {
            SlideDownMaxSlope(maxSlopHitLeft, ref velocity);
            SlideDownMaxSlope(maxSlopHitRight, ref velocity);
        }*/

        if (true) // !collisions.slidingDownMaxSlope
        {
            RaycastHit raycast;
            float directionX = Mathf.Sign(actualSpeedX);
            Vector2 rayOrigin = (directionX == -1) ? bottomRight : bottomLeft;
            //RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, layerMask);
            Physics.Raycast(rayOrigin, -Vector2.up, out raycast, Mathf.Infinity, layerMask);
            if (raycast.collider != null)
            {
                float slopeAngle = Vector2.Angle(raycast.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maxAngle)
                {
                    if (Mathf.Sign(raycast.normal.x) == directionX)
                    {
                        if (raycast.distance - offsetRaycastY <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX))
                        {
                            float moveDistance = Mathf.Abs(actualSpeedX);
                            float DescendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            actualSpeedX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(actualSpeedX);
                            actualSpeedY -= DescendVelocityY;

                            climbingAngle = slopeAngle;
                            descendingSlope = true;
                        }
                    }
                }
            }
        }
    }
}
