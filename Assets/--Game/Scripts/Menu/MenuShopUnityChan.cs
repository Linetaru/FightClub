using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Menu
{
	[System.Serializable]
	public class UnityChanBehavior
	{
		public string Text;
		public AnimationClip unityAnim;
		public AnimationClip unityFace;
	}


	public class MenuShopUnityChan : MonoBehaviour
	{
		[SerializeField]
		MenuList menuList;

		[Title("Unity chan")]
		[SerializeField]
		Animator animatorUnityChan;
		[SerializeField]
		Textbox textboxUnityChan;

		[Title("Unity chan dialog")]
		[SerializeField]
		UnityChanBehavior[] greetings;
		[SerializeField]
		Vector2 waitTime;
		[SerializeField]
		UnityChanBehavior[] waiting;
		[SerializeField]
		UnityChanBehavior[] quiting;

		private void Start()
		{
			StartCoroutine(UnityChanMouthCoroutine());
			PlayUnityChanAnimation(greetings[Random.Range(0, greetings.Length)]);
			//menuList.OnSelected
		}



		private void PlayUnityChanAnimation(UnityChanBehavior behavior)
		{
			if (behavior.unityAnim != null) 
			{
				animatorUnityChan.Play(behavior.unityAnim.name, 0);
				StartCoroutine(UnityChanAnimationCoroutine(behavior.unityAnim.length));
			}

			if (behavior.unityFace == null)
			{
				animatorUnityChan.SetLayerWeight(1, 0);
			}
			else
			{
				animatorUnityChan.SetLayerWeight(1, 1);
				animatorUnityChan.Play(behavior.unityFace.name, 1);
			}

			if (behavior.Text.Length > 0)
			{
				textboxUnityChan.DrawTextbox(behavior.Text);
			}
		}

		private IEnumerator UnityChanAnimationCoroutine(float time)
		{
			yield return new WaitForSeconds(time);
			animatorUnityChan.SetTrigger("Idle");
			yield return new WaitForSeconds(Random.Range(waitTime.x, waitTime.y));
			PlayUnityChanAnimation(waiting[Random.Range(0, waiting.Length)]);
		}

		private IEnumerator UnityChanMouthCoroutine()
		{
			while (true)
			{
				int t = 0;
				float timeBlend = 0.05f;
				while (textboxUnityChan.isTextFinished() == false)
				{
					char c = textboxUnityChan.CharacterDrawed();
					if (c == 'a' || c == 'A')
					{
						//animatorUnityChan.Play("MTH_Close");
						animatorUnityChan.CrossFadeInFixedTime("MTH_A", timeBlend, 2);
						t = 0;
						//animatorUnityChan.Play("MTH_A", 2);
					}
					else if (c == 'i' || c == 'I')
					{
						//animatorUnityChan.Play("MTH_Close");
						animatorUnityChan.CrossFadeInFixedTime("MTH_I", timeBlend, 2);
						t = 0;
						//animatorUnityChan.Play("MTH_I", 2);
					}
					else if (c == 'u' || c == 'U')
					{
						//animatorUnityChan.Play("MTH_Close");
						animatorUnityChan.CrossFadeInFixedTime("MTH_U", timeBlend, 2);
						t = 0;
						//animatorUnityChan.Play("MTH_U", 2);
					}
					else if (c == 'e' || c == 'E')
					{
						//animatorUnityChan.Play("MTH_Close");
						animatorUnityChan.CrossFadeInFixedTime("MTH_E", timeBlend, 2);
						t = 0;
						//animatorUnityChan.Play("MTH_E", 2);
					}
					else if (c == 'o' || c == 'O')
					{
						//animatorUnityChan.Play("MTH_Close");
						animatorUnityChan.CrossFadeInFixedTime("MTH_O", timeBlend, 2);
						t = 0;
						//animatorUnityChan.Play("MTH_O", 2);
					}
					else
					{
						t += 1;
						if (t == 10)
							animatorUnityChan.CrossFadeInFixedTime("MTH_Close", timeBlend, 2);
					}
					yield return null;
				}
				animatorUnityChan.CrossFadeInFixedTime("MTH_Close", 0.2f, 2);
				yield return null;
			}
		}
	}
}
