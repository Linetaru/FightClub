using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterParry : MonoBehaviour
{

	[Title("States")]
	[SerializeField]
	CharacterState parrySuccesState;
	public CharacterState ParrySuccesState
	{
		get { return parrySuccesState; }
	}

	[SerializeField]
	CharacterState parryRepelState;
	public CharacterState ParryRepelState
	{
		get { return parryRepelState; }
	}

	[Title("Parameter")]
	[SerializeField]
	private int[] timingParry;
	public int[] TimingParry
	{
		get { return timingParry; }
	}



	private int parryNumber;
	public int ParryNumber
	{
		get { return parryNumber; }
		set { parryNumber = value; }
	}

	private bool isParry;
	public bool IsParry
	{
		get { return isParry; }
		set { isParry = value; }
	}



	[Title("Particle - A virer plus tard")]
	[SerializeField]
	GameObject particleParry;
	public GameObject ParticleParry
	{
		get { return particleParry; }
	}


	// Faire une interface ou une classe abstraire pour attackManager
	public virtual bool CanParry(AttackManager attackManager)
	{
		return isParry;
	}
	public virtual bool CanParry(AttackSubManager attackManager)
	{
		return isParry;
	}

	/// <summary>
	/// Fonction à utiliser sur celui qui parry
	/// </summary>
	/// <param name="user"></param>
	public virtual void Parry(CharacterBase user, CharacterBase target)
	{
		isParry = false;
		user.SetMotionSpeed(0, 0.3f);
		user.Action.CancelAction();
		user.PowerGauge.ForceAddPower(20);

		user.SetState(parrySuccesState);
		user.Action.HasHit(target);

		Vector2 angleEjection = (target.transform.position - this.transform.position).normalized;
		GameObject go = Instantiate(particleParry, user.Knockback.ContactPoint, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
		go.name = particleParry.name;
		Destroy(go, 1f);
	}

	/// <summary>
	/// Fonction à utiliser sur celui qui se fait parry
	/// </summary>
	/// <param name="user"></param>
	public virtual void ParryRepel(CharacterBase user, CharacterBase target)
	{
		isParry = false;
		user.SetMotionSpeed(0f, 0.3f);
		user.Action.CancelAction();



		Vector2 angleEjection = (user.transform.position - target.transform.position).normalized;
		user.Knockback.Launch(angleEjection, 1);

		user.SetState(parryRepelState);
	}

}
