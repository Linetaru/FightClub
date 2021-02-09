using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private List<AttackComponent> atkCompList;

    public void Start()
    {

    }

    public void ActionActive()
    {

    }

    public void CancelAction()
    {

    }

    public void Hit()
    {
        foreach(AttackComponent atkC in atkCompList)
        {
            atkC.OnHit();
        }
    }

}
