using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Stats
{
    [SerializeField]
    [OnValueChanged("CalculateFinalStat")]
    private float baseStat;
    public float BaseStat
    {
        get { return baseStat; }
    }

    [SerializeField]
    [OnValueChanged("CalculateFinalStat")]
    private float multiplierValue = 0.0f;
    public float MultiplierValue
    {
        get { return multiplierValue; }
    }

    [SerializeField]
    [OnValueChanged("CalculateFinalStat")]
    private float incrementValue = 0.0f;
    public float IncrementValue
    {
        get { return incrementValue; }
    }

    [ReadOnly][SerializeField]
    private float valueStat;
    public float Value
    {
        get { return valueStat; }
    }


    public Stats()
    {
        multiplierValue = 0.0f;
        incrementValue = 0.0f;

        valueStat = baseStat;
    }

    public void InitStats(float stat)
    {
        baseStat = stat;
        multiplierValue = 0.0f;
        incrementValue = 0.0f;

        CalculateFinalStat();
    }

    public void IncrementFlatBonusStat(float bonusValue)
    {
        incrementValue += bonusValue;
        CalculateFinalStat();
    }

    public void IncrementMultiplierBonusStat(float bonusValue)
    {
        multiplierValue += bonusValue;
        CalculateFinalStat();
    }


    private void CalculateFinalStat()
    {
        valueStat = (baseStat + incrementValue) + ((baseStat + incrementValue) * multiplierValue);
    }
}
