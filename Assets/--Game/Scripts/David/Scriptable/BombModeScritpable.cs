using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "BombModeData", menuName = "Data/BombModeData")]
public class BombModeScritpable : ScriptableObject
{
    [Title("Timers")]
    [Range(1f, 120f)]
    public float normalModeTimer = 10f;
    [Range(1f, 120f)]
    public float fakeModeTimer = 10f;
    [Range(1f, 120f)]
    public float invisibleModeTimer = 10f;
    [Range(1f, 120f)]
    public float noCountModeTimer = 10f;
    [Range(1f, 120f)]
    public float resetModeTimer = 10f;

    [Title("Round Infos")]
    public float timeBetweenRounds;

    [Title("Player Scale Infos")]
    // A partir de quel pourcentage de bombTimer la scale sera à son max
    // Si coefScaleMax = 0.5 => Le joueur aura sa scale max à 50% de bombTimer
    [Range(0f, 1f)]
    [PropertyTooltip("Pourcentage de bombTimer auquel le player aura sa taille maximum (0.5 = 50%)")]
    public float coefScaleMax = 0.5f;

    [Range(1f, 5f)]
    public float scaleMaxMultiplier = 2f;


    [Title("Special Rounds")]
    public bool randomRounds;
    public int canRoundSpecialAfter = 3;

    [HideIf("randomRounds")]
    [Title("Special Mode List")]
    [DetailedInfoBox("Special Mode Codes (click to show)",
        "0 : Normal \n" +
        "1 : Fake Bomb \n" +
        "2 : Invisible Bomb \n" +
        "3 : No Countdown \n" +
        "4 : Bomb Reset \n", InfoMessageType.Info)]
    public List<int> specialRounds = new List<int>();

}