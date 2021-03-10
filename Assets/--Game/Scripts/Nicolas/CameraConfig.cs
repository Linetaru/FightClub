using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[Serializable]
[CreateAssetMenu(fileName = "CameraConfig", menuName = "Camera/CameraConfig", order = 1)]
public class CameraConfig : ScriptableObject
{
    [Title("Config Zoom Controller")]
    public Vector3 config_offset;

    public float config_smoothTime = 0.5f;

    public float config_minZoom = 40f;
    public float config_maxZoom = 10f;
    public float config_zoomLimiter = 50f;

    public float config_fovForStaticScrolling = 60f;
}