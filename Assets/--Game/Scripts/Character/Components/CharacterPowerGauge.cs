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
            if (gameEvent != null)
                gameEvent.Raise(currentPower);
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
    public const int maxPower = 100;

    [Title("Segment Parameter")]
    private bool canGainPoint = false;//true;
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

        if (CurrentPower < 25)
        {
            if (CurrentPower + i_value >= 25)
            {
                CurrentPower = 25;
                canGainPoint = false;
            }
            else
                CurrentPower += i_value;
        }
        else if(CurrentPower >= 25 && CurrentPower < 50)
        {
            if (CurrentPower + i_value >= 50)
            {
                CurrentPower = 50;
                canGainPoint = false;
            }
            else
                CurrentPower += i_value;
        }
        else if (CurrentPower >= 50 && CurrentPower < 75)
        {
            if (CurrentPower + i_value >= 75)
            {
                CurrentPower = 75;
                canGainPoint = false;
            }
            else
                CurrentPower += i_value;
        }
        else if(CurrentPower >= 75 && CurrentPower <= 100)
        {
            if (CurrentPower + i_value >= 100)
            {
                CurrentPower = maxPower;
                canGainPoint = false;
            }
            else
                CurrentPower += i_value;
        }
    }

    public void ForceAddPower(int i_value)
    {
        currentPower += i_value;
        currentPower = Mathf.Clamp(currentPower, 0, 100);

        if (gameEvent != null)
            gameEvent.Raise(currentPower);
    }

    public void AddBoost(int i_value)
    {
        if (CurrentPower + i_value <= maxPower)
            CurrentPower += i_value;
        else
            CurrentPower = maxPower;
    }

    public void ConsumePowerSegment(Input_Info input_Info, CharacterBase user)
    {
        /*if (input_Info.CheckAction(0, InputConst.LeftShoulder))
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

        if(input_Info.CheckAction(0, InputConst.RightShoulder))
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
        }*/

        UpdateTimer(user);
    }
}