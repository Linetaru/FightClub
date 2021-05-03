using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombIcon : Icon
{
    private bool isFake;

    [SerializeField]
    private TextMeshPro bombTimerText;

    [SerializeField]
    private SpriteRenderer iconRenderer;
    [SerializeField]
    private MeshRenderer textRenderer;

    private float bombTimer;

    private int bombTimerInSeconds;

    private bool timeOut;

    private void Start()
    {
        bombTimer = StickyBombManager.Instance.bombTimer;
    }
    void Update()
    {
        bombTimer -= Time.deltaTime;
        bombTimerInSeconds = (int) bombTimer % 60;
        bombTimerText.text = bombTimerInSeconds.ToString();

        if(bombTimer <= 0f && !timeOut)
        {
            timeOut = true;
            if(!isFake)
                StickyBombManager.Instance.TimeOutManager();
        }
    }
    public override void SwitchIcon(bool isFake)
    {

        if(IsEnabled)
        {
            IsEnabled = false;
            iconRenderer.enabled = false;
            textRenderer.enabled = false;
        }
        else
        {
            this.isFake = isFake;
            IsEnabled = true;
            iconRenderer.enabled = true;
            textRenderer.enabled = true;
        }
    }
}
