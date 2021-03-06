/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Menu
{
    //public delegate void ItemAction(ItemData item);

    //public delegate void ActionInt(int id);

    public class MenuButtonListController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Title("UI")]
        [SerializeField]
        protected RectTransform listTransform;

        [SerializeField]
        protected MenuButtonList prefabItem;
        [SerializeField]
        protected List<MenuButtonList> listItem = new List<MenuButtonList>();
        public List<MenuButtonList> ListItem
        {
            get { return listItem; }
        }

        protected int indexSelection = 0;
        public int IndexSelection
        {
            get { return indexSelection; }
        }

        protected int listIndexCount = 0;



        [Title("Input")]
        [SerializeField]
        protected float stickThreshold = 0.8f;

        [SerializeField]
        protected int timeBeforeRepeat = 10;
        [SerializeField]
        protected int repeatInterval = 3;
        [SerializeField]
        protected int scrollSize = 3;


        protected float currentTimeBeforeRepeat = -1;
        protected float currentRepeatInterval = -1;
        protected int lastDirection = 0; // 2 c'est bas, 8 c'est haut (voir numpad)
        protected int indexLimit = 0;

        protected IEnumerator coroutineScroll = null;



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
        protected virtual void Start()
        {
            indexLimit = scrollSize;
            listIndexCount = listItem.Count;
        }

        public void DrawItemList(int i, Sprite icon, string text, string subText = "")
        {
            if (i >= listItem.Count)
                listItem.Add(Instantiate(prefabItem, listTransform));
            listItem[i].DrawButton(icon, text, subText);
            listItem[i].gameObject.SetActive(true);
            listIndexCount = listItem.Count;
        }

        public void SetItemCount(int count)
        {
            listIndexCount = count;
            for (int i = count; i < listItem.Count; i++)
            {
                listItem[i].gameObject.SetActive(false);
            }
        }


        public bool InputList(Input_Info input)
        {
            if (input.vertical > stickThreshold)
            {
                SelectUp();
                return true;
            }
            else if (input.vertical < -stickThreshold)
            {
                SelectDown();
                return true;
            }
            else if (Mathf.Abs(input.vertical) <= 0.2f)
            {
                StopRepeat();
                return false;
            }
            return false;
        }

        public bool InputListHorizontal(Input_Info input)
        {
            if (input.horizontal > stickThreshold)
            {
                SelectDown();
                return true;
            }
            else if (input.horizontal < -stickThreshold)
            {
                SelectUp();
                return true;
            }
            else if (Mathf.Abs(input.horizontal) <= 0.2f)
            {
                StopRepeat();
                return false;
            }
            return false;
        }

        public void SelectUp()
        {
            if (listIndexCount == 0)
            {
                return;
            }
            if (lastDirection != 8)
            {
                StopRepeat();
                lastDirection = 8;
            }

            if (CheckRepeat() == false)
                return;

            listItem[indexSelection].UnselectButton();
            indexSelection -= 1;
            if (indexSelection <= -1)
            {
                indexSelection = listIndexCount - 1;
            }
            listItem[indexSelection].SelectButton();
            MoveScrollRect();
        }



        public void SelectDown()
        {
            if (listIndexCount == 0)
            {
                return;
            }
            if (lastDirection != 2)
            {
                StopRepeat();
                lastDirection = 2;
            }
            if (CheckRepeat() == false)
                return;

            listItem[indexSelection].UnselectButton();
            indexSelection += 1;
            if (indexSelection >= listIndexCount)
            {
                indexSelection = 0;
            }
            listItem[indexSelection].SelectButton();
            MoveScrollRect();
        }



        public void SelectIndex(int id)
        {
            listItem[indexSelection].UnselectButton();
            indexSelection = id;
            listItem[indexSelection].SelectButton();
        }





        // Check si on peut repeter l'input
        protected bool CheckRepeat()
        {
            if (currentRepeatInterval == -100) // Nombre magique
            {
                if (currentTimeBeforeRepeat == -100) // Nombre magique
                {
                    currentTimeBeforeRepeat = timeBeforeRepeat * 0.016f; // (0.016f = 60 fps et opti de la division)
                    return true;
                }
                else if (currentTimeBeforeRepeat <= 0)
                {
                    currentRepeatInterval = repeatInterval * 0.016f;// (0.016f = 60 fps et opti de la division)
                }
                else
                {
                    currentTimeBeforeRepeat -= Time.deltaTime;
                }
            }
            else if (currentRepeatInterval <= 0)
            {
                currentRepeatInterval = repeatInterval * 0.016f;// (0.016f = 60 fps et opti de la division)
                return true;
            }
            else
            {
                currentRepeatInterval -= Time.deltaTime;
            }
            return false;
        }

        public void StopRepeat()
        {
            currentRepeatInterval = -100; // Nombre magique
            currentTimeBeforeRepeat = -100;// Nombre magique
        }


        // Gère si la liste est dans un scroll rect
        protected void MoveScrollRect()
        {
            if (listTransform == null)
            {
                //if (selectionTransform != null)
                 //   selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
                return;
            }
            if (indexSelection > indexLimit)
            {
                indexLimit = indexSelection;
                coroutineScroll = MoveScrollRectCoroutine();
                if (coroutineScroll != null)
                {
                    StopCoroutine(coroutineScroll);
                }
                StartCoroutine(coroutineScroll);
            }
            else if (indexSelection < indexLimit - scrollSize + 1)
            {
                indexLimit = indexSelection + scrollSize - 1;
                coroutineScroll = MoveScrollRectCoroutine();
                if (coroutineScroll != null)
                {
                    StopCoroutine(coroutineScroll);
                }
                StartCoroutine(coroutineScroll);
            }
            /*else
            {
                if (selectionTransform != null)
                    selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
            }*/

        }

        private IEnumerator MoveScrollRectCoroutine()
        {
            float t = 0f;
            float speed = 1 / 0.1f;
            int ratio = indexLimit - scrollSize;
            Vector2 destination = new Vector2(0, Mathf.Clamp(ratio * prefabItem.RectTransform.sizeDelta.y, 0, (listIndexCount - scrollSize) * prefabItem.RectTransform.sizeDelta.y));
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                listTransform.anchoredPosition = Vector2.Lerp(listTransform.anchoredPosition, destination, t);
                //selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
                yield return null;
            }
            listTransform.anchoredPosition = destination;
            //selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace