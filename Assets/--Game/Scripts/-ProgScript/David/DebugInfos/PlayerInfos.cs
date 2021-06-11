using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    [SerializeField]
    private Text playerName = null;
    public Text PlayerName { get { return playerName; } set { playerName = value; } }

    [SerializeField]
    private Text currentState = null;
    public Text CurrentState { get { return currentState; } set { currentState = value; } }

    [SerializeField]
    private Text speedX = null;
    public Text SpeedX { get { return speedX; } set { speedX = value; } }

    [SerializeField]
    private Text speedY = null;
    public Text SpeedY { get { return speedY; } set { speedY = value; } }

    [SerializeField]
    private Text startupText = null;
    public Text StartupText { get { return startupText; } set { startupText = value; } }

    [SerializeField]
    private Text inputs = null;
    public Text Inputs { get { return inputs; } set { inputs = value; } }


    [SerializeField]
    public Transform knockbackTime = null;
    //public Transform Transform { get { return transform; } set { transform = value; } }
}
