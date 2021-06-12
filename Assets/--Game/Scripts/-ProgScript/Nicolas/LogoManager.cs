using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogoManager : MonoBehaviour
{
	//public Image teamImage;
	//public Image gameImage;
	public string menuPrincipalName;

	private Animator animator;

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		animator.SetTrigger("Team");
	}

	public void TeamToGame()
    {
		animator.SetTrigger("Game");
	}

	public void GameToMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("");
	}
}