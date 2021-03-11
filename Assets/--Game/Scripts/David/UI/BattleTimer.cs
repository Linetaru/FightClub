using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleTimer : MonoBehaviour
{
    [SerializeField]
    private float timerInSeconds = 90; // Récupérer depuis le game data à terme

    [SerializeField]
    private Text timerText;

    private float countdownTimer;
    private int oldTimer;

    // Used only for the 00:00 format
    private float minutes;
    private float seconds;

    private bool timesUp;



    void Start()
    {
        countdownTimer = timerInSeconds;
    }


    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        if(!timesUp)
        {
            if (countdownTimer > 0)
            {
                oldTimer = Mathf.RoundToInt(countdownTimer);
                countdownTimer -= Time.deltaTime;

                if(oldTimer != Mathf.RoundToInt(countdownTimer))
                {
                    DisplayTimer();
                }
            }
            else
            {
                timesUp = true;
                timerText.text = "END";
                Debug.Log("End of battle");
                // Do Something
            }
        }
    }

    void DisplayTimer()
    {
        // METHOD TO DISPLAY IN "00:00" FORMAT
        /* 
        minutes = Mathf.FloorToInt(countdownTimer / 60);
        seconds = Mathf.FloorToInt(countdownTimer % 60);
        if (seconds >= 10)
            timerText.text = minutes + ":" + seconds;
        else
            timerText.text = minutes + ":0" + seconds;
        */

        // METHOD TO DISPLAY SECONDS
        timerText.text = Mathf.RoundToInt(countdownTimer).ToString();
    }
}
