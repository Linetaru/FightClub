using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRoundPanel : MonoBehaviour
{
    [ReadOnly]
    public bool animationOver = true;

    public void AnimationPlay()
    {
        animationOver = false;
    }

    public void AnimationOver()
    {
        animationOver = true;
    }
}
