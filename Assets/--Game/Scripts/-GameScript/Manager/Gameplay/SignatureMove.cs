using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Tools
{
	public static void SetLayerRecursively(this Transform parent, int layer)
	{
		parent.gameObject.layer = layer;

		for (int i = 0, count = parent.childCount; i < count; i++)
		{
			parent.GetChild(i).SetLayerRecursively(layer);
		}
	}
}

public class SignatureMove : MonoBehaviour
{
	[SerializeField]
	CharacterModel[] characterModelPlayer;

	// Placeholder vu qu'on a que un perso pour le moment
	[SerializeField]
	CharacterModel[] characterModelEnemy;
	[SerializeField]
	List<CharacterModel> modelEnemy;

	CharacterBase target;
	public delegate void Action();
	public event Action OnEnd;

	public void StartSignatureMove(CharacterBase user, CharacterBase target)
	{
		for (int i = 0; i < characterModelPlayer.Length; i++)
		{
			characterModelPlayer[i].SetColor(0, user.Model.GetColor());
		}

		// Placeholder
		for (int i = 0; i < characterModelEnemy.Length; i++)
		{
			//characterModelEnemy[i].SetColor(0, target.Model.GetColor());
			//characterModelEnemy[i].gameObject.SetActive(false);
			modelEnemy.Add(Instantiate(target.Model, characterModelEnemy[i].transform.parent));
			//modelEnemy[i].transform.localPosition += (target.CenterPoint.localPosition - user.CenterPoint.localPosition);
			Tools.SetLayerRecursively(modelEnemy[modelEnemy.Count-1].transform, 25);
			characterModelEnemy[i].gameObject.SetActive(false);
		}

		this.target = target;
		//StartCoroutine(SignatureMoveCoroutine());
	}

	/*private IEnumerator SignatureMoveCoroutine()
	{
		yield return null;
		yield return null;
		for (int i = 0; i < modelEnemy.Count; i++)
		{
			modelEnemy[i].GetComponent<Animator>().SetTrigger("Knockback");
		}

	}*/

	public void AddDamage(float damage)
	{
		target.Stats.LifePercentage += damage;
	}
	public void SetTargetIdle(int id)
	{
		modelEnemy[id].GetComponent<Animator>().SetTrigger("Idle");
	}
	public void SetTargetHit(int id)
	{
		if(id >= modelEnemy.Count) { return; }

		modelEnemy[id].GetComponent<Animator>().SetTrigger("Knockback");
	}
	public void SetAnimatorSpeed(int id)
	{
		modelEnemy[id].GetComponent<Animator>().speed = 0.1f;
	}
	public void EndSignatureMove()
	{		
		OnEnd.Invoke();
		Destroy(this.gameObject);
	}
}
