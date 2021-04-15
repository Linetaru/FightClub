using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProjectile : MonoBehaviour
{
    private List<Projectile> listProjectiles = new List<Projectile>();

    [SerializeField]
    private int projectileMax = 1;

    public void AddProjectile(Projectile projectile)
    {
        listProjectiles.Add(projectile);
    }

    public void TriggerAll()
    {
        foreach(Projectile p in listProjectiles)
        {
            p.Explode();
            //listProjectiles.Remove(p);
        }
        listProjectiles.Clear();
    }

    public bool CanShootProjectile()
    {
        return listProjectiles.Count < projectileMax;
    }
}
