using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_CharaRotation : AttackComponent
{
	public Transform parent;
	public float offset;

	// Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
		user.Model.transform.parent.transform.eulerAngles = new Vector3(user.Model.transform.parent.transform.eulerAngles.x, user.Model.transform.parent.transform.eulerAngles.y, 
			Mathf.Atan2(user.Movement.SpeedX * user.Movement.Direction, user.Movement.SpeedY) * Mathf.Rad2Deg + (offset * user.Movement.Direction));

		parent.localScale = new Vector3(Mathf.Abs(parent.localScale.x) * user.transform.localScale.x * user.Movement.Direction,
										   parent.localScale.y * user.transform.localScale.y,
										   parent.localScale.z * user.transform.localScale.z);
		parent.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Mathf.Abs(user.Movement.SpeedX) * user.Movement.Direction, -user.Movement.SpeedY) * Mathf.Rad2Deg + (offset * user.Movement.Direction));
		//Debug.Log(Mathf.Atan2(user.Movement.SpeedX * user.Movement.Direction, user.Movement.SpeedY) * Mathf.Rad2Deg);
		/*if (user.Movement.Direction == 1)
			parent.eulerAngles = new Vector3(0, 0, Mathf.Atan2(user.Movement.SpeedX, user.Movement.SpeedY) * Mathf.Rad2Deg - offset);
		if (user.Movement.Direction == -1)
			parent.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Mathf.Abs(user.Movement.SpeedX) * user.Movement.Direction, user.Movement.SpeedY) * Mathf.Rad2Deg - offset);*/

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

	public override void OnParry(CharacterBase user, CharacterBase target)
	{

	}
	public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
	{

	}
	public override void OnClash(CharacterBase user, CharacterBase target)
	{

	}

	// Appelé au moment de la destruction de l'attaque
	public override void EndComponent(CharacterBase user)
    {
		user.Model.transform.parent.transform.eulerAngles = new Vector3(user.Model.transform.eulerAngles.x, user.Model.transform.eulerAngles.y, 0);
	}
}
