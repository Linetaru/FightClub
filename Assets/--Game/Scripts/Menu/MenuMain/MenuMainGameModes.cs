using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
	public class MenuMainGameModes : MenuList
	{
		[SerializeField]
		Transform arrow;
		[SerializeField]
		Transform[] buttonsPosition;




		protected override void SelectEntry(int id)
		{
			base.SelectEntry(id);
			arrow.position = buttonsPosition[id].transform.position;
		}
		protected override void ValidateEntry(int id)
		{
			base.SelectEntry(id);
			arrow.position = buttonsPosition[id].transform.position;
		}
	}
}
