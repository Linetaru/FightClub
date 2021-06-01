using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_ProjectileTP : AttackComponent
{
	[SerializeField]
	AttackManager projectilePrefab;
	[SerializeField]
	AttackManager followUpTP;

	[SerializeField]
	[SuffixLabel("en frames")]
	float frameProjectileAppear = 12;
	[SerializeField]
	[SuffixLabel("en frames")]
	float frameTeleportation = 50;

	/*[SerializeField]
	Transform muzzle;*/


	float t = 0;
    AttackManager projectile;
	bool projectileAppear = true;


	public override void StartComponent(CharacterBase user)
	{
		t = 0;
		frameProjectileAppear /= 60;
		frameTeleportation /= 60;
		projectileAppear = false;
	}

	public override void UpdateComponent(CharacterBase user)
	{
		t += Time.deltaTime * user.MotionSpeed;
		if (t >= frameProjectileAppear && projectileAppear == false) 
		{
			projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
			projectile.CreateAttack(user);
			projectileAppear = true;
		}


		if (t >= frameTeleportation && projectileAppear == true)
		{
			user.transform.position = projectile.transform.position;
			projectile.CancelAction();
			projectile = null;

			user.Action.CancelAction();
			user.Action.Action(followUpTP);
			return;
		}

	}

	public override void EndComponent(CharacterBase user)
	{
		if(projectile != null)
			projectile.CancelAction();
	}

	public override void OnHit(CharacterBase user, CharacterBase target)
	{
	}

	public override void OnParry(CharacterBase user, CharacterBase target)
	{


	}
	public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
	{

	}
	public override void OnClash(CharacterBase user, CharacterBase target)
	{
		
	}
}
