using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamMode : MonoBehaviour
{
    public GameModeStateEnum gameMode;

    [Scene]
    public List<string> scenes = new List<string>();

    public int nbLife = 1;

    public int[] scoreArr = new int[4];


}
