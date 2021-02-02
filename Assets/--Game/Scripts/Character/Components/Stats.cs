using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public float baseStat = 100.0f;
    public float multiplierValue = 1.0f;
    public float incrementValue = 0.0f;
    public float newStat;


    public Stats()
    {
        baseStat = 100.0f;
        multiplierValue = 1.0f;
        incrementValue = 0.0f;

        newStat = baseStat;
    }

    public void IncrementBonusStat(float bonusValue, bool isAddition = true)
    {
        if (isAddition)
        {
            newStat += bonusValue;
        }
        else
        {
            newStat += baseStat * (bonusValue/100);
        }
    }
    public void RemoveBonusStat(float bonusValue, bool isSubstraction = true)
    {
        if (isSubstraction)
        {
            newStat -= bonusValue;
        }
        else
        {
            newStat -= baseStat * (bonusValue / 100);
        }
    }
}
