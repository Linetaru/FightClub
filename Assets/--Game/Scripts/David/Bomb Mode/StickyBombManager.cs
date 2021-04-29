using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class StickyBombManager : MonoBehaviour
{
    public BattleManager battleManager;
	public BattleManager BattleManager
    {
        get { return battleManager; }
        set { battleManager = value; }
    }

    private CharacterBase oldBombedPlayer;
    private CharacterBase currentBombedPlayer;

    //private UnityEvent<string> onHitColliderEvents;

    void Start()
    {
        InitStickyBomb();
    }


    void Update()
    {
        
    }
    public void TestHit(string tag)
    {
        Debug.Log(tag + " s'est fait rekt");
    }

    public void InitStickyBomb()
    {
        int firstPlayerBomb = Random.Range(0, battleManager.characterAlive.Count);

        currentBombedPlayer = battleManager.characterAlive[firstPlayerBomb];

        UpdateBombIcons();

    }

    public void UpdateBombIcons()
    {
        if (oldBombedPlayer != null)
            oldBombedPlayer.CharacterIcon.SwitchIcon();

        currentBombedPlayer.CharacterIcon.SwitchIcon();
    }

}
