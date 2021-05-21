using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class TargetsCamera
{
    public Transform transform;
    public int priority;

    public TargetsCamera(Transform t, int p)
    {
        transform = t;
        priority = p;
    }
}


public class CameraZoomController : MonoBehaviour
{

    #region Brackey_Camera_Controller
    //public List<Transform> targets;

    //public GameObject focusLevel;

    ////public BlastZoneManager blastZoneManager;

    //[Title("Movement Parameter")]
    //public Vector3 offset;

    //public float smoothTime = 0.5f;

    //[Title("Zoom Parameter")]
    //public float minZoom = 40f;
    //public float maxZoom = 10f;
    //public float zoomLimiter = 50f;

    //public float fovForStaticScrolling = 60f;

    //[Title("Blast Zone Parameter")]
    //public float max_X_DistanceWithFocusLevel = 20f;
    //public float max_Y_DistanceWithFocusLevel = 15f;

    //[Title("Moving Cam Blast Parameter")]
    //[ReadOnly] public float max_X_MoveCam = 10;
    //[ReadOnly] public float offsetModifier = 7;

    //private Vector3 velocity;
    //private float velocityRef;
    //private Camera cam;
    //private bool canFocus = true;

    //Start Function that take component cam and add Focus transform on list of targets
    //private void Start()
    //{
    //    if (cam == null)
    //        cam = GetComponent<Camera>();
    //    //targets.Add(focusLevel.transform);
    //}

    //private void LateUpdate()
    //{
    //    //Return if no target is fund to not get errors
    //    if (targets.Count == 0)
    //        return;

    //    //If camera don't scroll in the level so camera can focus on player and execute function.
    //    if (canFocus)
    //    {
    //        //Launch Movement Camera Function
    //        MoveCamera();
    //        //Launch Zoom Camera Function
    //        //ZoomCamera();
    //    }
    //    else
    //    {
    //        //UnZoomCamera();
    //        GetNewBoundsEncapsulate();
    //    }
    //}

    //Zoom camera by field of view between distance from all targets
    //void ZoomCamera()
    //{
    //    //Get float for fov by distance from all targets on min and max zoom
    //    float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);

    //    //change fov of our cam with smooth lerp from last fov to new fov
    //    //cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, newZoom, ref velocityRef, smoothTime);
    //    Vector3 aled = this.transform.position;
    //    aled.z  = Mathf.SmoothDamp(this.transform.position.z, newZoom, ref velocityRef, smoothTime);
    //    this.transform.position = aled;
    //}

    //UnZoom camera by field of view for scrolling moment
    //void UnZoomCamera()
    //{
    //    //change fov of our cam with smooth lerp from last fov to new fov
    //    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fovForStaticScrolling, Time.deltaTime);
    //}

    //Get bounds of all targets and return width
    //float GetGreatestDistance()
    //{
    //    Bounds b = GetNewBoundsEncapsulate();
    //    return Mathf.Max(b.size.x, b.size.y);
    //}

    //Move camera position smoothly by calculate position of all targets
    //void MoveCamera()
    //{
    //    //Calculate centerpoint between all targets to have a center for camera
    //    Vector3 centerPoint = GetCenterPoint();

    //    float SizeZoomPasLeLogicielZoom = /*GetNewBoundsEncapsulate().size.magnitude * */5;

    //    Bounds bluePlane = BoundsCameraView(SizeZoomPasLeLogicielZoom, centerPoint);

    //    Bounds d = focusLevel.GetComponent<BoxCollider>().bounds;

    //    float x = 0;
    //    if (bluePlane.min.x < d.min.x)
    //        x = d.min.x - bluePlane.min.x;
    //    else if (bluePlane.max.x > d.max.x)
    //        x = d.max.x - bluePlane.max.x;
    //    else
    //        Debug.Log("On fait rien en X c'ptain");

    //    float y = 0;
    //    if (bluePlane.min.y < d.min.y)
    //        y = d.min.y - bluePlane.min.y;
    //    else if (bluePlane.max.y > d.max.y)
    //        y = d.max.y - bluePlane.max.y;
    //    else
    //        Debug.Log("On fait rien en Y c'ptain");

    //    //Calculate new Position for the camera by calculating centerpoint with an offset
    //    Vector3 newPos = centerPoint + offset;
    //    newPos.x += x;
    //    newPos.y += y;
    //    newPos.z -= SizeZoomPasLeLogicielZoom;

    //    //Change transform position smoothly without jitter from new Pos vector we got.
    //    transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    //}

    //Get new bounds of the first target and encapsule all targets in the bounds
    //Bounds GetNewBoundsEncapsulate()
    //{
    //    //Get position of first target or if target null get position of focus level
    //    Vector3 pos;
    //    if (targets.Count > 0)
    //        pos = targets[0].position;
    //    else
    //        pos = focusLevel.transform.position;

    //    //Get new bounds of the first target or FocusLevel
    //    Bounds bounds = new Bounds(pos, Vector3.zero);

    //    //Get Center of bounds from a target in the list if he isn't out of range 
    //    switch(targets.Count)
    //    {
    //        default:
    //            break;

    //        case 1:
    //            if (((focusLevel.transform.position.x - targets[0].position.x > max_X_DistanceWithFocusLevel) || (focusLevel.transform.position.x - targets[0].position.x < -max_X_DistanceWithFocusLevel / 2)) || ((focusLevel.transform.position.y - targets[0].position.y > max_Y_DistanceWithFocusLevel) || (focusLevel.transform.position.y - targets[0].position.y < -max_Y_DistanceWithFocusLevel / 2)))
    //                bounds.center = focusLevel.transform.position;
    //            break;
    //        case 2:
    //            bounds = Checking(bounds, 0, 1);
    //            break;
    //        case 3:
    //            bounds = Checking(bounds, 0, 1);
    //            bounds = Checking(bounds, 1, 2);
    //            break;
    //        case 4:
    //            bounds = Checking(bounds, 0, 1);
    //            bounds = Checking(bounds, 1, 2);
    //            bounds = Checking(bounds, 2, 3);
    //            break;
    //    }

    //    //Encapsule all targets in the bounds
    //    for (int i = 0; i < targets.Count; i++)
    //    {
    //        if (((focusLevel.transform.position.x - targets[i].position.x <= max_X_DistanceWithFocusLevel) && (focusLevel.transform.position.x - targets[i].position.x >= -max_X_DistanceWithFocusLevel)) && ((focusLevel.transform.position.y - targets[i].position.y <= max_Y_DistanceWithFocusLevel) && (focusLevel.transform.position.y - targets[i].position.y >= -max_Y_DistanceWithFocusLevel)))
    //            bounds.Encapsulate(targets[i].position);
    //        else if (targets[i] != null)
    //            BlastZoneManager.Instance.OutOfCamera(targets[i].gameObject);
    //    }

    //    //Return bounds with all targets encapsulte in
    //    return bounds;
    //}

    //private Bounds Checking(Bounds bounds, int id, int nextPlayerId)
    //{
    //    if (((focusLevel.transform.position.x - targets[id].position.x > max_X_DistanceWithFocusLevel) || (focusLevel.transform.position.x - targets[id].position.x < -max_X_DistanceWithFocusLevel / 2)) || ((focusLevel.transform.position.y - targets[id].position.y > max_Y_DistanceWithFocusLevel) || (focusLevel.transform.position.y - targets[id].position.y < -max_Y_DistanceWithFocusLevel / 2)))
    //        bounds.center = targets[nextPlayerId].position;

    //    return bounds;
    //}

    #endregion

    #region Camera_Frustrum_Clamped

    public List<TargetsCamera> targets = new List<TargetsCamera>();


    [Title("Object Reference")]
    public BoxCollider focusLevel;
    //public BoxCollider blastZone;


    [Title("Camera Offset")]
    public Vector3 offset = Vector3.zero;
    public Vector3 offsetRot = Vector3.zero;

    [Title("Zoom Smooth Time")]
    [LabelText("zoomSmoothTime")]
    public float smoothTime = 0.5f;
    public float dezoomSmoothTime = 0.2f;
    public float rotationSmoothTime = 0.05f;

    [Title("Zoom Parameter")]
    public float ZoomInLimiter = 5f;
    public Vector2 minZoomDistance = new Vector2(3,2);


    //[Title("Focus Collider Parameter")]
    //[SerializeField]
    //[InfoBox("Change Directly Focus Collider Center", InfoMessageType.Info)]
    //[OnValueChanged("FocusLevelBoxCenter")]
    //[PropertyOrder(1)]
    //Vector3 focusLevelBoxCenter;

    //public void FocusLevelBoxCenter()
    //{
    //    if (focusLevel != null)
    //        focusLevel.center = focusLevelBoxCenter;
    //}

    //[SerializeField]
    //[InfoBox("Change Directly Focus Collider Size", InfoMessageType.Info)]
    //[OnValueChanged("FocusLevelBoxSize")]
    //[PropertyOrder(1)]
    //Vector3 focusLevelBoxSize;

    //public void FocusLevelBoxSize()
    //{
    //    if (focusLevel != null)
    //        focusLevel.size = focusLevelBoxSize;
    //}

    //[GUIColor(0, 1, 0)]
    //[PropertyOrder(1)]
    //[Button]
    //public void GetFocusBoxProperties()
    //{
    //    if (focusLevel != null)
    //    {
    //        focusLevelBoxSize = focusLevel.size;
    //        focusLevelBoxCenter = focusLevel.center;
    //    }
    //}



    //[Title("BlastZone Collider Parameter")]
    //[SerializeField]
    //[InfoBox("Change Directly Blastzone Collider Center", InfoMessageType.Info)]
    //[OnValueChanged("BlastZoneBoxCenter")]
    //[PropertyOrder(2)]
    //Vector3 blastZoneBoxCenter;

    //public void BlastZoneBoxCenter()
    //{
    //    if (blastZone != null)
    //        blastZone.center = blastZoneBoxCenter;
    //}

    //[SerializeField]
    //[InfoBox("Change Directly Blastzone Collider Size", InfoMessageType.Info)]
    //[OnValueChanged("BlastZoneBoxSize")]
    //[PropertyOrder(2)]
    //Vector3 blastZoneBoxSize;

    //public void BlastZoneBoxSize()
    //{
    //    if (blastZone != null)
    //        blastZone.size = blastZoneBoxSize;
    //}

    //[GUIColor(1, 0, 0.9f, 1f)]
    //[PropertyOrder(2)]
    //[Button]
    //public void GetBlastZoneBoxProperties()
    //{
    //    if (blastZone != null)
    //    {
    //        blastZoneBoxSize = blastZone.size;
    //        blastZoneBoxCenter = blastZone.center;
    //    }
    //}




    [PropertyOrder(3)]
    [Title("Debug Area Drawing Box in Scene Viewport")]
    [DetailedInfoBox(
        "Debug Box will Appear to see all Area...", "Debug Box will Appear to see all Area... \n" +
        "Red Area = Player Encapsulate Bounds. \n" +
        "Black Circle = Center between players \n" +
        "Blue Area = Camera Area \nG" +
        "Green Area = Level Limit \n" +
        "Magenta Area = BlastZone Limit", InfoMessageType.Info)]
    public bool DebugArea = false;




    private Vector3 velocity;
    private Vector3 newPos;
    private float sizeZoom;
    private float previousSizeZoom;
    private bool canFocus = true;


    private Bounds cameraView;
    public Bounds CameraView
    {
        get { return cameraView; }
    }


    private Camera cam;
    public Camera Camera
    {
        get { return cam; }
    }


    private void Start()
    {
        cam = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        MoveCamera();
        this.transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(offsetRot), rotationSmoothTime);
    }

    /*void ZoomCamera()
    {

        float maxZoom = focusLevel.bounds.size.magnitude * 0.5f;

        float ratioY = (targetsBounds.size.y - minZoomDistance.y) / (focusLevel.bounds.size.y - minZoomDistance.y);
        float ratioX = (targetsBounds.size.x - minZoomDistance.x) / (focusLevel.bounds.size.x - minZoomDistance.x);
        float bestRatio = Mathf.Clamp(Mathf.Max(ratioX, ratioY), 0, 1);

        sizeZoom = Mathf.Lerp(ZoomInLimiter, maxZoom, bestRatio);
       // sizeZoom = Mathf.Lerp(ZoomInLimiter, focusLevel.bounds.size.magnitude, GetGreatestDistance().magnitude / focusLevel.size.magnitude);
    }*/

    /*void UnZoomCamera()
    {
        sizeZoom = focusLevel.bounds.size.magnitude;

        Bounds bluePlane = BoundsCameraView(sizeZoom, focusLevel.transform.position);

        Bounds d = focusLevel.bounds;

        float x = 0;
        if (bluePlane.min.x < d.min.x)
            x = d.min.x - bluePlane.min.x;
        else if (bluePlane.max.x > d.max.x)
            x = d.max.x - bluePlane.max.x;

        float y = 0;
        if (bluePlane.min.y < d.min.y)
            y = d.min.y - bluePlane.min.y;
        else if (bluePlane.max.y > d.max.y)
            y = d.max.y - bluePlane.max.y;

        //Calculate new Position for the camera by calculating centerpoint with an offset
        newPos = focusLevel.transform.position + offset;
        newPos.x += x;
        newPos.y += y;
        newPos.z -= sizeZoom;

        //Change transform position smoothly without jitter from new Pos vector we got.
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }*/

    //Move camera position smoothly by calculate position of all targets
    void MoveCamera()
    {
        //Calculate centerpoint between all targets to have a center for camera
        Bounds targetsBounds = CalculateNewBoundsEncapsulate();
        Vector3 centerPoint = targetsBounds.center;


        // Zoom
        if (canFocus == true)
        {
            float maxZoom = focusLevel.bounds.size.magnitude * 0.5f;

            float ratioY = (targetsBounds.size.y - minZoomDistance.y) / (focusLevel.bounds.size.y - minZoomDistance.y);
            float ratioX = (targetsBounds.size.x - minZoomDistance.x) / (focusLevel.bounds.size.x - minZoomDistance.x);
            float bestRatio = Mathf.Clamp(Mathf.Max(ratioX, ratioY), 0, 1);

            sizeZoom = Mathf.Lerp(ZoomInLimiter, maxZoom, bestRatio);
        }
        else
        {
            sizeZoom = focusLevel.bounds.size.magnitude;
        }


        //Calculate camera view and resize zoom to fit the bound of camera view
        Bounds bluePlane = CalculateBoundsCameraView(sizeZoom, centerPoint);

        // Clamp the camera view
        Bounds d = focusLevel.bounds;
        float x = 0;
        if (bluePlane.min.x < d.min.x)
            x = d.min.x - bluePlane.min.x;
        else if (bluePlane.max.x > d.max.x)
            x = d.max.x - bluePlane.max.x;

        float y = 0;
        if (bluePlane.min.y < d.min.y)
            y = d.min.y - bluePlane.min.y;
        else if (bluePlane.max.y > d.max.y)
            y = d.max.y - bluePlane.max.y;

        //Calculate new Position for the camera by calculating centerpoint with an offset
        newPos = centerPoint + offset;
        newPos.x += x;
        newPos.y += y;
        newPos.z -= sizeZoom;

        cameraView = bluePlane;
        cameraView.center += new Vector3(x, y);

        // Différent smoothTime en fonction de si on zoom ou on dezoom
        float finalSmoothTime = 0f;
        if (previousSizeZoom < sizeZoom)
            finalSmoothTime = dezoomSmoothTime;
        else
            finalSmoothTime = smoothTime;
        previousSizeZoom = sizeZoom;

        //Change transform position smoothly without jitter from new Pos vector we got.
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, finalSmoothTime);
    }


    Bounds CalculateNewBoundsEncapsulate()
    {
        // int c'est plus léger
        List<int> finalTargets = new List<int>();

        // Check highest camera priority
        int bestPriority = -9999;
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i].priority > bestPriority)
            {
                bestPriority = targets[i].priority;
                finalTargets.Clear();
                finalTargets.Add(i);
            }
            else if (targets[i].priority == bestPriority)
            {
                finalTargets.Add(i);
            }
        }

        Vector3 pos = focusLevel.transform.position;
        if (bestPriority == 0 && finalTargets.Count == 0)
        {
            pos = focusLevel.transform.position;
        }
        else if (finalTargets.Count != 0)
        {
            pos = targets[finalTargets[0]].transform.position;
        }
        Bounds bounds = new Bounds(pos, Vector3.zero);

        //Encapsule all targets in the bounds
        for (int i = 0; i < finalTargets.Count; i++)
        {
            bounds.Encapsulate(targets[finalTargets[i]].transform.position);
        }

        if (bestPriority == 0 && finalTargets.Count == 1)
            bounds.Encapsulate(focusLevel.transform.position);

        return bounds;
    }

    private Bounds CalculateBoundsCameraView(float distance, Vector3 center)
    {
        float frustumHeight = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        if (frustumHeight >= focusLevel.size.y)
        {
            frustumHeight = focusLevel.size.y;
            sizeZoom = frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        float frustumWidth = frustumHeight * cam.aspect;

        return new Bounds(center, new Vector3(frustumWidth, frustumHeight));
    }

    public void ModifyTargetPriority(Transform t, int newPriority)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i].transform == t)
            {
                targets[i].priority = newPriority;
                return;
            }
        }
    }


#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (DebugArea)
        {
            if (cam == null)
                cam = GetComponent<Camera>();

            Bounds CameraWire = CalculateNewBoundsEncapsulate();
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(CameraWire.center, CameraWire.size);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(newPos, CalculateBoundsCameraView(sizeZoom, CameraWire.center).size);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(CameraWire.center, 0.3f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(focusLevel.transform.position.x - focusLevel.center.x, focusLevel.transform.position.y + focusLevel.center.y), focusLevel.size);
            Gizmos.color = Color.magenta;

        }
    }
#endif

    public void ChangeFocusState()
    {
        canFocus = !canFocus;
    }

    public void ChangeValueFocus(CameraConfig cameraConfig)
    {
        offset = cameraConfig.config_offset;
        smoothTime = cameraConfig.config_smoothTime;

        ZoomInLimiter = cameraConfig.config_ZoomInLimiter;

        /*focusLevelBoxCenter = cameraConfig.config_FocusBoxCenter;
        focusLevelBoxSize = cameraConfig.config_FocusBoxSize;

        blastZoneBoxCenter = cameraConfig.config_BlastZoneBoxCenter;
        blastZoneBoxSize = cameraConfig.config_BlastZoneBoxSize;

        FocusLevelBoxCenter();
        FocusLevelBoxSize();
        BlastZoneBoxCenter();
        BlastZoneBoxSize();*/
    }

#if UNITY_EDITOR
    //Add Parameter to change a scriptable object by saving config parameter
    [Title("Editor Save Config")]
    [Expandable]
    [PropertyOrder(3)]
    public CameraConfig camConfig;

    //Save config parameter on scriptable object camConfig if it's not null, Got a button on inspector to use this function
    [Button]
    [PropertyOrder(3)]
    public void CopyConfigOnScriptable()
    {
        if (camConfig != null)
        {
            camConfig.config_offset = offset;
            camConfig.config_smoothTime = smoothTime;

            camConfig.config_ZoomInLimiter = ZoomInLimiter;

            /*camConfig.config_FocusBoxCenter = focusLevelBoxCenter;
            camConfig.config_FocusBoxSize = focusLevelBoxSize;

            camConfig.config_BlastZoneBoxCenter = blastZoneBoxCenter;
            camConfig.config_BlastZoneBoxSize = blastZoneBoxSize;*/

            UnityEditor.EditorUtility.SetDirty(camConfig);
            camConfig = null;
        }
    }

    //Change parameter base on scriptable object camConfig config parameter
    [Button]
    [PropertyOrder(3)]
    public void AddParameterConfigOnThisScript()
    {
        if (camConfig != null)
        {
            offset = camConfig.config_offset;
            smoothTime = camConfig.config_smoothTime;

            ZoomInLimiter = camConfig.config_ZoomInLimiter;

            /*focusLevelBoxCenter = camConfig.config_FocusBoxCenter;
            focusLevelBoxSize = camConfig.config_FocusBoxSize;

            blastZoneBoxCenter = camConfig.config_BlastZoneBoxCenter;
            blastZoneBoxSize = camConfig.config_BlastZoneBoxSize;

            FocusLevelBoxCenter();
            FocusLevelBoxSize();
            /*BlastZoneBoxCenter();
            BlastZoneBoxSize();*/

            UnityEditor.EditorUtility.SetDirty(camConfig);
            camConfig = null;
        }
    }
#endif
    #endregion





















    private IEnumerator rotationImpulseCoroutine;
    public void CameraRotationImpulse(Vector2 impulse, float time)
    {
        if (rotationImpulseCoroutine != null)
            StopCoroutine(rotationImpulseCoroutine);
        rotationImpulseCoroutine = RotationImpulseCoroutine(impulse, time);
        StartCoroutine(rotationImpulseCoroutine);
    }

    private IEnumerator RotationImpulseCoroutine(Vector2 impulse, float time)
    {
        float t = 0f;
        Quaternion intialRot = this.transform.localRotation;
        Quaternion finalRot = Quaternion.Euler(impulse.x + offsetRot.x, impulse.y + offsetRot.y, 0);
        while (t < time)
        {
            t += Time.deltaTime;
            this.transform.localRotation = Quaternion.Lerp(intialRot, finalRot, t / time);
            yield return null;
        }
    }

















}