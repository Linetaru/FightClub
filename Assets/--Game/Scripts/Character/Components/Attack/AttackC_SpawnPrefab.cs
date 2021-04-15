using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_SpawnPrefab : AttackComponent
{

	[Title("General")]
	[SerializeField]
	private GameObject prefabToSpawn;

	[SerializeField]
	private bool destroyWithAttack;

	GameObject go;

	[Title("Projectile")]
	[SerializeField]
	private bool isProjectile;

	/*
	[ShowIf("isProjectile")]
	public float projectileSpeed;
	*/

	// Appelé au moment où l'attaque est initialisé
	public override void StartComponent(CharacterBase user)
	{
		if (prefabToSpawn != null)
		{

			if(!isProjectile)
			{
				//Si pas un projectile, on le stick au joueur
				go = Instantiate(prefabToSpawn, transform.position, prefabToSpawn.transform.rotation);
				go.transform.parent = transform.root;
			}
			else
            {
				if(user.Projectile.CanShootProjectile())
				{
					go = Instantiate(prefabToSpawn, transform.position, prefabToSpawn.transform.rotation);
					Projectile projectile = go.GetComponent<Projectile>();
					user.Projectile.AddProjectile(projectile);
					//projectile.Speed = projectileSpeed;
					projectile.Direction = user.Movement.Direction;
					projectile.User = user;
				}
            }
		}
	}
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
		
    }
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
		
    }
	
	// Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {
		if(destroyWithAttack)
			Destroy(go);
    }
}
