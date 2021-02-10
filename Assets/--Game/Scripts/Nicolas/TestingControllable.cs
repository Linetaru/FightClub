using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired.UI.ControlMapper;

public class TestingControllable : MonoBehaviour, IControllable
{
	private ControlMapper controlMapper;
	public GameObject panelPrincipal;

	// Start is called before the first frame update
	void Start()
	{
		controlMapper = GameObject.FindObjectOfType<ControlMapper>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void UpdateControl(int ID, Input_Info input_Info)
	{
		if (input_Info.inputUiAction == InputConst.Pause)
		{
			input_Info.inputUiAction = null;
			if (!controlMapper.isOpen)
			{
				controlMapper.Open();
				panelPrincipal.SetActive(false);
			}
			else
			{
				controlMapper.Close(true);
				panelPrincipal.SetActive(true);
			}

		}
	}
}