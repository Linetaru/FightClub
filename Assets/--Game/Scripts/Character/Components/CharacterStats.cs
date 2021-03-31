using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStats : MonoBehaviour
{
    public CharacterBase userBase;

    [Title("Data")]
    [SerializeField]
    [ReadOnly] // A mettre dans battle manager
    private GameData gameData;
    public GameData GameData
    {
        get { return gameData; }
        set { gameData = value; }
    }

    [SerializeField]
    [ReadOnly]
    public PackageCreator.Event.GameEventUICharacter gameEvent;

    [Title("Life")]
    [SerializeField]
    [ReadOnly]
    private float lifePercentage;
    public float LifePercentage
    {
        get { return lifePercentage; }
        set { lifePercentage = value;
            if (gameEvent != null)
                gameEvent.Raise(LifePercentage);
        }
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

    [Title("Movement Stats")]
    [SerializeField]
    private Stats speed;
    public Stats Speed
    {
        get { return speed; }
        set { speed = value; }
    }

//=======================================================================================

    public void InitStats()
    {
        userBase = this.transform.parent.transform.parent.GetComponent<CharacterBase>();
        if (GameData.VictoryCondition == VictoryCondition.Health)
            LifeStocks = GameData.NumberOfLifes;
        LifePercentage = 0;
        Death = false;
        Speed.InitStats(userBase.Movement.SpeedMax);
    }

    public void ChangeSpeed(float newValue)
    {
        Speed.IncrementBonusStat(newValue);
        userBase.Movement.SpeedMax = Speed.valueStat;
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
