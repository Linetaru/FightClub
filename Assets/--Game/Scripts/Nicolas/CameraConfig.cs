using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Camera/CameraConfig", order = 1)]
public class CameraConfig : ScriptableObject
{
    [Title("Config Zoom Controller")]
    [SerializeField]
    public Vector3 config_offset;

    [SerializeField]
    public float config_smoothTime = 0.5f;

    [SerializeField]
    public float config_minZoom = 40f;
    [SerializeField]
    public float config_maxZoom = 10f;
    [SerializeField]
    public float config_zoomLimiter = 50f;

    [SerializeField]
    public float config_fovForStaticScrolling = 60f;
}