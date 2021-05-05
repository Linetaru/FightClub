using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StickyBombUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countDownText;
    [SerializeField]
    private TextMeshProUGUI currentModeText;

    public bool isCountdownOver;


    private void Start()
    {
    }

    public void LaunchCountDownAnim()
    {
        GetComponent<Animator>().Play("CountDownAnim");
    }

    public void LaunchCurrentModeAnim()
    {
        GetComponent<Animator>().Play("CurrentModeAnim");
    }

    public void ChangeCountdownValue(string value)
    {
        countDownText.text = value;
    }
    public void ChangeCurrentModeValue(string value)
    {
        currentModeText.text = value;
    }

    public void CountdownOver()
    {
        isCountdownOver = true;
    }

}
