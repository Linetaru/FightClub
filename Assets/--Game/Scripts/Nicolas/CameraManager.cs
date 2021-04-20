using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

//State machine for camera manager
public enum StateCamera{
	InFocusMode,
	InMovingMode,
}

[System.Serializable]
public class Cam_Infos
{
	[Title("Config")]
	//Config Object for changing Camera parameter
	[Expandable]
	public CameraConfig cameraConfig;

	[Title("Rails Array")]
	//All array for scrolling movement get A and B point for each rails
	public GameObject[] railsCamTravelling = new GameObject[2];
	public GameObject[] railsFocusTravelling = new GameObject[2];
	public GameObject[] railsBlastZoneTravelling = new GameObject[2];

	[Title("Parameter Timer")]
	//Float for timer before scrolling start
	public float timeBeforeMoving;

	//Time before camera reaching Last point of this travelling rails
	public float durationTravelling;

	[Title("Parameter Bool")]
	//Boolean to know if this travelling will move on axis Y or not
	public bool movingInY;

	[Title("Object Reference")]
	//GameObject Panel of Arrows scrolling information
	public GameObject canvasPanelArrowToActivate;

	//List GameObject who will be disable on focus mode
	public List<GameObject> objectDisableOnFocusMode;

	//List of platform 
	public List<GameObject> platform;

	//Function boolean to check if camera position has reach the last Point (Function to clear some condition in other class)
	public bool isNotAtLimit(Transform c)
	{
		//Check if bool to know if we moving in axis Y
		if (movingInY)
		{
			//Check if position in axis Y is different form last rails position and return true or false
			if (c.position.y != railsCamTravelling[1].transform.position.y)
				return true;
			else
				return false;
		}
		else
		{
			//Check if position in axis X is different form last rails position and return true or false
			if (c.position.x != railsCamTravelling[1].transform.position.x)
				return true;
			else
				return false;
		}
	}

	//Change Visibility of all Gameobject in a array
	public void ChangeToVisible(bool value)
	{
		if(objectDisableOnFocusMode.Count > 0)
			foreach (GameObject go in objectDisableOnFocusMode)
				go.SetActive(value);
	}
}

public class CameraManager : MonoBehaviour
{
	//State machine camera with enumerator
	[ReadOnly] public StateCamera stateCamera = StateCamera.InFocusMode;

	[Title("Reference Object")]
	//Script Camera for Zoom between Player in Focus Mode
	public CameraZoomController zoomController;

	//Get reference at blastZone object
	public GameObject blastZone;

	public List<UnityEngine.UI.Image> imagesUiRedScrolling;

	[Range(0.5f, 5)]
	public float timeRedBorder = 0.8f;
	[Range(1, 10)]
	public int loopRedBorder = 2;

	[Title("Configs Path Scrolling")]
	//Array for all config on the level
	public Cam_Infos[] cam_Infos;

	//Boolean to know if we loop scrolling movement at last config
	public bool loopScrolling;

	public bool canScroll = true;

	public List<GameObject> disableGameObjectOnStart;

	[Title("Parametre in Visible to Debug")]
	//Initialize timer for changing state
	[ReadOnly] public float timer;

	//Initialize posiion ID to know which config we are on
	[ReadOnly] public int positionID = 0;

	//Initialize timer for Lerp Vector 3 position
	private float timeLerp;

	//Boolean for condition to know if we already change focus of camera
	private bool isFocusChanged;

	//Store position taken from this object at a moment we want take it
	private Vector3 positionTaken;

	// Start function add timer a value from the first config if array of all config is different from zero
	void Start()
	{
		if (cam_Infos.Length != 0)
		{
			timer = cam_Infos[0].timeBeforeMoving;
			cam_Infos[0].ChangeToVisible(false);
		}

		if (disableGameObjectOnStart.Count > 0)
			foreach (GameObject go in disableGameObjectOnStart)
				go.SetActive(false);
			
	}

	//Update function to apply scrolling and make timer updated
	void Update()
	{
		if(!canScroll) { return; }

        switch (stateCamera)
        {
			case StateCamera.InFocusMode:
				if (timer > 0)
				{
					timer -= Time.deltaTime;

					//Check if timer in at or under 35 percent of starting timer value
					if (timer / cam_Infos[positionID].timeBeforeMoving * 100 <= 35)
					{
						
						if (!isFocusChanged)
						{
							//Change focus of camera zoom controller
							zoomController.ChangeFocusState();
							//Change bool of this condition to don't play again
							isFocusChanged = true;
							//Store Position of this at this moment
							positionTaken = transform.position;
						}

						//Change canvas visibility of arrow alert
						cam_Infos[positionID].canvasPanelArrowToActivate.SetActive(!cam_Infos[positionID].canvasPanelArrowToActivate.activeSelf);

						//Update this position to be on same position at first point on camera rail array using actual timer to lerp on the position.
						Vector3 newPos = transform.position;
						newPos = Vector3.Lerp(cam_Infos[positionID].railsCamTravelling[0].transform.position, positionTaken, timer);
						newPos.z = transform.position.z;
						transform.position = newPos;
					}
				}
				else if (timer <= 0)
				{
					//Check if we have a new config after this one to update parameter
					if (cam_Infos.Length > positionID)
					{
						//cam_Infos[positionID].railsCamTravelling[0].transform.position = transform.position;

						//Change state of state machine
						stateCamera = StateCamera.InMovingMode;
						//Change canvas visibility of arrow alert
						cam_Infos[positionID].canvasPanelArrowToActivate.SetActive(false);
						//Change bool of this condition for the next config update
						isFocusChanged = false;
						
						if(positionID == 0)
                        {
							if (disableGameObjectOnStart.Count > 0)
							{
								foreach (GameObject go in disableGameObjectOnStart)
								{
									go.SetActive(true);
								}
								cam_Infos[positionID].ChangeToVisible(true);
							}
						}
						else
							cam_Infos[positionID - 1].ChangeToVisible(true);

						imagesUiRedScrolling[0].transform.parent.gameObject.SetActive(true);

						foreach (UnityEngine.UI.Image image in imagesUiRedScrolling)
                        {
							image.DOColor(new Color(1, 0, 0, 0), timeRedBorder).SetLoops(loopRedBorder).OnComplete(() => imagesUiRedScrolling[0].transform.parent.gameObject.SetActive(false));
                        }
					}
				}
					break;
			case StateCamera.InMovingMode:

				//Check if position of this is at limit of the rail position
				if (cam_Infos[positionID].isNotAtLimit(transform))
				{

					//Update this position to be on same position at last point on camera rail array using a timer to lerp on the position divide by travelling time of the config.
					Vector3 newPos = transform.position;
					newPos = Vector3.Lerp(cam_Infos[positionID].railsCamTravelling[0].transform.position, cam_Infos[positionID].railsCamTravelling[1].transform.position, timeLerp / cam_Infos[positionID].durationTravelling);
					newPos.z = transform.position.z;
					transform.position = newPos;

					//Update position of the gameObject Focus 
					//zoomController.focusLevel.transform.position = Vector3.Lerp(cam_Infos[positionID].railsFocusTravelling[0].transform.position, cam_Infos[positionID].railsFocusTravelling[1].transform.position, timeLerp / cam_Infos[positionID].durationTravelling);
					
					//Update position of the blastzone to kill player when they are to slow on scrolling mode
					BlastZoneManager.Instance.transform.position = Vector3.Lerp(cam_Infos[positionID].railsBlastZoneTravelling[0].transform.position, cam_Infos[positionID].railsBlastZoneTravelling[1].transform.position, timeLerp / cam_Infos[positionID].durationTravelling);
					
					timeLerp += Time.deltaTime;
				}
				else
				{
                    //Change zoom camera parameter to value of new config 
                    zoomController.ChangeValueFocus(cam_Infos[positionID].cameraConfig);

					cam_Infos[positionID].ChangeToVisible(false);

					//Change position ID for change config
					positionID++;
					//Change state of state machine
					stateCamera = StateCamera.InFocusMode;
					//Change focus of camera zoom controller
					zoomController.ChangeFocusState();
					//Check if it has more config over this config 
					if (cam_Infos.Length >= positionID + 1)
					{
						timer = cam_Infos[positionID].timeBeforeMoving;
					}
					//Reset timer for the next config
					timeLerp = 0;

					if (loopScrolling && cam_Infos.Length == positionID)
					{
						positionID = 0;
						timer = cam_Infos[positionID].timeBeforeMoving;
					}

					foreach (UnityEngine.UI.Image image in imagesUiRedScrolling)
					{
						image.color = new Color(1,0,0,1);
					}
				}
                break;
        }
    }
}