using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Camera/CameraConfig", order = 1)]
public class CameraConfig : ScriptableObject
{
    [Title("Config Zoom Controller")]
    public float config_depthUpdateSpeed;
    public float config_angleUpdateSpeed;
    public float config_positionUpdateSpeed;

    public float config_depthMax;
    public float config_depthMin;

    public float config_angleMax;
    public float config_angleMin;


    [Title("Config Focus Level")]
    public float halfXBounds;
    public float halfYBounds;
    public float halfZBounds;
}