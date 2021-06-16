using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ConfigMode", menuName = "Data/ConfigMode", order = 2)]
public class ConfigModeScriptable : ScriptableObject
{
	[Range(1, 99)]
	public int numberOfLife = 3;
	[Range(1, 99)]
	public int numberOfGoal = 5;
	[RangeEx(100, 10000, 100)]
	public int numberOfGrandSlamPoint = 400;
	[Range(1, 99)]
	public int numberOfGrandSlamBonus = 3;
}