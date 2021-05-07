using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombIcon : Icon
{
    [SerializeField]
    private TextMeshPro bombTimerText;

    [SerializeField]
    private SpriteRenderer iconRenderer;
    [SerializeField]
    private MeshRenderer textRenderer;

    private float bombTimer;

    private void Start()
    {

    }

    void Update()
    {
        // Pas sûr de la performance 
        bombTimer = (int)StickyBombManager.Instance.bombTimer % 60;
        bombTimerText.text = bombTimer.ToString();
    }

    public override void SwitchIcon()
    {
        if(IsEnabled)
        {
            IsEnabled = false;
            iconRenderer.enabled = false;
            textRenderer.enabled = false;
        }
        else
        {
            IsEnabled = true;
            iconRenderer.enabled = true;
            if(StickyBombManager.Instance.CurrentRoundMode != StickyBombManager.RoundMode.Inv_Countdown)
                textRenderer.enabled = true;
        }
    }
    public override void DestroySelf()
    {
        Destroy(gameObject);
    }
}
