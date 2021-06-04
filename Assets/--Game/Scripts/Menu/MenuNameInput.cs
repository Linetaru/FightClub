using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Menu
{
	public class MenuNameInput : SerializedMonoBehaviour, IControllable
	{
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        [TableMatrix]
        private string[,] characterTable = new string[18,4];


        [SerializeField]
        private RectTransform parentTransform;
        [SerializeField]
        private RectTransform selectionTransform;
        [SerializeField]
        private RectTransform buttonLetterPrefab;
        /*[SerializeField]
        private RectTransform[] characterPositions;*/

        [Space]
        [Space]
        [Space]
        [SerializeField]
        TMPro.TMP_InputField inputField;

        [Space]
        [Space]
        [Space]
        [Title("Inputs")]
        [SerializeField]
        protected float stickThreshold = 0.7f;
        [SerializeField]
        protected float timeBeforeRepeat = 10;
        [SerializeField]
        protected float repeatInterval = 3;




        private event UnityAction<string> OnValidate;

        protected float currentTimeBeforeRepeat = -1;
        protected float currentRepeatInterval = -1;
        protected int lastDirection = 0; // 2 c'est bas, 8 c'est haut (voir numpad)
        protected int indexLimit = 0;

        protected IEnumerator coroutineMove = null;
        protected int indexSelectedX = 0;
        protected int indexSelectedY = 0;


        private RectTransform[] characterPositions;
        //private TMPro.TextMeshProUGUI[] textMeshTable;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        private void Awake()
        {
            timeBeforeRepeat /= 60f;
            repeatInterval /= 60f;

            characterPositions = new RectTransform[characterTable.GetLength(0) * characterTable.GetLength(1)];
            for (int x = 0; x < characterTable.GetLength(1); x++)
            {
                for (int y = 0; y < characterTable.GetLength(0); y++)
                {
                    characterPositions[x * characterTable.GetLength(0) + y] = Instantiate(buttonLetterPrefab, parentTransform);
                    characterPositions[x * characterTable.GetLength(0) + y].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = characterTable[y, x];
                    characterPositions[x * characterTable.GetLength(0) + y].gameObject.SetActive(true);
                }
            }
            selectionTransform.SetAsLastSibling();
            StopRepeat();
        }



        public void RegisterPlayerName()
        {
            OnValidate?.Invoke(inputField.text);
        }



        public void UpdateControl(int id, Input_Info input)
        {
            if (input.horizontal > stickThreshold)
                SelectRight();
            else if (input.horizontal < -stickThreshold)
                SelectLeft();
            else if (input.vertical < -stickThreshold)
                SelectDown();
            else if (input.vertical > stickThreshold)
                SelectUp();
            else
                StopRepeat();

            if (input.inputUiAction == InputConst.Interact)
                Type();
            else if (input.inputUiAction == InputConst.Jump)
                TypeSpace();
            else if (input.inputUiAction == InputConst.Return)
                EraseText();
            else if (input.inputUiAction == InputConst.Pause)
                RegisterPlayerName();

            input.inputUiAction = null;
        }





        public void Type()
        {
            inputField.text += characterTable[indexSelectedX, indexSelectedY];
        }

        public void TypeSpace()
        {
            inputField.text += " ";
        }

        public void EraseText()
        {
            if (inputField.text.Length == 0)
                return;
            inputField.text = inputField.text.Remove(inputField.text.Length-1);
        }

        public void SelectUp()
        {
            if (CheckRepeat() == false)
                return;

            indexSelectedY -= 1;
            if (indexSelectedY <= -1)
            {
                indexSelectedY = characterTable.GetLength(1)-1;
            }
            MoveSelection();
        }

        public void SelectDown()
        {
            if (CheckRepeat() == false)
                return;

            indexSelectedY += 1;
            if (indexSelectedY >= characterTable.GetLength(1))
            {
                indexSelectedY = 0;
            }
            MoveSelection();
        }

        public void SelectLeft()
        {
            if (CheckRepeat() == false)
                return;

            indexSelectedX -= 1;
            if (indexSelectedX <= -1)
            {
                indexSelectedX = characterTable.GetLength(0)-1;
            }
            MoveSelection();
        }

        public void SelectRight()
        {
            if (CheckRepeat() == false)
                return;

            indexSelectedX += 1;
            if (indexSelectedX >= characterTable.GetLength(0))
            {
                indexSelectedX = 0;
            }
            MoveSelection();
        }

        private void MoveSelection()
        {
            if (coroutineMove != null)
                StopCoroutine(coroutineMove);
            coroutineMove = MoveSelectionCursorCoroutine();
            StartCoroutine(coroutineMove);

        }

        private IEnumerator MoveSelectionCursorCoroutine(int time = 5)
        {
            Vector2 speed = new Vector2((selectionTransform.anchoredPosition.x - characterPositions[indexSelectedX + (characterTable.GetLength(0) * indexSelectedY)].anchoredPosition.x) / time,
                                        (selectionTransform.anchoredPosition.y - characterPositions[indexSelectedX + (characterTable.GetLength(0) * indexSelectedY)].anchoredPosition.y) / time);
            while (time != 0)
            {
                selectionTransform.anchoredPosition -= speed;
                time -= 1;
                yield return null;
            }
            coroutineMove = null;
        }


        // Check si on peut repeter l'input
        private bool CheckRepeat()
        {
            if (currentTimeBeforeRepeat == timeBeforeRepeat)
            {
                currentTimeBeforeRepeat -= Time.deltaTime;
                return true;
            }


            if (currentTimeBeforeRepeat > 0)
            {
                currentTimeBeforeRepeat -= Time.deltaTime;
            }
            else if (currentRepeatInterval > 0)
            {
                currentRepeatInterval -= Time.deltaTime;
            }
            else
            {
                currentRepeatInterval = repeatInterval;
                return true;
            }
            return false;


            /*if (currentRepeatInterval <= -1)
            {
                if (currentTimeBeforeRepeat <= -1)
                {
                    currentTimeBeforeRepeat = timeBeforeRepeat;
                    return true;
                }
                else if (currentTimeBeforeRepeat == 0)
                {
                    currentRepeatInterval = repeatInterval;
                }
                else
                {
                    currentTimeBeforeRepeat -= Time.deltaTime;
                }
            }
            else if (currentRepeatInterval < 0)
            {
                currentRepeatInterval = repeatInterval;
                return true;
            }
            else
            {
                currentRepeatInterval -= Time.deltaTime;
            }
            return false;*/
        }

        public void StopRepeat()
        {
            currentRepeatInterval = repeatInterval;
            currentTimeBeforeRepeat = timeBeforeRepeat;
        }





        #endregion

    } // MenuNameInput class
	
}// #PROJECTNAME# namespace
