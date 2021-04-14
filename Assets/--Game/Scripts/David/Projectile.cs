using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private CharacterBase user;
    public CharacterBase User
    {
        get { return user; }
        set { user = value; }
    }

    private float speed = 0.0f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private int direction;
    public int Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private Vector3 projectileDir;


    void Start()
    {
        if (direction > 0)
            projectileDir = transform.right;
        else
            projectileDir = -transform.right;
    }

    void Update()
    {
        transform.position += projectileDir * speed * Time.deltaTime;
    }
}
