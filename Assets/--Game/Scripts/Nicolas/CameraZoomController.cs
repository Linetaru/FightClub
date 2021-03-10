﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraZoomController : MonoBehaviour
{
    #region Old_Camera_Controller
    //public GameObject[] playerTarget;

    //public float yOffset = 2.0f;
    //public float minDistance = 7.5f;

    //private float xMin, xMax, yMin, yMax;

    //private void Start()
    //{
    //    GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
    //    playerTarget = new GameObject[allPlayers.Length];
    //    for(int i = allPlayers.Length; i > 0; i--)
    //    {
    //        playerTarget[i - 1] = allPlayers[i - 1];
    //    }
    //}

    //private void LateUpdate()
    //{
    //    if(playerTarget.Length == 0) { return;}

    //    xMin = xMax = playerTarget[0].transform.position.x;
    //    yMin = yMax = playerTarget[0].transform.position.y;

    //    for(int i = 1; i < playerTarget.Length; i++)
    //    {
    //        if (playerTarget[i].transform.position.x < xMin)
    //            xMin = playerTarget[i].transform.position.x;

    //        if (playerTarget[i].transform.position.x > xMax)
    //            xMax = playerTarget[i].transform.position.x;

    //        if (playerTarget[i].transform.position.x < yMin)
    //            yMin = playerTarget[i].transform.position.y;

    //        if (playerTarget[i].transform.position.x > yMax)
    //            yMax = playerTarget[i].transform.position.y;
    //    }

    //    float xMiddle = (xMin + xMax) / 2;
    //    float yMiddle = (yMin + yMax) / 2;
    //    float distance = xMax - xMin;

    //    if (distance < minDistance)
    //        distance = minDistance;

    //    transform.position = new Vector3(xMiddle, yMiddle, -distance);
    //}
    #endregion

    #region New_Camera_Controller
//    [Expandable]
//    public CameraFocusLevel focusLevel;

//    public List<GameObject> playersTarget;

//    public float depthUpdateSpeed = 5f;
//    public float angleUpdateSpeed = 7f;
//    public float positionUpdateSpeed = 5f;

//    public float depthMax = -10f;
//    public float depthMin = -22f;

//    public float angleMax = 11f;
//    public float angleMin = 3f;

//    private float cameraEulerX;
//    private Vector3 CameraPosition;

//    private bool canFocus = true;

//    private void Start()
//    {
//        playersTarget.Add(focusLevel.gameObject);
//    }

//    private void LateUpdate()
//    {
//        if (canFocus)
//        {
//            CalculateCameraLocation();
//            MoveCamera();
//        }
//    }

//    private void MoveCamera()
//    {
//        Vector3 pos = gameObject.transform.position;
//        if(pos != CameraPosition)
//        {
//            Vector3 targetPos = Vector3.zero;
//            targetPos.x = Mathf.MoveTowards(pos.x, CameraPosition.x, positionUpdateSpeed * Time.deltaTime);
//            targetPos.y = Mathf.MoveTowards(pos.y, CameraPosition.y, positionUpdateSpeed * Time.deltaTime);
//            targetPos.z = Mathf.MoveTowards(pos.z, CameraPosition.z, depthUpdateSpeed * Time.deltaTime);
//            gameObject.transform.position = targetPos;
//        }

//        Vector3 localEulerAngle = gameObject.transform.localEulerAngles;
//        if(localEulerAngle.x != cameraEulerX)
//        {
//            Vector3 targetEulerAngle = new Vector3(cameraEulerX, localEulerAngle.y, localEulerAngle.z);
//            gameObject.transform.localEulerAngles = Vector3.MoveTowards(localEulerAngle, targetEulerAngle, angleUpdateSpeed * Time.deltaTime);
//        }
//    }

//    private void CalculateCameraLocation()
//    {
//        Vector3 averageCenter = Vector3.zero;
//        Vector3 totalPosition = Vector3.zero;
//        Bounds playerBounds = new Bounds();

//        for(int i = 0; i < playersTarget.Count; i++)
//        {
//            Vector3 playerPos = playersTarget[i].transform.position;

//            if(!focusLevel.focusBounds.Contains(playerPos))
//            {
//                float playerX = Mathf.Clamp(playerPos.x, focusLevel.focusBounds.min.x, focusLevel.focusBounds.max.x);
//                float playerY = Mathf.Clamp(playerPos.y, focusLevel.focusBounds.min.y, focusLevel.focusBounds.max.y);
//                float playerZ = Mathf.Clamp(playerPos.z, focusLevel.focusBounds.min.z, focusLevel.focusBounds.max.z);
//                playerPos = new Vector3(playerX, playerY, playerZ);
//            }

//            totalPosition += playerPos;
//            playerBounds.Encapsulate(playerPos);
//        }

//        averageCenter = totalPosition / playersTarget.Count;

//        float extents = playerBounds.extents.x + playerBounds.extents.y;
//        float lerpPercent = Mathf.InverseLerp(0, (focusLevel.halfXBounds + focusLevel.halfYBounds) / 2, extents);

//        float depth = Mathf.Lerp(depthMax, depthMin, lerpPercent);
//        float angle = Mathf.Lerp(angleMax, angleMin, lerpPercent);

//        cameraEulerX = angle;
//        CameraPosition = new Vector3(averageCenter.x, averageCenter.y, depth);
//    }

//    public void ChangeFocusState()
//    {
//        canFocus = !canFocus;
//    }

//    public void ChangeValueFocus(CameraConfig cameraConfig)
//    {
//        depthUpdateSpeed = cameraConfig.config_depthUpdateSpeed;
//        angleUpdateSpeed = cameraConfig.config_angleUpdateSpeed;
//        positionUpdateSpeed = cameraConfig.config_positionUpdateSpeed;

//        depthMax = cameraConfig.config_depthMax;
//        depthMin = cameraConfig.config_depthMin;

//        angleMax = cameraConfig.config_angleMax;
//        angleMin = cameraConfig.config_angleMin;

//        focusLevel.halfXBounds = cameraConfig.halfXBounds;
//        focusLevel.halfYBounds = cameraConfig.halfYBounds;
//        focusLevel.halfZBounds = cameraConfig.halfZBounds;
//    }



    #endregion

    #region Brackey_Camera_Controller
    public List<Transform> targets;

    public GameObject focusLevel;

    [Title("Movement Parameter")]
    public Vector3 offset;

    public float smoothTime = 0.5f;

    [Title("Zoom Parameter")]
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    public float fovForStaticScrolling = 60f;

    private Vector3 velocity;
    private float velocityRef;
    private Camera cam;
    private bool canFocus = true;

    //Start Function that take component cam and add Focus transform on list of targets
    private void Start()
    {
        cam = GetComponent<Camera>();
        //targets.Add(focusLevel.transform);
    }

    private void LateUpdate()
    {
        //Return if no target is fund to not get errors
        if (targets.Count == 0)
            return;

        //If camera don't scroll in the level so camera can focus on player and execute function.
        if (canFocus)
        {
            //Launch Movement Camera Function
            MoveCamera();
            //Launch Zoom Camera Function
            ZoomCamera();
        }
        else
        {
            UnZoomCamera();
        }
    }

    //Zoom camera by field of view between distance from all targets
    void ZoomCamera()
    {
        //Get float for fov by distance from all targets on min and max zoom
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);

        //change fov of our cam with smooth lerp from last fov to new fov
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, newZoom, ref velocityRef, smoothTime);
    }

    //UnZoom camera by field of view for scrolling moment
    void UnZoomCamera()
    {
        //change fov of our cam with smooth lerp from last fov to new fov
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fovForStaticScrolling, Time.deltaTime);
    }

    //Get bounds of all targets and return width
    float GetGreatestDistance()
    {
        return Mathf.Max(GetNewBoundsEncapsulate().size.x, GetNewBoundsEncapsulate().size.y);
    }

    //Move camera position smoothly by calculate position of all targets
    void MoveCamera()
    {
        //Calculate centerpoint between all targets to have a center for camera
        Vector3 centerPoint = GetCenterPoint();

        //Calculate new Position for the camera by calculating centerpoint with an offset
        Vector3 newPos = centerPoint + offset;

        //Change transform position smoothly without jitter from new Pos vector we got.
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }

    //Get Vector 3 center point between all differents target
    Vector3 GetCenterPoint()
    {
        //if only one target return position of this target
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        //return bounds center of all targets encapsulate in the bounds.
        return GetNewBoundsEncapsulate().center;
    }

    //Get new bounds of the first target and encapsule all targets in the bounds
    Bounds GetNewBoundsEncapsulate()
    {
        //Get new bounds of the first target
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

        //Encapsule all targets in the bounds
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        //Return bounds with all targets encapsulte in
        return bounds;
    }

    public void ChangeFocusState()
    {
        canFocus = !canFocus;
    }

    public void ChangeValueFocus(CameraConfig cameraConfig)
    {
        offset = cameraConfig.config_offset;
        smoothTime = cameraConfig.config_smoothTime;

        minZoom = cameraConfig.config_minZoom;
        maxZoom = cameraConfig.config_maxZoom;
        zoomLimiter = cameraConfig.config_zoomLimiter;
        fovForStaticScrolling = cameraConfig.config_fovForStaticScrolling;
    }

#if UNITY_EDITOR
    //Add Parameter to change a scriptable object by saving config parameter
    [Title("Editor Save Config")]
    [Expandable]
    public CameraConfig camConfig;

    //Save config parameter on scriptable object camConfig if it's not null, Got a button on inspector to use this function
    [Button]
    public void CopyConfigOnScriptable()
    {
        if (camConfig != null)
        {
            camConfig.config_offset = offset;
            camConfig.config_smoothTime = smoothTime;

            camConfig.config_minZoom = minZoom;
            camConfig.config_maxZoom = maxZoom;
            camConfig.config_zoomLimiter = zoomLimiter;
            camConfig.config_fovForStaticScrolling = fovForStaticScrolling;

            UnityEditor.EditorUtility.SetDirty(camConfig);
            camConfig = null;
        }
    }
#endif

    #endregion
}