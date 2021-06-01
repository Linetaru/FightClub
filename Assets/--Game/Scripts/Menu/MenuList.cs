using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Au cas où on a besoin d'une classe parent
namespace Menu
{
    public class MenuList : MonoBehaviour, IControllable
    {
		[SerializeField]
		protected bool initializeOnStart = true;
		[SerializeField]
        protected MenuButtonListController listEntry;

		public event EventVoid OnStart;
		public event EventInt OnSelected;
		public event EventInt OnValidate;
		public event EventVoid OnEnd;

		private void Start()
		{
			if(initializeOnStart == true)
				InitializeMenu();
		}



		public virtual void UpdateControl(int id, Input_Info input)
		{
			if (listEntry.InputList(input) == true) // On s'est déplacé dans la liste
			{
				SelectEntry(listEntry.IndexSelection);
			}
			else if (input.inputUiAction == InputConst.Interact)
			{
				input.inputUiAction = null;
				ValidateEntry(listEntry.IndexSelection);
			}
			else if (input.inputUiAction == InputConst.Return)
			{
				QuitMenu();
			}
		}


		public virtual void InitializeMenu()
		{
			OnStart?.Invoke();
		}

		protected virtual void SelectEntry(int id)
		{
			OnSelected?.Invoke(id);
		}

		protected virtual void ValidateEntry(int id)
		{
			OnValidate?.Invoke(id);
		}

		protected virtual void QuitMenu()
		{
			OnEnd?.Invoke();
		}
	}
}
