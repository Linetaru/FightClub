using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class CharacterRigidbodySlope : CharacterRigidbody
{
    [Title("CharacterController")]
    [SerializeField]
    private BoxCollider characterCollider;

    [SerializeField]
    [SuffixLabel(" en kilo")]
    private float weight = 62;

    [Title("Collision")]
    [SerializeField]
    private bool collision = true;

    [SerializeField] 
    private LayerMask layerMask;
    [SerializeField]
    private LayerMask aerialLayerMask;
    [SerializeField]
    private LayerMask groundLayerMask;

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

    private bool climbingSlope = false;


    Vector3 bottomLeft;
    Vector3 upperLeft;
    Vector3 bottomRight;
    Vector3 upperRight;

    Transform collisionInfo;

    [SerializeField]
    private CollisionRigidbody collisionWallInfo;
    public override CollisionRigidbody CollisionWallInfo
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


    private LayerMask currentLayerMask;
    private LayerMask currentAerialLayerMask;
    private LayerMask currentCheckGroundLayerMask;


    public override void SetNewLayerMask(LayerMask newLayerMask, bool groundLayerMask = false, bool aerialLayerMask = false)
    {
        if (aerialLayerMask == true)
            currentAerialLayerMask = newLayerMask;
        else if (groundLayerMask == true)
            currentCheckGroundLayerMask = newLayerMask;
        else
            currentLayerMask = newLayerMask;
    }

    // Repasse sur le layerMask par defaut
    public override void ResetLayerMask()
    {
        currentLayerMask = layerMask;
        currentAerialLayerMask = aerialLayerMask;
        currentCheckGroundLayerMask = groundLayerMask;
    }

    private void CalculateBounds(Vector3 offset)
    {
        bottomLeft = new Vector3(characterCollider.bounds.min.x, characterCollider.bounds.min.y, transform.position.z) + offset;
        upperLeft = new Vector3(characterCollider.bounds.min.x, characterCollider.bounds.max.y, transform.position.z) + offset;
        bottomRight = new Vector3(characterCollider.bounds.max.x, characterCollider.bounds.min.y, transform.position.z) + offset;
        upperRight = new Vector3(characterCollider.bounds.max.x, characterCollider.bounds.max.y, transform.position.z) + offset;
    }

    private void Start()
    {
        ResetLayerMask();
        collisionWallInfo = new CollisionRigidbody(numberRaycastHorizontal);
    }

    public override void CheckGround(float gravity)
    {
        gravity *= -Time.deltaTime;
        RaycastHit raycastY;
        Vector3 originRaycast = bottomLeft;
        Vector3 originOffset = (upperRight - upperLeft) / (numberRaycastVertical - 1);

        int layerMaskY = currentCheckGroundLayerMask;

        for (int i = 0; i < numberRaycastVertical; i++)
        {
            Physics.Raycast(originRaycast, new Vector2(0, gravity), out raycastY, Mathf.Abs(gravity), layerMaskY);
           // Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.red, 0.5f);
            if (raycastY.collider != null)
            {
                isGrounded = true;
                collisionGroundInfo = raycastY.collider.transform;
                return;
            }
            originRaycast += originOffset;
        }
        isGrounded = false;
        collisionGroundInfo = null;
    }

    public override bool CheckGroundNear(float gravity)
    {
        RaycastHit raycastY;
        Vector3 originRaycast = bottomLeft;
        Vector3 originOffset = (upperRight - upperLeft) / (numberRaycastVertical - 1);

        int layerMaskY = currentCheckGroundLayerMask;

        for (int i = 0; i < numberRaycastVertical; i++)
        {
            Physics.Raycast(originRaycast, new Vector2(0, gravity), out raycastY, Mathf.Abs(gravity), layerMaskY);
            if (raycastY.collider != null)
            {
                return true;
            }
            originRaycast += originOffset;
        }
        return false;
    }

    public override void UpdateCollision(float speedX, float speedY)
    {
        //isGrounded = false;
        collisionWallInfo.Collision = null;
        collisionGroundInfo = null;
        collisionRoofInfo = null;
        climbingSlope = false;

        actualSpeedX = speedX;
        actualSpeedY = speedY;
        actualSpeedX *= Time.deltaTime;
        actualSpeedY *= Time.deltaTime;

        if (actualSpeedY > 0.0001f) // On fait ce check avant le update X pour passer au travers des Character à la première frame d'un saut
        {
            isGrounded = false;
        }

        if (collision == true)
        {
            CalculateBounds(Vector2.zero);
            CheckPush(speedX, speedY);

            float slopeAngle = CheckSlope();
            if (slopeAngle <= maxSlopeAngle) // On climb --------------------------------------------------------------------
            {
                climbingSlope = true;
                float newSlopeAngle = slopeAngle;
                float speedYSaved = actualSpeedY;
                do // C'est pour géré des pentes successives qu'on fait une boucle
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
            else // On climb pas ou on touche un mur --------------------------------------------------------------------
            {
                UpdatePositionX();
                if (preventFall == true && isGrounded == true)
                    CheckEdge();

                Vector2 offsetX = new Vector2(actualSpeedX, 0);
                CalculateBounds(offsetX);
                UpdatePositionY();
            }
        }

        transform.position = new Vector3(transform.position.x + actualSpeedX, transform.position.y + actualSpeedY, transform.position.z);
        Physics.SyncTransforms();
    }



    private float CheckSlope()
    {
        RaycastHit raycastX;
        float directionX = Mathf.Sign(actualSpeedX);
        Vector3 originRaycast = (directionX == -1) ? bottomLeft : bottomRight;
        Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastX, Mathf.Abs(actualSpeedX) + offsetRaycastX, GetHorizontalLayerMask());
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
        Vector3 originRaycast = (directionX == -1) ? bottomLeft : bottomRight;
        Vector3 originOffset = (upperRight - bottomRight) / (numberRaycastHorizontal - 1);

        //!\
        float originSpeedX = actualSpeedX;
        //!\

        for (int i = 0; i < numberRaycastHorizontal; i++)
        {
            Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastX, Mathf.Abs(actualSpeedX) + offsetRaycastX, GetHorizontalLayerMask());
            Debug.DrawRay(originRaycast, new Vector2(actualSpeedX, 0) , Color.yellow, 0.5f);
            if (raycastX.collider != null)
            {
                float distance = raycastX.distance - offsetRaycastX;
                actualSpeedX = distance * directionX;
                if(!(climbingSlope == true && i == 0))
                    collisionWallInfo.Collision = raycastX.collider.transform;
            }
            //!\ Peut faire mal aux perfs
            collisionWallInfo.Contacts[i] = Physics.Raycast(originRaycast, new Vector2(originSpeedX, 0), Mathf.Abs(originSpeedX) + offsetRaycastX, GetHorizontalLayerMask());
            //!\
            originRaycast += originOffset;
        }
    }


    private void UpdatePositionY()
    {
        if (-0.0001f < actualSpeedY && actualSpeedY < 0.0001f)
            return;

        RaycastHit raycastY;
        float directionY = Mathf.Sign(actualSpeedY);
        Vector3 originRaycast = (directionY == -1) ? bottomLeft: upperLeft;
        Vector3 originOffset = (upperRight - upperLeft) / (numberRaycastVertical - 1);

        int layerMaskY = (directionY == -1) ? currentCheckGroundLayerMask : GetHorizontalLayerMask();

        for (int i = 0; i < numberRaycastVertical; i++)
        {
            Physics.Raycast(originRaycast, new Vector2(0, actualSpeedY), out raycastY, Mathf.Abs(actualSpeedY), layerMaskY);
            Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY), Color.red, 0.5f);
            if (raycastY.collider != null)
            {
                float distance = raycastY.distance - offsetRaycastY;
                actualSpeedY = distance * directionY;

                if(directionY == -1)
                {
                    isGrounded = true;
                    collisionGroundInfo = raycastY.collider.transform;
                }
                else
                {
                    collisionRoofInfo = raycastY.collider.transform;
                }
            }

            originRaycast += originOffset;
        }

        if (directionY == -1 && collisionGroundInfo == null)
            isGrounded = false;
        if (directionY == 1)
            isGrounded = false;
        if (climbingSlope == true)
            isGrounded = true;
    }




    // C'est plus clair mais redondant avec le check slope et donc moins performant
    private void CheckPush(float speedX, float speedY)
    {
        RaycastHit raycastX;
        float directionX = Mathf.Sign(actualSpeedX);
        Vector3 originRaycast = (directionX == -1) ? bottomLeft : bottomRight;
        Vector3 originOffset = (upperRight - bottomRight) / (numberRaycastHorizontal - 1);

        for (int i = 0; i < numberRaycastHorizontal; i++)
        {
            Physics.Raycast(originRaycast, new Vector2(actualSpeedX, 0), out raycastX, Mathf.Abs(actualSpeedX) + offsetRaycastX, GetHorizontalLayerMask());
            if (raycastX.collider != null)
            {
                CharacterRigidbody rigidbody = raycastX.collider.GetComponent<CharacterRigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.Push(speedX, speedY);
                    return;
                }
            }
            originRaycast += originOffset;
        }
    }

    public override void Push(float speedX, float speedY)
    {
        UpdateCollision(speedX * (30 / weight), speedY);
    }


    private LayerMask GetHorizontalLayerMask()
    {
        if (isGrounded == false) return currentAerialLayerMask;
         return currentLayerMask;
    }

    // Quand on monte une pente on check une collision vers le haut, ce qui peut etre interprété comme un saut, du coup on fais un check supplémentaire en bas pour bien dire qu'on est au sol
    /*private void CheckClimbGround()
    {
        RaycastHit raycastY;
        float directionY = -1;
        Vector3 originRaycast = bottomLeft;
        Vector3 originOffset = (upperRight - upperLeft) / (numberRaycastVertical - 1);

        int layerMaskY = currentGroundLayerMask;

        for (int i = 0; i < numberRaycastVertical; i++)
        {
            Physics.Raycast(originRaycast, new Vector2(0, -offsetRaycastY), out raycastY, Mathf.Abs(offsetRaycastY*2), layerMaskY);
            Debug.DrawRay(originRaycast, new Vector2(0, offsetRaycastY*2), Color.red, 0.5f);
            if (raycastY.collider != null)
            {
                float distance = raycastY.distance - offsetRaycastY;
                actualSpeedY = distance * directionY;
                isGrounded = true;
                collisionGroundInfo = raycastY.collider.transform;
            }
            originRaycast += originOffset;
        }
    }*/

    [SerializeField]
    float edgeDetection = 0.2f;


    private void CheckEdge()
    {
        RaycastHit raycastY;
        float directionY = -edgeDetection;
        Vector3 originRaycast = (Mathf.Sign(actualSpeedX) == -1) ? bottomLeft : bottomRight;
        originRaycast += new Vector3(actualSpeedX, 0, 0);

        Physics.Raycast(originRaycast, new Vector2(0, -edgeDetection), out raycastY, edgeDetection, currentCheckGroundLayerMask);
        if (raycastY.collider == null) // Il y du vide danger
        {
            originRaycast += new Vector3(0, -edgeDetection);

            Physics.Raycast(originRaycast, new Vector2(-actualSpeedX, 0), out raycastY, Mathf.Abs(actualSpeedX), currentLayerMask);
            if (raycastY.collider != null) // On a touché un mur, on sait la distance
            {
                float distance = raycastY.distance;
                actualSpeedX -= (distance * Mathf.Sign(actualSpeedX));
            }
            else // On sait pas donc c'est chaud, on bouge pas pour le moment mais wait & see
            {
                actualSpeedX = 0;
            }
        }
    }

}
