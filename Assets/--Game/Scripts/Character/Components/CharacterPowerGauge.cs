using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterPowerGauge : MonoBehaviour
{
    [Title("Powers Parameter")]
    [SerializeField]
    [ReadOnly]
    private int currentPower = 0;
    public int CurrentPower
    {
        get { return currentPower; }
        set
        {
            currentPower = value;
        }
    }

    [SerializeField]
    private float speedPower = 100;
    public float SpeedPower
    {
        get { return speedPower; }
        set{ speedPower = value; }
    }

    public float speedTimeMax = 3;
    private float speedTime;
    private bool isOnSpeedBoost;

    [ReadOnly]
    public const int maxPower = 99;

    [Title("Segment Parameter")]
    private bool canGainPoint = true;
    public float timerSegmentPowerMax = 1.5f;
    private float timerSegmentPower;
    public bool canSignatureMoveUseOneSegmentPerUse = true;


    [Title("Power Given")]
    public int powerGivenOnAttack = 5;
    public int powerGivenToHitTarget = 7;
    public int powerGivenOnWallJump = 5;
    public int powerGivenOnWallRun = 5;
    public float timePowerGivenOnWallRunMax = 1.0f;
    private float timePowerGivenOnWallRun = 1.0f;
    [HideInInspector] public bool canGainPointByWallRun = false;
    public int powerGivenByBoost = 33;
    public int powerGivenByMegaBoost = 99;

    [Title("Event")]
    [SerializeField]
    [ReadOnly]
    public PackageCreator.Event.GameEventUICharacter gameEvent;

    public void Start()
    {
        CurrentPower = 0;
        if (gameEvent != null)
            gameEvent.Raise(currentPower);
    }

    public void UpdateTimer(CharacterBase user)
    {
        if (!canGainPoint)
        {
            if (timerSegmentPower < timerSegmentPowerMax)
            {
                timerSegmentPower += Time.deltaTime;
            }
            else if (timerSegmentPower >= timerSegmentPowerMax)
            {
                canGainPoint = !canGainPoint;
                timerSegmentPower = 0;
            }
        }

        if (canGainPointByWallRun && timePowerGivenOnWallRun >= timePowerGivenOnWallRunMax)
        {
            AddPower(powerGivenOnWallRun);
            timePowerGivenOnWallRun = 0;
        }
        else if (canGainPointByWallRun && timePowerGivenOnWallRun < timePowerGivenOnWallRunMax)
        {
            timePowerGivenOnWallRun += Time.deltaTime;
        }

        if(isOnSpeedBoost && speedTime < speedTimeMax)
        {
            speedTime += Time.deltaTime;
        }
        else if(isOnSpeedBoost)
        {
            speedTime = 0;
            isOnSpeedBoost = false;
            user.Stats.ChangeSpeed(-SpeedPower);
            Debug.Log("SpeedBoost Off");
        }
    }

    public void AddPower(int i_value)
    {
        if (!canGainPoint)
            return;

        if (currentPower < 33)
        {
            if (currentPower + i_value >= 33)
            {
                currentPower = 33;
                canGainPoint = false;
            }
            else
                currentPower += i_value;
        }
        else if(currentPower >= 33 && currentPower < 66)
        {
            if (currentPower + i_value >= 66)
            {
                currentPower = 66;
                canGainPoint = false;
            }
            else
                currentPower += i_value;
        }
        else if(currentPower <= 99 && currentPower >= 66)
        {
            if (currentPower + i_value >= 99)
            {
                currentPower = maxPower;
                canGainPoint = false;
            }
            else
                currentPower += i_value;
        }

        if (gameEvent != null)
            gameEvent.Raise(currentPower);
    }

    public void ConsumePowerSegment(Input_Info input_Info, CharacterBase user)
    {
        if (input_Info.CheckAction(0, InputConst.LeftTrigger))
        {
            input_Info.inputActions[0].timeValue = 0;
            if (currentPower >= 33)
            {
                currentPower -= 33;
                Debug.Log("SpeedBoost");
                user.Stats.ChangeSpeed(SpeedPower);
                isOnSpeedBoost = true;

                if (gameEvent != null)
                    gameEvent.Raise(currentPower);
            }
            else
            {
                Debug.Log("No Power to active SpeedBoost");
            }
        }

        if(input_Info.CheckAction(0, InputConst.RightTrigger))
        {
            input_Info.inputActions[0].timeValue = 0;
            if (canSignatureMoveUseOneSegmentPerUse)
            {
                if (currentPower >= 33)
                {
                    currentPower -= 33;
                    Debug.Log("SignatureMove");
                    if (gameEvent != null)
                        gameEvent.Raise(currentPower);
                }
                else
                {
                    Debug.Log("No Power to active SignatureMove");
                }
            }
            else
            {
                if (currentPower >= 99)
                {
                    currentPower -= 99;
                    Debug.Log("SignatureMove");
                    if (gameEvent != null)
                        gameEvent.Raise(currentPower);
                }
                else
                {
                    Debug.Log("No Power to active SignatureMove");
                }
            }
        }

        UpdateTimer(user);
    }
}