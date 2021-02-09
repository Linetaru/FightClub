using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterKnockback : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<AttackManager>() != null)
        {
            AttackManager atkMan = other.GetComponent<AttackManager>();

            atkMan.Hit(this);
        }
    }
}
