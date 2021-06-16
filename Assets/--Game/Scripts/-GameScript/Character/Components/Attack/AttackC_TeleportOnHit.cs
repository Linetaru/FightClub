using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_TeleportOnHit : AttackComponent
{
	public override void OnHit(CharacterBase user, CharacterBase target)
	{
		user.Action.MoveCancelable();
		user.transform.position = this.transform.position;
	}
}
