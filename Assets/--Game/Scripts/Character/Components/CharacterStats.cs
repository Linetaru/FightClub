using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Life")]
    [SerializeField]
    private float lifePercentage;
    public float LifePercentage
    {
        get { return lifePercentage; }
        //set { lifePercentage = value; }
    }

    [SerializeField]
    private int lifeStocks;
    public int LifeStocks
    {
        get { return lifeStocks; }
        //set { lifeStocks = value; }
    }

    [Header("Movement")]
    [SerializeField]
    private float speed;
    public float Speed
    {
        get { return speed; }
        //set { speed = value; }
    }

    [SerializeField]
    private float gravity;
    public float Gravity
    {
        get { return gravity; }
        //set { gravity = value; }
    }

    [SerializeField]
    private float groundAcceleration;
    public float GroundAcceleration
    {
        get { return groundAcceleration; }
        //set { groundAcceleration = value; }
    }

    [SerializeField]
    private float groundDeceleration;
    public float GroundDeceleration
    {
        get { return groundDeceleration; }
        //set { groundDeceleration = value; }
    }

    [SerializeField]
    private float airAcceleration;
    public float AirAcceleration
    {
        get { return airAcceleration; }
        //set { airAcceleration = value; }
    }

    [SerializeField]
    private float airDeceleration;
    public float AirDeceleration
    {
        get { return airDeceleration; }
        //set { airDeceleration = value; }
    }

    [Header("Attack")]
    [SerializeField]
    private float attackPower;
    public float AttackPower
    {
        get { return attackPower; }
        //set { attackPower = value; }
    }

    [SerializeField]
    private float throwPower;
    public float ThrowPower
    {
        get { return throwPower; }
        //set { throwPower = value; }
    }

    [SerializeField]
    private float knockbackPower;
    public float KnockbackPower
    {
        get { return knockbackPower; }
        //set { knockbackPower = value; }
    }
}
