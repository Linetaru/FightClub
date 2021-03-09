using System.Collections;
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
    [Expandable]
    public CameraFocusLevel focusLevel;

    public List<GameObject> playersTarget;

    public float depthUpdateSpeed = 5f;
    public float angleUpdateSpeed = 7f;
    public float positionUpdateSpeed = 5f;

    public float depthMax = -10f;
    public float depthMin = -22f;

    public float angleMax = 11f;
    public float angleMin = 3f;

    private float cameraEulerX;
    private Vector3 CameraPosition;

    private bool canFocus = true;

    private void Start()
    {
        playersTarget.Add(focusLevel.gameObject);
    }

    private void LateUpdate()
    {
        if (canFocus)
        {
            CalculateCameraLocation();
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        Vector3 pos = gameObject.transform.position;
        if(pos != CameraPosition)
        {
            Vector3 targetPos = Vector3.zero;
            targetPos.x = Mathf.MoveTowards(pos.x, CameraPosition.x, positionUpdateSpeed * Time.deltaTime);
            targetPos.y = Mathf.MoveTowards(pos.y, CameraPosition.y, positionUpdateSpeed * Time.deltaTime);
            targetPos.z = Mathf.MoveTowards(pos.z, CameraPosition.z, depthUpdateSpeed * Time.deltaTime);
            gameObject.transform.position = targetPos;
        }

        Vector3 localEulerAngle = gameObject.transform.localEulerAngles;
        if(localEulerAngle.x != cameraEulerX)
        {
            Vector3 targetEulerAngle = new Vector3(cameraEulerX, localEulerAngle.y, localEulerAngle.z);
            gameObject.transform.localEulerAngles = Vector3.MoveTowards(localEulerAngle, targetEulerAngle, angleUpdateSpeed * Time.deltaTime);
        }
    }

    private void CalculateCameraLocation()
    {
        Vector3 averageCenter = Vector3.zero;
        Vector3 totalPosition = Vector3.zero;
        Bounds playerBounds = new Bounds();

        for(int i = 0; i < playersTarget.Count; i++)
        {
            Vector3 playerPos = playersTarget[i].transform.position;

            if(!focusLevel.focusBounds.Contains(playerPos))
            {
                float playerX = Mathf.Clamp(playerPos.x, focusLevel.focusBounds.min.x, focusLevel.focusBounds.max.x);
                float playerY = Mathf.Clamp(playerPos.y, focusLevel.focusBounds.min.y, focusLevel.focusBounds.max.y);
                float playerZ = Mathf.Clamp(playerPos.z, focusLevel.focusBounds.min.z, focusLevel.focusBounds.max.z);
                playerPos = new Vector3(playerX, playerY, playerZ);
            }

            totalPosition += playerPos;
            playerBounds.Encapsulate(playerPos);
        }

        averageCenter = totalPosition / playersTarget.Count;

        float extents = playerBounds.extents.x + playerBounds.extents.y;
        float lerpPercent = Mathf.InverseLerp(0, (focusLevel.halfXBounds + focusLevel.halfYBounds) / 2, extents);

        float depth = Mathf.Lerp(depthMax, depthMin, lerpPercent);
        float angle = Mathf.Lerp(angleMax, angleMin, lerpPercent);

        cameraEulerX = angle;
        CameraPosition = new Vector3(averageCenter.x, averageCenter.y, depth);
    }

    public void ChangeFocusState()
    {
        canFocus = !canFocus;
    }

    public void ChangeValueFocus(CameraConfig cameraConfig)
    {
        depthUpdateSpeed = cameraConfig.config_depthUpdateSpeed;
        angleUpdateSpeed = cameraConfig.config_angleUpdateSpeed;
        positionUpdateSpeed = cameraConfig.config_positionUpdateSpeed;

        depthMax = cameraConfig.config_depthMax;
        depthMin = cameraConfig.config_depthMin;

        angleMax = cameraConfig.config_angleMax;
        angleMin = cameraConfig.config_angleMin;

        focusLevel.halfXBounds = cameraConfig.halfXBounds;
        focusLevel.halfYBounds = cameraConfig.halfYBounds;
        focusLevel.halfZBounds = cameraConfig.halfZBounds;
    }

    #if UNITY_EDITOR
        [Title("Editor Save Config")]
        public CameraConfig camConfig;

        [Button]
        public void CopyConfigOnScriptable()
        {
            if (camConfig != null)
            {
                camConfig.config_depthUpdateSpeed = depthUpdateSpeed;
                camConfig.config_angleUpdateSpeed = angleUpdateSpeed;
                camConfig.config_positionUpdateSpeed = positionUpdateSpeed;

                camConfig.config_depthMax = depthMax;
                camConfig.config_depthMin = depthMin;

                camConfig.config_angleMax = angleMax;
                camConfig.config_angleMin = angleMin;

                camConfig.halfXBounds = focusLevel.halfXBounds;
                camConfig.halfYBounds = focusLevel.halfYBounds;
                camConfig.halfZBounds = focusLevel.halfZBounds;
            }
        }
    #endif

#endregion
}