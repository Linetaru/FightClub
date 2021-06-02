using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterParry : MonoBehaviour
{

	[Title("States")]
	[SerializeField]
	CharacterState parrySuccesState = null;

	[SerializeField]
	CharacterState parryRepelState = null;


	[Title("Parameter")]
	[SerializeField]
	private StatusData guardBreakStatus;
	public StatusData GuardBreakStatus
	{
		get { return guardBreakStatus; }
	}

	/*[SerializeField]
	private int[] timingParry;
	public int[] TimingParry
	{
		get { return timingParry; }
	}*/


	[SerializeField]
	private float ejectionPower = 16;
	public float EjectionPower
	{
		get { return ejectionPower; }
	}


	[SerializeField]
	private float parryAngle;
	public float ParryAngle
	{
		get { return parryAngle; }
		set { parryAngle = value; } // Pour les tests, ne pas mettre en write sinon
	}





	private Vector2 parryDirection;
	public Vector2 ParryDirection
	{
		get { return parryDirection; }
		set { parryDirection = value; }
	}



	private int parryNumber;
	public int ParryNumber
	{
		get { return parryNumber; }
		set { parryNumber = value; }
	}

	private bool isParry = false;
	public bool IsParry
	{
		get { return isParry; }
		set { isParry = value; }
	}

	private bool isGuard = false;
	public bool IsGuard
	{
		get { return isGuard; }
		set { isGuard = value; }
	}

	private bool isGuardDash = false;
	public bool IsGuardDash
	{
		get { return isGuardDash; }
		set { isGuardDash = value; }
	}


	CharacterBase characterParried = null;
	public CharacterBase CharacterParried
	{
		get { return characterParried; }
	}





	public event EventCharacterBase OnParry;
	public event EventCharacterBaseDouble OnGuard;
	public event EventAttackSubManager OnClash;




	[Title("Particle - A virer plus tard")]
	[SerializeField]
	GameObject particleParry = null;
	public GameObject ParticleParry
	{
		get { return particleParry; }
	}

	[SerializeField]
	GameObject particleDirectionRepel = null;
	public GameObject ParticleDirectionRepel
	{
		get { return particleDirectionRepel; }
	}

	[SerializeField]
	GameObject particleGuard = null;
	[SerializeField]
	GameObject particleGuardMedium = null;
	[SerializeField]
	GameObject particleGuardCritical = null;

	[SerializeField]
	GameObject particleGuardBreak = null;

	[SerializeField]
	GameObject particleGuardDirectionRepel = null;


	// Faire une interface ou une classe abstraire pour attackManager
	public virtual bool CanParry(AttackManager attackManager)
	{
		return isParry;
	}
	/*public virtual bool CanParry(AttackSubManager attackManager)
	{
		return isParry;
	}*/

	// Directionnal
	public virtual bool CanParry(AttackSubManager attackManager)
	{
		if (isParry == false)
			return false;
		else if (attackManager.BreakParry == true)
			return false;
		return CheckAngle(attackManager);
	}

	/*public virtual bool CanGuard(AttackSubManager attackManager)
	{
		return isGuard;
	}*/

	public virtual bool CanGuard(AttackSubManager attackManager)
	{
		if (isGuardDash == true && attackManager.GuardOnDash == true)
			return true;
		if (isGuard == false)
			return false;
		else if (attackManager.BreakGuard == true)
			return false;
		return CheckAngle(attackManager);
	}

	public bool CheckAngle(AttackSubManager attackManager)
	{
		float angle = Vector2.Angle((attackManager.transform.position - this.transform.position), parryDirection);
		//return (parryDirection - (parryAngle *0.5f) < angle && angle < parryDirection + (parryAngle * 0.5f));
		return angle < (parryAngle * 0.5f);
	}

	public virtual void Clash(CharacterBase user, AttackSubManager attack)
	{
		AttackSubManager attackUser = attack.AttackClashed;
		AttackSubManager attackEnemy = attack;
		if (attackUser.User == user) // C'est bien notre attaque
		{
			if(attackEnemy.ClashLevel > attackUser.ClashLevel) // User est repoussé
			{
				attackEnemy.User.Knockback.Parry.ParryResolution(attackEnemy.User, attackUser);
				/*attackEnemy.User.Knockback.Parry.Parry(attackEnemy.User, user);
				ParryRepel(user, attackEnemy.User);*/
			}
			else if (attackUser.ClashLevel > attackEnemy.ClashLevel) // Enemy est repoussé
			{
				ParryResolution(user, attackEnemy);
				/*Parry(user, attackEnemy.User);
				attackEnemy.User.Knockback.Parry.ParryRepel(attackEnemy.User, user);*/
			}
			else // Match nul
			{
				if(attackUser.Disjoint == false)
					user.SetMotionSpeed(0, 0.2f);

				if(attackEnemy.Disjoint == false)
					attackEnemy.User.SetMotionSpeed(0f, 0.2f);

				Vector2 angleEjection = (user.transform.position - this.transform.position).normalized;
				GameObject go = Instantiate(particleParry, (user.Knockback.ContactPoint + attackEnemy.User.Knockback.ContactPoint) * 0.5f, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
				go.name = particleParry.name;
				Destroy(go, 1f);

				attackUser.ActionUnactive();
				attackEnemy.ActionUnactive();

				attackUser.Clash(attackEnemy.User);
				attackEnemy.Clash(attackUser.User);

				OnClash?.Invoke(attackEnemy);

			}

		}
	}


	public virtual void ParryResolution(CharacterBase character, AttackSubManager atkRegistered)
	{
		atkRegistered.AddPlayerHitList(character.tag);

		Parry(character, atkRegistered.User);
		if (atkRegistered.Disjoint == false)
		{
			atkRegistered.User.Knockback.Parry.ParryRepel(atkRegistered.User, character, !atkRegistered.NoParryCancel);
		}


		// Pour tourner le joueur dans le sens de la parry
		if (Mathf.Sign(atkRegistered.User.transform.position.x - character.transform.position.x) != character.Movement.Direction)
			character.Movement.Direction *= -1;

		// Feedback

		Vector2 angleEjection = (atkRegistered.User.transform.position - character.transform.position).normalized;
		GameObject go2 = Instantiate(particleParry, character.Knockback.ContactPoint, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
		go2.name = particleParry.name;
		Destroy(go2, 1f);

		if (atkRegistered.NoParryCancel == false)
		{
			GameObject go = Instantiate(particleDirectionRepel, character.Knockback.ContactPoint, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
			go.name = particleParry.name;
			Destroy(go, 1f);
		}

		atkRegistered.Parry(character);
	}


	public virtual void GuardResolution(CharacterBase character, AttackSubManager atkRegistered)
	{
		//	atkRegistered.User.Knockback.ContactPoint = character.Knockback.ContactPoint;
		if (atkRegistered.GuardWin == false)
		{
			if (character.PowerGauge.CurrentPower <= 1) // Guard Break
			{
				//character.PowerGauge.ForceAddPower(-20);
				character.PowerGauge.ForceAddPower(80);
				atkRegistered.User.PowerGauge.ForceAddPower(20);

				character.Knockback.Hit(character, atkRegistered);

				character.SetMotionSpeed(0.1f, 2f);
				atkRegistered.User.SetMotionSpeed(0.1f, 0.8f);
				character.Knockback.KnockbackDuration = 1;
				//character.Knockback.IsHardKnockback = true;
				character.Status.AddStatus(new Status("GuardBreak", guardBreakStatus));

				Vector2 angleEjection = character.Knockback.GetAngleKnockback().normalized;
				Feedbacks.GlobalFeedback.Instance.CameraRotationImpulse(new Vector2(-angleEjection.y, angleEjection.x) * 10, 0.8f);
				Feedbacks.GlobalFeedback.Instance.ZoomDramatic(character, 0.8f);

				// Feedback
				GameObject go2 = Instantiate(particleGuardBreak, character.CenterPoint.position, Quaternion.identity);
				go2.name = particleParry.name;
				Destroy(go2, 1f);

				return;
			}
			else 
			{
				Guard(character, atkRegistered.User);
			}
		}
		if (atkRegistered.Disjoint == false)
		{
			atkRegistered.User.Knockback.Parry.Parry(atkRegistered.User, character);
			//atkRegistered.User.PowerGauge.ForceAddPower(-20);
		}
		atkRegistered.AddPlayerHitList(character.tag);

		// Pour tourner le joueur dans le sens de la garde
		if (Mathf.Sign(atkRegistered.User.transform.position.x - character.transform.position.x) != character.Movement.Direction)
			character.Movement.Direction *= -1;

		// Feedback
		if (atkRegistered.GuardWin == true)
		{
			character.Knockback.ShakeEffect.Shake(0.1f, 0.3f);
			character.Model.FlashModel(Color.white, 0.5f);

			Vector2 angleEjection = (character.transform.position - atkRegistered.User.transform.position).normalized;
			GameObject go = Instantiate(particleParry, character.CenterPoint.position, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
			go.name = particleParry.name;
			Destroy(go, 1f);
		}
		else
		{
			Vector2 angleEjection = (character.transform.position - atkRegistered.User.transform.position).normalized;

			if (character.PowerGauge.CurrentPower <= character.PowerGauge.maxPower * 0.33f)
			{
				GameObject go2 = Instantiate(particleGuardCritical, character.CenterPoint.position, Quaternion.identity);
				go2.name = particleParry.name;
				Destroy(go2, 1f);
			}
			else if (character.PowerGauge.CurrentPower <= character.PowerGauge.maxPower * 0.66f)
			{
				GameObject go2 = Instantiate(particleGuardMedium, character.CenterPoint.position, Quaternion.identity);
				go2.name = particleParry.name;
				Destroy(go2, 1f);
			}
			else if (character.PowerGauge.CurrentPower <= character.PowerGauge.maxPower)
			{
				GameObject go2 = Instantiate(particleGuard, character.CenterPoint.position, Quaternion.identity);
				go2.name = particleParry.name;
				Destroy(go2, 1f);
			}

			GameObject go = Instantiate(particleGuardDirectionRepel, character.CenterPoint.position, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
			go.name = particleParry.name;
			Destroy(go, 1f);
		}

		atkRegistered.Guard(character);
	}









	/// <summary>
	/// Fonction à utiliser sur celui qui parry
	/// </summary>
	/// <param name="user"></param>
	public virtual void Parry(CharacterBase characterParry, CharacterBase characterRepelled)
	{
		isParry = false;
		characterParry.Knockback.ShakeEffect.Shake(0.05f, 0.1f);
		characterParry.SetMotionSpeed(0, 0.35f);
		characterParry.Action.CancelAction();

		characterParry.PowerGauge.ForceAddPower(20);


		characterParry.SetState(parrySuccesState);
		characterParry.Action.HasHit(characterRepelled);

		characterParried = characterRepelled;

		Vector2 angleEjection = (characterRepelled.transform.position - characterParry.transform.position).normalized;

		OnParry?.Invoke(characterParried);
		Feedbacks.GlobalFeedback.Instance.CameraRotationImpulse(new Vector2(-angleEjection.y, angleEjection.x) * 4, 0.15f);
	}

	/// <summary>
	/// Fonction à utiliser sur celui qui se fait parry
	/// </summary>
	/// <param name="user"></param>
	public virtual void ParryRepel(CharacterBase characterRepelled, CharacterBase characterParry, bool repel = true)
	{
		isParry = false;
		characterRepelled.Knockback.ShakeEffect.Shake(0.12f, 0.3f);
		characterRepelled.SetMotionSpeed(0f, 0.35f);
		//characterRepelled.Action.CancelAction();

		characterRepelled.Model.FlashModel(Color.white, 0.7f);

		if (repel == true)
		{
			float ejectionPower = characterParry.Knockback.Parry.EjectionPower;

			Vector2 angleEjection = (characterRepelled.transform.position - characterParry.transform.position).normalized;
			characterRepelled.Knockback.Launch(angleEjection, ejectionPower);

			characterRepelled.Action.CancelAction();
			characterRepelled.SetState(parryRepelState);
		}
		else
		{
			characterRepelled.Action.ActionAllUnactive();
		}

	}


	public virtual void Guard(CharacterBase characterRepelled, CharacterBase characterParry)
	{
		isParry = false;
		characterRepelled.Knockback.ShakeEffect.Shake(0.12f, 0.5f);
		characterRepelled.SetMotionSpeed(0f, 0.35f);
		characterRepelled.Action.CancelAction();


		characterRepelled.PowerGauge.ForceAddPower(-20);
		characterParry.PowerGauge.ForceAddPower(-20);

		characterRepelled.Model.FlashModel(Color.white, 0.7f);


		float ejectionPower = characterParry.Knockback.Parry.EjectionPower;

		Vector2 angleEjection = (characterRepelled.transform.position - characterParry.transform.position).normalized;
		characterRepelled.Knockback.Launch(angleEjection, ejectionPower);

		characterRepelled.SetState(parryRepelState);
		OnGuard?.Invoke(characterRepelled, characterParry);

	}

}
