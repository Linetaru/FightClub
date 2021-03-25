using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public float baseStat;
    public float multiplierValue = 1.0f;
    public float incrementValue = 0.0f;
    [ReadOnly] public float valueStat;

    public Stats()
    {
        multiplierValue = 1.0f;
        incrementValue = 0.0f;

        valueStat = baseStat;
    }

    public void InitStats(float stat)
    {
        baseStat = stat;
        valueStat = baseStat;
    }

    public void IncrementBonusStat(float bonusValue, bool isAddition = true)
    {
        if (isAddition)
        {
            valueStat += bonusValue;
        }
        else
        {
            valueStat += baseStat * (bonusValue/100);
        }
    }
    public void RemoveBonusStat(float bonusValue, bool isSubstraction = true)
    {
        if (isSubstraction)
        {
            valueStat -= bonusValue;
        }
        else
        {
            valueStat -= baseStat * (bonusValue / 100);
        }
    }
}
