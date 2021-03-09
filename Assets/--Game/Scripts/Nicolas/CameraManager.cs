using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateCamera{
	InFocusMode,
	InMovingMode,
}

[System.Serializable]
public class Cam_Infos
{
	public CameraConfig cameraConfig;

	public float timeBeforeMoving;

	public GameObject[] railsCamTravelling;
	public GameObject[] railsFocusTravelling;
	public GameObject[] railsBlastZoneTravelling;

	public float durationTravelling;

	public bool movingInY;

	public bool isAtLimit(Transform c)
	{
		if (movingInY)
		{
			if (c.position.y != railsCamTravelling[1].transform.position.y)
				return true;
			else
				return false;
		}
		else
        {
			if (c.position.x != railsCamTravelling[1].transform.position.x)
				return true;
			else
				return false;
		}
	}
}

public class CameraManager : MonoBehaviour
{
	[ReadOnly] public StateCamera stateCamera = StateCamera.InFocusMode;

	public CameraZoomController zoomController;

	public Cam_Infos[] cam_Infos;

	public GameObject blastZone;

	[ReadOnly] public float timer;

	[ReadOnly] public int positionID = 0;

	private float timeLerp;

	// Start is called before the first frame update
	void Start()
	{
		if (cam_Infos.Length != 0)
			timer = cam_Infos[0].timeBeforeMoving;
	}

	// Update is called once per frame
	void Update()
	{
        switch (stateCamera)
        {
            case StateCamera.InFocusMode:
				if (timer > 0)
				{
					timer -= Time.deltaTime;
				}
				else if (timer <= 0)
				{
					if (cam_Infos.Length > positionID)
					{
						stateCamera = StateCamera.InMovingMode;
						zoomController.ChangeFocusState();
					}
				}
					break;
            case StateCamera.InMovingMode:
				if (cam_Infos[positionID].isAtLimit(transform))
				{
					transform.position = Vector3.Lerp(cam_Infos[positionID].railsCamTravelling[0].transform.position, cam_Infos[positionID].railsCamTravelling[1].transform.position, timeLerp / cam_Infos[positionID].durationTravelling);
					zoomController.focusLevel.transform.position = Vector3.Lerp(cam_Infos[positionID].railsFocusTravelling[0].transform.position, cam_Infos[positionID].railsFocusTravelling[1].transform.position, timeLerp / cam_Infos[positionID].durationTravelling);
					blastZone.transform.position = Vector3.Lerp(cam_Infos[positionID].railsBlastZoneTravelling[0].transform.position, cam_Infos[positionID].railsBlastZoneTravelling[1].transform.position, timeLerp / cam_Infos[positionID].durationTravelling);
					timeLerp += Time.deltaTime;
				}
				else
				{
					zoomController.ChangeValueFocus(cam_Infos[positionID].cameraConfig);
					positionID++;
					stateCamera = StateCamera.InFocusMode;
					zoomController.ChangeFocusState();
					if (cam_Infos.Length >= positionID + 1)
					{
						timer = cam_Infos[positionID].timeBeforeMoving;
					}
					timeLerp = 0;
				}
                break;
        }
    }
}