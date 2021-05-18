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


    [Title("Attack Stats")]
    [SerializeField]
    private Stats attackMultiplier;
    public Stats AttackMultiplier
    {
        get { return attackMultiplier; }
        set { attackMultiplier = value; }
    }

    [SerializeField]
    private Stats defenseMultiplier;
    public Stats DefenseMultiplier
    {
        get { return defenseMultiplier; }
        set { defenseMultiplier = value; }
    }


    [Title("Movement Stats")]
    [SerializeField]
    private Stats speed;
    public Stats Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    private Stats jump;
    public Stats Jump
    {
        get { return jump; }
        set { jump = value; }
    }

    //=======================================================================================

    public void InitStats()
    {
        userBase = this.transform.parent.transform.parent.GetComponent<CharacterBase>();
        if (GameData.VictoryCondition == VictoryCondition.Health)
            LifeStocks = GameData.NumberOfLifes;
        LifePercentage = 0;
        Death = false;

        AttackMultiplier.InitStats(1);
        DefenseMultiplier.InitStats(1);
        Speed.InitStats(userBase.Movement.SpeedMax);
        Jump.InitStats(userBase.Movement.JumpNumber);
    }

    public void ChangeSpeed(float newValue)
    {
        Speed.IncrementFlatBonusStat(newValue);
        userBase.Movement.SpeedMax = Speed.Value;
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
        LifePercentage = 0.0f;
        if (gameEvent != null)
            gameEvent.Raise(LifePercentage);
    }
}
