using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    [SerializeField]
    private Text playerName;
    public Text PlayerName { get { return playerName; } set { playerName = value; } }

    [SerializeField]
    private Text currentState;
    public Text CurrentState { get { return currentState; } set { currentState = value; } }

    [SerializeField]
    private Text speedX;
    public Text SpeedX { get { return speedX; } set { speedX = value; } }

    [SerializeField]
    private Text speedY;
    public Text SpeedY { get { return speedY; } set { speedY = value; } }

    [SerializeField]
    private Text inputs;
    public Text Inputs { get { return inputs; } set { inputs = value; } }


}
