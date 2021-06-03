using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Linq;

// Disclaimer : Le code n'est pas encore optimisé pour éviter les doublons de code !!!!!

public class ChoosenInputProfile : MonoBehaviour
{
	public TMP_InputField inputField;
	public TMP_Dropdown dropdown;
	public MenuManager menuManager;
	public InputController inputController;
	public GameObject baseProfile;
	public GameObject panelConfig;
	public Button doneDropdownButton;
	public Button createButton;
	public Button backButton;
	private ButtonInputData bt_InputData;

	[HideInInspector] public List<Button> profileCreate;
	public TMP_Dropdown[] dropdowns = new TMP_Dropdown[6];
	public List<InputMappingDataClassic> input;

	public Button inputButton, audioButton;

	private void Start()
    {
		doneDropdownButton.onClick.AddListener(AddTouchOnClickButtonDone);
	}

	public void Init()
	{
		input = InputMappingDataStatic.inputMappingDataClassics;
		if(input == null)
			input = new List<InputMappingDataClassic>();

		EventSystem.current.SetSelectedGameObject(createButton.gameObject);

		for (int i = 0; i < input.Count; i++)
		{
			GameObject go = Instantiate(baseProfile, this.transform.GetChild(0));
			go.GetComponent<ButtonInputData>().inputMappingData = input[i];
			go.GetComponentInChildren<TextMeshProUGUI>().text = input[i].profileName;
			go.SetActive(true);
			profileCreate.Add(go.GetComponent<Button>());
			go.GetComponent<Button>().onClick.AddListener(OnClickButton);
			Navigation NewNav = new Navigation();
			NewNav.mode = Navigation.Mode.Explicit;

			NewNav.selectOnRight = inputButton;
			NewNav.selectOnLeft = inputButton;

			if (profileCreate.Count == 1)
			{
				NewNav.selectOnUp = createButton;
				NewNav.selectOnDown = createButton;
				go.GetComponent<Button>().navigation = NewNav;
			}
			else
			{
				for (int z = 0; z < profileCreate.Count; z++)
				{
					if (z == 0)
					{
						NewNav.selectOnUp = createButton;
						NewNav.selectOnDown = profileCreate[z + 1];
					}
					else if (z == profileCreate.Count - 1)
					{
						NewNav.selectOnUp = profileCreate[z - 1];
						NewNav.selectOnDown = createButton;
					}
					else
					{
						NewNav.selectOnUp = profileCreate[z - 1];
						NewNav.selectOnDown = profileCreate[z + 1];
					}
					profileCreate[z].navigation = NewNav;
				}
			}
		}

		UpdateNavigation();

		Navigation NewNav2 = new Navigation();
		NewNav2.mode = Navigation.Mode.Explicit;

		if (profileCreate.Count > 0)
		{
			NewNav2.selectOnUp = profileCreate[profileCreate.Count - 1];
			NewNav2.selectOnDown = profileCreate[0];
		}
		NewNav2.selectOnRight = inputButton;
		NewNav2.selectOnLeft = inputButton;

		createButton.navigation = NewNav2;
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < dropdowns.Length; i++)
		{
			if (i == dropdowns.Length - 1 && dropdowns[i].IsExpanded)
				dropdowns[i].transform.GetChild(dropdowns[i].transform.childCount - 1).GetComponent<Canvas>().sortingOrder = -1;
			else if (dropdowns[i].IsExpanded)
				dropdowns[i].transform.GetChild(dropdowns[i].transform.childCount - 1).GetComponent<Canvas>().sortingOrder = dropdowns[i].GetComponent<Canvas>().sortingOrder;

			/*ColorBlock cl = new ColorBlock();
			cl.highlightedColor = Color.black;
			foreach (Toggle tg in dropdowns[i].gameObject.GetComponentsInChildren<Toggle>())
			{
				tg.colors = cl;
			}*/
		}
	}

	public void OnClickButton()
	{
		Button bt = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
		EventSystem.current.SetSelectedGameObject(dropdown.gameObject);
		ButtonInputData tmpBT = bt.GetComponent<ButtonInputData>();
		bt_InputData = tmpBT;
		dropdown.Select();
		panelConfig.SetActive(true);

		Navigation NewNav = new Navigation();
		NewNav.mode = Navigation.Mode.Explicit;

		for (int z = 0; z < dropdowns.Length; z++)
		{
			dropdowns[z].ClearOptions();
			if (z == 0)
			{
				NewNav.selectOnUp = doneDropdownButton;
				NewNav.selectOnDown = dropdowns[z + 1];
			}
			else if (z == dropdowns.Length - 1)
			{
				NewNav.selectOnUp = dropdowns[z - 1];
				NewNav.selectOnDown = doneDropdownButton;
			}
			else
			{
				NewNav.selectOnUp = dropdowns[z - 1];
				NewNav.selectOnDown = dropdowns[z + 1];
			}
			dropdowns[z].navigation = NewNav;
		}

		EnumInput[] inputVar = new EnumInput[]
		 {
			tmpBT.inputMappingData.inputJump,
			tmpBT.inputMappingData.inputShortHop,
			tmpBT.inputMappingData.inputAttack,
			tmpBT.inputMappingData.inputSpecial,
			tmpBT.inputMappingData.inputParry,
			tmpBT.inputMappingData.inputDash,
		 };
		//Verify imput name and inputVar are in same place !
		string[] name = new string[]
		{
			nameof(tmpBT.inputMappingData.inputJump),
			nameof(tmpBT.inputMappingData.inputShortHop),
			nameof(tmpBT.inputMappingData.inputAttack),
			nameof(tmpBT.inputMappingData.inputSpecial),
			nameof(tmpBT.inputMappingData.inputParry),
			nameof(tmpBT.inputMappingData.inputDash),
		};

		List<string> stringEnumInDropDown = new List<string>();

		for (int i = 0; i < dropdowns.Length; i++)
		{
			//foreach (string st in Enum.GetNames(typeof(EnumInput)))
			//{
			//	if (st != inputVar[i].ToString())
			//		stringEnumInDropDown.Add(st);
			//}

			string[] stringDropDown = new string[]
			{
				//name[i] + " : " + inputVar[i].ToString(),
				//name[i] + " : " + stringEnumInDropDown[0],
				//name[i] + " : " + stringEnumInDropDown[1],
				//name[i] + " : " + stringEnumInDropDown[2],
				//name[i] + " : " + stringEnumInDropDown[3],
				//name[i] + " : " + stringEnumInDropDown[4],
				name[i] + " : " + Enum.GetName(typeof(EnumInput), 0),
                name[i] + " : " + Enum.GetName(typeof(EnumInput), 1),
                name[i] + " : " + Enum.GetName(typeof(EnumInput), 2),
                name[i] + " : " + Enum.GetName(typeof(EnumInput), 3),
                name[i] + " : " + Enum.GetName(typeof(EnumInput), 4),
                name[i] + " : " + Enum.GetName(typeof(EnumInput), 5),
            };
			dropdowns[i].AddOptions(stringDropDown.ToList());
			dropdowns[i].value = (int)inputVar[i];
			//bt.GetComponent<ButtonInputData>().inputMappingData.inputAttack.ToString()
		}
	}

	public void OnDeselectInputField()
    {
		createButton.gameObject.SetActive(true);
		inputField.gameObject.SetActive(false);

		EventSystem.current.SetSelectedGameObject(createButton.gameObject);
		UpdateNavigation();
		Navigation NewNav = new Navigation();
		NewNav.mode = Navigation.Mode.Explicit;
		if (profileCreate.Count == 1)
		{
			NewNav.selectOnUp = createButton;
			NewNav.selectOnDown = createButton;
			profileCreate[0].navigation = NewNav;
		}
		else
		{
			for (int z = 0; z < profileCreate.Count; z++)
			{
				if (z == 0)
				{
					NewNav.selectOnUp = createButton;
					NewNav.selectOnDown = profileCreate[z + 1];
				}
				else if (z == profileCreate.Count - 1)
				{
					NewNav.selectOnUp = profileCreate[z - 1];
					NewNav.selectOnDown = createButton;
				}
				else
				{
					NewNav.selectOnUp = profileCreate[z - 1];
					NewNav.selectOnDown = profileCreate[z + 1];
				}
				profileCreate[z].navigation = NewNav;

			}
		}
	}

	private void AddTouchOnClickButtonDone()
	{
		//if(dropdownsBool[0])
		bt_InputData.inputMappingData.inputJump = (EnumInput)dropdowns[0].value;
		//if (dropdownsBool[1])
		bt_InputData.inputMappingData.inputShortHop = (EnumInput)dropdowns[1].value;
		//if (dropdownsBool[2])
		bt_InputData.inputMappingData.inputAttack = (EnumInput)dropdowns[2].value;
		//if (dropdownsBool[3])
		bt_InputData.inputMappingData.inputSpecial = (EnumInput)dropdowns[3].value;
		//if (dropdownsBool[4])
		bt_InputData.inputMappingData.inputParry = (EnumInput)dropdowns[4].value;
		//if (dropdownsBool[5])
		bt_InputData.inputMappingData.inputDash = (EnumInput)dropdowns[5].value;

		for(int i = 0; i < InputMappingDataStatic.inputMappingDataClassics.Count; i++)
		{
			if (bt_InputData.inputMappingData.profileName == InputMappingDataStatic.inputMappingDataClassics[i].profileName)
			{
				InputMappingDataStatic.inputMappingDataClassics[i] = bt_InputData.inputMappingData;
				Debug.Log("Remapping " + bt_InputData.GetComponent<ButtonInputData>().inputMappingData.profileName + " - " + InputMappingDataStatic.inputMappingDataClassics[i].profileName);
			}
		}

		panelConfig.SetActive(false);
		EventSystem.current.SetSelectedGameObject(bt_InputData.gameObject);
	}

	public void CreateButton()
    {
		if (input.Count < 6)
		{
			createButton.gameObject.SetActive(false);
			inputField.gameObject.SetActive(true);
			EventSystem.current.SetSelectedGameObject(inputField.gameObject);

			Navigation NewNav = new Navigation();
			NewNav.mode = Navigation.Mode.Explicit;
			if (profileCreate.Count == 1)
			{
				NewNav.selectOnUp = inputField;
				NewNav.selectOnDown = inputField;
				profileCreate[0].navigation = NewNav;
			}
			else
			{
				for (int z = 0; z < profileCreate.Count; z++)
				{
					if (z == 0)
					{
						NewNav.selectOnUp = inputField;
						NewNav.selectOnDown = profileCreate[z + 1];
					}
					else if (z == profileCreate.Count - 1)
					{
						NewNav.selectOnUp = profileCreate[z - 1];
						NewNav.selectOnDown = inputField;
					}
					else
					{
						NewNav.selectOnUp = profileCreate[z - 1];
						NewNav.selectOnDown = profileCreate[z + 1];
					}
					profileCreate[z].navigation = NewNav;

				}
			}
		}
		else
        {
			createButton.interactable = false;
		}
	}

	private void UpdateNavigation()
    {
		Navigation NewNav = new Navigation();
		NewNav.mode = Navigation.Mode.Explicit;

		if (profileCreate.Count > 0)
		{
			NewNav.selectOnRight = profileCreate[profileCreate.Count - 1];
			NewNav.selectOnLeft = profileCreate[profileCreate.Count - 1];
		}
		else
		{
			NewNav.selectOnRight = createButton;
			NewNav.selectOnLeft = createButton;
		}
		NewNav.selectOnDown = backButton;
		NewNav.selectOnUp = inputButton;

		audioButton.navigation = NewNav;

		if (profileCreate.Count > 0)
		{
			NewNav.selectOnRight = profileCreate[profileCreate.Count - 1];
			NewNav.selectOnLeft = profileCreate[profileCreate.Count - 1];
		}
		else
		{
			NewNav.selectOnRight = createButton;
			NewNav.selectOnLeft = createButton;
		}
		NewNav.selectOnDown = audioButton;
		NewNav.selectOnUp = backButton;

		inputButton.navigation = NewNav;

		if (profileCreate.Count > 0)
		{
			NewNav.selectOnRight = profileCreate[profileCreate.Count - 1];
			NewNav.selectOnLeft = profileCreate[profileCreate.Count - 1];
		}
		else
		{
			NewNav.selectOnRight = createButton;
			NewNav.selectOnLeft = createButton;
		}
		NewNav.selectOnDown = inputButton;
		NewNav.selectOnUp = audioButton;

		backButton.navigation = NewNav;
	}

	public void OnFinishInputField()
	{
		if (!inputField.text.Contains(" ") && inputField.text != "" && inputField.text.Count<char>() < 7 && input.Count < 6)
		{
			GameObject go = Instantiate(baseProfile, this.transform.GetChild(0));
			go.GetComponentInChildren<TextMeshProUGUI>().text = inputField.text;
			InputMappingDataStatic.inputMappingDataClassics.Add(new InputMappingDataClassic(inputField.text));
			go.GetComponent<ButtonInputData>().inputMappingData = InputMappingDataStatic.inputMappingDataClassics[InputMappingDataStatic.inputMappingDataClassics.Count - 1];
			Debug.Log("Creation " + go.GetComponent<ButtonInputData>().inputMappingData.profileName + InputMappingDataStatic.inputMappingDataClassics.Count);
			inputField.text = "";
            go.SetActive(true);
			profileCreate.Add(go.GetComponent<Button>());
			go.GetComponent<Button>().onClick.AddListener(OnClickButton);
			Navigation NewNav = new Navigation();
			NewNav.mode = Navigation.Mode.Explicit;

			NewNav.selectOnRight = inputButton;
			NewNav.selectOnLeft = inputButton;

			createButton.gameObject.SetActive(true);
			inputField.gameObject.SetActive(false);

			EventSystem.current.SetSelectedGameObject(createButton.gameObject);

			if (profileCreate.Count == 1)
			{
				NewNav.selectOnUp = createButton;
				NewNav.selectOnDown = createButton;
				go.GetComponent<Button>().navigation = NewNav;
			}
			else
			{
				for (int z = 0; z < profileCreate.Count; z++)
				{
					if (z == 0)
					{
						NewNav.selectOnUp = inputField;
						NewNav.selectOnDown = profileCreate[z + 1];
					}
					else if (z == profileCreate.Count - 1)
					{
						NewNav.selectOnUp = profileCreate[z - 1];
						NewNav.selectOnDown = inputField; 
					}
					else
					{
						NewNav.selectOnUp = profileCreate[z - 1];
						NewNav.selectOnDown = profileCreate[z + 1];
					}
					profileCreate[z].navigation = NewNav;

				}
			}
			UpdateNavigation();

			NewNav.selectOnUp = profileCreate[profileCreate.Count - 1];
			NewNav.selectOnDown = profileCreate[0];
			NewNav.selectOnRight = inputButton;
			NewNav.selectOnLeft = inputButton;

			createButton.navigation = NewNav;

		}
	}
}