using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandSlamUi : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreInfosPanel;


    public void ActivePanelScore()
    {
        scoreInfosPanel.SetActive(true);
    }

    public void DeactivePanelScore()
    {
        scoreInfosPanel.SetActive(false);
    }

}
