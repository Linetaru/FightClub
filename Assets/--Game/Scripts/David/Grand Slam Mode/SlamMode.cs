using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SlamMode : MonoBehaviour
{
    public GameModeStateEnum gameMode;

    public bool hasScoreGoal = false;

    [HideIf("hasScoreGoal")]
    public int nbLife = 1;

    [ShowIf("hasScoreGoal")]
    public int scoreGoal = 5;



    [Scene]
    public List<string> scenes = new List<string>();

    public int[] scoreArr = new int[4];


}
