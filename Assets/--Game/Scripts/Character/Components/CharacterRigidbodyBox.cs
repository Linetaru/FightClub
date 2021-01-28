using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class CharacterRigidbodyBox : CharacterRigidbody
{
    public delegate void ActionCollision(Transform transform);
    public event ActionCollision OnWallCollision;
    public event ActionCollision OnGroundCollision;

    [Header("CharacterController")]
    [SerializeField]
    protected BoxCollider characterCollider;


    [Header("Collision")]
    [SerializeField]
    protected bool collision = true;

    [HorizontalGroup("RaycastOffset")]
    [SerializeField]
    protected float offsetRaycastX = 0.0001f;
    [HorizontalGroup("RaycastOffset")]
    [SerializeField]
    protected float offsetRaycastY = 0.0001f;

    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    protected int numberRaycastVertical = 2;
    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    protected int numberRaycastHorizontal = 2;

    [SerializeField] private LayerMask[] collisionMasks;


    [HorizontalGroup("RaycastNumber")]
    [SerializeField]
    protected float maxAngle = 45;

    /*protected float characterMotionSpeed = 1;
    public float CharacterMotionSpeed
    {
        get { return characterMotionSpeed; }
        set { characterMotionSpeed = value; }
    }*/


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

    protected float actualSpeedX = 0;
    protected float actualSpeedY = 0;


    int layerMask;
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

    public override void UpdateCollision(float speedX, float speedY)
    {
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

            UpdatePositionX();
            //transform.position = new Vector3(transform.position.x + (actualSpeedX * Time.deltaTime), transform.position.y, 0);
            //Physics.SyncTransforms();

            Vector2 offsetX = new Vector2(actualSpeedX, 0);
            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y) + offsetX;
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y) + offsetX;
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y) + offsetX;
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y) + offsetX;

            UpdatePositionYBox();
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
                        float distance2 = raycastX.point.x - originRaycast.x;
                        distance2 += offsetRaycastX;
                        ClimbSlope(slopeAngle);
                        actualSpeedX += distance2;
                    }
                    else
                    {
                        collisionInfo = raycastX.collider.transform;
                        float distance = raycastX.point.x - originRaycast.x;
                        distance += offsetRaycastX;
                        actualSpeedX = distance;
                        collisionWallInfo = collisionInfo;
                        OnWallCollision?.Invoke(collisionInfo);
                        return;
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
                Debug.DrawRay(originRaycast, new Vector2(actualSpeedX, 0), Color.yellow, 1f);
                if (raycastX.collider != null)
                {

                    float slopeAngle = Vector2.Angle(raycastX.normal, Vector2.up);
                    if (i == 0 && slopeAngle <= maxAngle)
                    {
                        float distance2 = raycastX.point.x - originRaycast.x;
                        distance2 -= offsetRaycastX;
                        ClimbSlope(slopeAngle);
                        actualSpeedX += distance2;
                    }
                    else
                    {
                        collisionInfo = raycastX.collider.transform;
                        float distance = raycastX.point.x - originRaycast.x;
                        distance -= offsetRaycastX;
                        actualSpeedX = distance;
                        collisionWallInfo = collisionInfo;
                        OnWallCollision?.Invoke(collisionInfo);
                        return;
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
        float minDistance = 999999;

        if (actualSpeedY < 0)
        {
            // ======================================================================================================
            originRaycast = bottomLeft;// - new Vector2(0, offsetRaycastY);
            for (int i = 0; i < numberRaycastVertical; i++)
            {
                //Physics.BoxCast(out raycastY)
                Physics.Raycast(originRaycast, new Vector2(0, actualSpeedY), out raycastY, Mathf.Abs(actualSpeedY), layerMask);
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.red, 1f);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y - originRaycast.y;
                    distance += offsetRaycastY;
                    if (Mathf.Abs(distance) < minDistance)
                    {
                        minDistance = Mathf.Abs(distance);
                        actualSpeedY = distance;
                        isGrounded = true;
                        //OnWallCollision?.Invoke(collisionInfo);
                    }
                    //return;
                }
                originRaycast += new Vector2(Mathf.Abs(bottomRight.x - bottomLeft.x) / (numberRaycastVertical - 1), 0);
            }
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
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.yellow, 1f);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y - originRaycast.y;
                    distance -= offsetRaycastY;
                    actualSpeedY = distance;
                    OnWallCollision?.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(Mathf.Abs(upperRight.x - upperLeft.x) / (numberRaycastVertical - 1), 0);
            }
            // ======================================================================================================
        }
    }

    //CapsuleCollider capsuleCollider;


    private void UpdatePositionYBox()
    {
        RaycastHit raycastY;
        Vector2 originRaycast;

        if (actualSpeedY < 0)
        {
            // ======================================================================================================
            originRaycast = bottomLeft + ((bottomRight - bottomLeft) * 0.5f);// - new Vector2(0, offsetRaycastY);
            //Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.red);
            Physics.BoxCast(originRaycast, new Vector3((bottomRight.x - bottomLeft.x) * 0.5f, offsetRaycastY, 0.5f), new Vector2(0, actualSpeedY).normalized, out raycastY, Quaternion.identity, Mathf.Abs(actualSpeedY), layerMask);
            //Debug.Log(actualSpeedY);
            if (raycastY.collider != null)
            {
                Debug.DrawRay(originRaycast, new Vector3(raycastY.point.x - originRaycast.x, raycastY.point.y - originRaycast.y, 0), Color.yellow);
                Debug.DrawRay(raycastY.point, raycastY.normal, Color.blue);
                Debug.DrawRay(raycastY.point + new Vector3(-0.1f, -0.1f, 0), new Vector3(0.2f, 0.2f, 0), Color.white);
                Debug.DrawRay(raycastY.point + new Vector3(-0.1f, 0.1f, 0), new Vector3(0.2f, -0.2f, 0), Color.white);

                collisionInfo = raycastY.collider.transform;
                float distance = raycastY.point.y - originRaycast.y;
                distance += (offsetRaycastY * 2);
                actualSpeedY = distance;
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.red);
                return;
            }
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
                Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.red, 1f);
                if (raycastY.collider != null)
                {
                    collisionInfo = raycastY.collider.transform;
                    float distance = raycastY.point.y - originRaycast.y;
                    distance -= offsetRaycastY;
                    actualSpeedY = distance;
                    OnWallCollision?.Invoke(collisionInfo);
                    return;
                }
                originRaycast += new Vector2(Mathf.Abs(upperRight.x - upperLeft.x) / (numberRaycastVertical - 1), 0);
            }
            // ======================================================================================================
        }
    }

    /*public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
    {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }
    public static void DrawBox(Box box, Color color)
    {
        Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
        Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
        Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
        Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

        Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
        Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
        Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
        Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

        Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
        Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
        Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
        Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
    }*/


    private void ClimbSlope(float angle)
    {
        //climbingSlopes = true;
        actualSpeedY = Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX) + (offsetRaycastY * 2);
        actualSpeedX = Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX) * Mathf.Sign(actualSpeedX);
        Debug.Log("Slope");
    }

    /*public void MoveX(float newSpeedX)
    {
        speedX = newSpeedX;
    }
    public void MoveY(float newSpeedY)
    {
        speedY = newSpeedY;
    }
    public void Move(float newSpeedX, float newSpeedY)
    {
        speedX = newSpeedX;
        speedY = newSpeedY;
    }*/



}
