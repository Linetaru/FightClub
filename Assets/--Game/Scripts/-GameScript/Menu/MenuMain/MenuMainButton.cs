using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuMainButton : MonoBehaviour
{
	[SerializeField]
	GameObject arrow = null;
	[SerializeField]
	Animator animator = null;

	[SerializeField]
	UnityEvent unityEvent = null;


	public void CallEvent()
    {
		unityEvent.Invoke();
    }
	
	public void Selected()
    {
		if(arrow != null)
			arrow.gameObject.SetActive(true);
		animator.SetTrigger("Selected");
	}

	public void Unselected()
    {
		if (arrow != null)
			arrow.gameObject.SetActive(false);
		animator.SetTrigger("Unselected");
	}
}
