using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



public class CharacterStats : MonoBehaviour, IStats
{
    public CharacterBase userBase;

    [Title("Data")]
    /*[SerializeField]
    [ReadOnly] // A mettre dans battle manager
    private GameData gameData;
    public GameData GameData
    {
        get { return gameData; }
        set { gameData = value; }
    }*/

    /*[SerializeField]
    [ReadOnly]
    private CharacterData characterData;
    public CharacterData CharacterData
    {
        get { return characterData; }
    }*/


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

    /*[Title("Kill")]
    [SerializeField]
    [ReadOnly]
    private int killNumber;
    public int KillNumber
    {
        get { return killNumber; }
        set { killNumber = value; }
    }*/


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

    [SerializeField]
    private Stats weight;
    public Stats Weight
    {
        get { return weight; }
        set { weight = value; }
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
    private Stats aerialSpeed;
    public Stats AerialSpeed
    {
        get { return aerialSpeed; }
        set { aerialSpeed = value; }
    }

    [SerializeField]
    private Stats jump;
    public Stats Jump
    {
        get { return jump; }
        set { jump = value; }
    }

    [SerializeField]
    private Stats knockbackPerDistance;
    public Stats KnockbackPerDistance
    {
        get { return knockbackPerDistance; }
        set { knockbackPerDistance = value; }
    }

    //=======================================================================================
    bool firstTime = false;
    public void InitStats()
    {
        //characterData = data;
        userBase = this.transform.parent.transform.parent.GetComponent<CharacterBase>();
        LifePercentage = 0;
        Death = false;

        if (firstTime == false) // Quand on reset la map, si on reset les stats une deuxieme fois ça peut créer des embrouilles
        {
            AttackMultiplier.InitStats(1);
            DefenseMultiplier.InitStats(1);

            Speed.InitStats(userBase.Movement.SpeedMax);
            AerialSpeed.InitStats(userBase.Movement.MaxAerialSpeed);
            Jump.InitStats(userBase.Movement.JumpNumber);

            Weight.InitStats(userBase.Knockback.Weight);
            KnockbackPerDistance.InitStats(userBase.Knockback.TimeKnockbackPerDistance);
            firstTime = true;
        }

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


    public Stats GetStat(MainStat mainStat)
    {
        switch (mainStat)
        {
            case MainStat.AttackMultiplier:
                return AttackMultiplier;
            case MainStat.DefenseMultiplier:
                return DefenseMultiplier;
            case MainStat.Speed:
                return Speed;
            case MainStat.AerialSpeed:
                return AerialSpeed;
            case MainStat.NbJump:
                return Jump;
            case MainStat.Weight:
                return Weight;
            case MainStat.KnockbackPerDistance:
                return KnockbackPerDistance;
        }
        return null;
    }
    public void ApplyStatModifs(MainStat mainStat)
    {
        switch(mainStat)
        {
            case MainStat.Speed:
                userBase.Movement.SpeedMax = Speed.Value;
                break;
            case MainStat.AerialSpeed:
                //userBase.Movement.MaxAerialSpeed = AerialSpeed.Value;
                break;
            case MainStat.NbJump:
                userBase.Movement.JumpNumber = (int)Jump.Value;
                userBase.Movement.CurrentNumberOfJump = 0;
                break;
            case MainStat.Weight:
                userBase.Knockback.Weight = Weight.Value;
                break;
            case MainStat.KnockbackPerDistance:
                userBase.Knockback.TimeKnockbackPerDistance = KnockbackPerDistance.Value;
                break;
        }
    }

}
