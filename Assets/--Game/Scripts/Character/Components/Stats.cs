using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float baseStat = 100.0f;
    float multiplierValue = 1.0f;
    float incrementValue = 0.0f;
    float newStat;

    public float UpdateStat(float baseStat, float collectibleIncrementValue = 0.0f, float collectibleMultiplierValue = 1.0f)
    {
        return (baseStat * (multiplierValue * collectibleIncrementValue)) + (incrementValue + collectibleIncrementValue);
    }
}
