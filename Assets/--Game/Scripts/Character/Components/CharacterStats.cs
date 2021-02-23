using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStats : MonoBehaviour
{
    [Title("Data")]
    [SerializeField]
    private GameData gameData;
    public GameData GameData
    {
        get { return gameData; }
        set { gameData = value; }
    }

    [Title("Life")]
    [SerializeField]
    [ReadOnly]
    private float lifePercentage;
    public float LifePercentage
    {
        get { return lifePercentage; }
        set { lifePercentage = value; }
    }

    [SerializeField]
    [ReadOnly]
    private int lifeStocks;
    public int LifeStocks
    {
        get { return lifeStocks; }
        set { lifeStocks = value; }
    }


    [Title("Death")]
    [SerializeField]
    [ReadOnly]
    private bool death;
    public bool Death
    {
        get { return death; }
        set { death = value; }
    }

    [Title("Kill")]
    [SerializeField]
    [ReadOnly]
    private int killNumber;
    public int KillNumber
    {
        get { return killNumber; }
        set { killNumber = value; }
    }


    public void InitStats()
    {
        if (GameData.VictoryCondition == VictoryCondition.Health)
            LifeStocks = GameData.NumberOfLifes;
        LifePercentage = 0;
        Death = false;
    }

    public void TakeDamage(float damage)
    {
        if(LifePercentage + damage <= 999)
            LifePercentage += damage;
        else
        {
            LifePercentage = 999;
        }
    }

    public void RespawnStats()
    {
        LifePercentage = 0;
    }
}
