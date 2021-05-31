using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

public class AnimatorToolBox : EditorWindow
{
    //Class DataAnimation to stock a animator with his animation selected
    public class DataAnimation
    {
        public Animator animator;
        public AnimationClip animationClip;

        public DataAnimation(Animator animValue, AnimationClip animClipValue)
        {
            animator = animValue;
            animationClip = animClipValue;
        }
    }

    //Variable DataAnimation
    private List<DataAnimation> dataAnimations = new List<DataAnimation>();
    private ReorderableList listDataAnimations;

    //Variable Animator
    private List<Animator> animatorsInScene = new List<Animator>();
    private string searchStringAnimator = "";
    private bool isAnimatorSelected = false;
    private ReorderableList listAnimator;
    private int indexAnimator;

    //Variable Animation
    private List<AnimationClip> animationInAnimator = new List<AnimationClip>();
    private string searchStringAnimation = "";
    private bool isAnimationSelected = false;
    private ReorderableList listAnimation;
    private int indexAnimation;

    //Time Editor To Play animation
    private float lastEditorTime = 0f;

    //Boolean To know when animation is Played to Lock GUI
    private bool isPlayingAnim = false;
    private bool isPlayingMultipleAnim = false;

    private enum PlayingAnimState{
        SingleAnim,
        MultipleAnim,
    }

    private PlayingAnimState animState = PlayingAnimState.SingleAnim;

    //Options var for Animation in option toolbar
    private bool needUseSlider = false;
    private bool canStopAnimFromSlider = false;
    private int timeForAnimation = 1;
    private float animTime = 0f;
    private int biggestLenght = 10;

    private bool loopingAnim = false;

    private Texture2D centerButtonTexture;
    public Texture2D pauseTexture;
    public Texture2D playTexture;
    public Texture2D fasterTexture;
    public Texture2D fasterRewindTexture;
    public Texture2D slowerTexture;
    public Texture2D slowerRewindTexture;

    private string centerButtonTextHover;

    private float timePause = 0f;
    private int timeScale = 1;
    private bool pause = false;
    private bool rewindLeft = false;
    private bool rewindRight = false;

    private string[] gridOptionsSlide = { "Use Slider", "Disable Slide" };
    private int intGridOptionsSlide = 1;

    private string[] gridOptionsBoucle = { "Loop Animation", "Disable Loop" };
    private int intGridOptionsBoucle = 1;

    private string[] prefabButton = { "Use Scene Search", "Use Prefab Search" };
    private int intprefabButton;
    private bool callingOnce;
    public string headerlabelText = "Get all animator in this scene and see all animation !";
    public string buttonlabelText = "Get all Animator in Scene ";

    //Variable Vector for Scrolling
    private Vector2 animatorScrollPos;
    private Vector2 animationScrollPos;
    private Vector2 dataAnimationScrollPos;

    //Enum for ToolBar
    public enum StateToolBar
    {
        AnimatorList = 0,
        SelectedObject = 1,
        OptionsAnimations = 2,
        PrefabUtility = 3,
    }

    //Enum var for Toolbar GUI to cast in int
    public StateToolBar toolbarInt;

    //String Array for Toolbar GUI, adding more string will pop more toolbar
    public string[] stringToolBar = { StateToolBar.AnimatorList.ToString(), StateToolBar.SelectedObject.ToString(), StateToolBar.OptionsAnimations.ToString(), StateToolBar.PrefabUtility.ToString() };

    //Menu Item for launch this window
    [MenuItem("Toolbox/Toolbox Animator")]
    static void InitWindow()
    {
        AnimatorToolBox window = GetWindow<AnimatorToolBox>();
        window.Show();
        window.titleContent = new GUIContent("Animator Toolbox");
    }

    //Init all List and some delegate to update list at some moment
    private void OnEnable()
    {
        centerButtonTexture = pauseTexture;
        centerButtonTextHover = "Pause";

        //Each Delegate will call funtion to change list needed to be reload
        EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        EditorSceneManager.sceneClosing += UnloadedScene;
        EditorSceneManager.sceneOpened += OnOpenScene;

        //Init List Animator and List with Data
        ListAnimatorInit();
        ListDataAnimationInit();
    }

    //Method called on each time an assets is modified in the project
    private void OnProjectChange()
    {
        //Update list to adapt from project change
        UpdateOnChangeAction();
    }

    //Method called on each time an gameObject is modified in the scene
    private void OnHierarchyChange()
    {
        //Update list to adapt from hierarchy change
        UpdateOnChangeAction();
    }

    //Update list to adapt from change
    public void UpdateOnChangeAction()
    {
        List<Animator> animators = animatorsInScene;
        //Update List Animator and reset it with new Object modified if needed
        UpdateAnimatorList();
        //Reset Animation List
        animationInAnimator.Clear();
        ListAnimationInit();

        ReorderingDataAnimation();

        if (animators != animatorsInScene)
            ListDataAnimationInit();

        StopAnimation();

        //Reset Data for adding or removing all change fom other list
        //if (animators != animatorsInScene)
        //    ListDataAnimationInit();
    }

    //Called By Delegate when we Open a new scene, it wil update all Animator list and update Data list
    private void OnOpenScene(Scene scene, OpenSceneMode mode)
    {
        UpdateAnimatorList();
        ListDataAnimationInit();
    }

    //Called by Delegate to release all list and data for changing to a new scene
    private void UnloadedScene(Scene scene, bool removingScene)
    {
        //Stop all animation who was on Play
        StopAnimation();

        //Unselect list
        isAnimatorSelected = false;
        isAnimationSelected = false;

        //Unload delegate
        listAnimator.onSelectCallback -= DisplayAnimator;

        //Unload delegate if list Animation was create
        if (listAnimation != null)
            listAnimation.onSelectCallback -= DisplayAnimation;

        //Clear List to clean editor list
        animatorsInScene.Clear();
        animationInAnimator.Clear();
        dataAnimations.Clear();

        //Clean editor list with the new fresh list
        listAnimator = new ReorderableList(animatorsInScene, typeof(Animator), false, true, false, false);
        listAnimation = new ReorderableList(animationInAnimator, typeof(AnimationClip), false, true, false, false);
        listDataAnimations = new ReorderableList(dataAnimations, typeof(DataAnimation), false, true, false, true);
    }

    //Init Animator Editor List and call delegate we need for draw Reorderable list
    public void ListAnimatorInit()
    {
        //Reset list to update it
        listAnimator = new ReorderableList(animatorsInScene, typeof(Animator), false, true, false, false);

        //Link Function to be called on each select animator in the list
        listAnimator.onSelectCallback += DisplayAnimator;

        //Callback to override Header of the Animator list
        listAnimator.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "All Animator List");
        };

        //Callback to override each time it draw a Element of the Animator list
        listAnimator.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            ToolBoxUtils.DrawElement(rect, animatorsInScene[index].name + " (Animator Controller)", new GUIStyle() { fontStyle = FontStyle.Italic, normal = new GUIStyleState() { textColor = Color.white } });
        };

        //Set Padding at bottom to avoid a large space
        listAnimator.footerHeight = 0;
    }

    //Init Data Editor List and call delegate we need for draw Reorderable list
    public void ListDataAnimationInit()
    {
        //Reset list to update it
        listDataAnimations = new ReorderableList(dataAnimations, typeof(DataAnimation), false, true, false, true);

        //Link Function to be called on each data removed element form the list
        listDataAnimations.onRemoveCallback += RemoveDataAnimation;

        //Callback to override Header of the data list
        listDataAnimations.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Animator with Animation Selecter");
        };


        //Callback to override each time it draw a Element of the Data list to draw Label of each data information 
        listDataAnimations.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Italic;
            style.normal.textColor = Color.white;

            if (index < dataAnimations.Count)
            {
                if (dataAnimations[index] != null)
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), dataAnimations[index].animator.name);
                    EditorGUI.LabelField(new Rect(rect.x + 100, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), dataAnimations[index].animationClip.name, style);
                }
            }
        };

        //Set Padding at bottom to avoid a large space
        listDataAnimations.footerHeight = 0;
    }

    //Init Animation Editor List and call delegate we need for draw Reorderable list
    public void ListAnimationInit()
    {

        //Reset list to update it
        listAnimation = new ReorderableList(animationInAnimator, typeof(AnimationClip), false, true, false, false);

        //Link Function to be called on each selected animation
        listAnimation.onSelectCallback += DisplayAnimation;

        //Callback to override Header of the animation list
        listAnimation.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "All Animation List");
        };

        //Callback to override each time it draw a Element of the Animation list into animation name field
        listAnimation.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            ToolBoxUtils.DrawElement(rect, animationInAnimator[index].name + " (Animation)", new GUIStyle() { fontStyle = FontStyle.Italic, normal = new GUIStyleState() { textColor = Color.white } });
        };

        //Set Padding at bottom to avoid a large space
        listAnimator.footerHeight = 0;
    }


    //Callback to remove an data form the list of data when remove button are clicked
    private void RemoveDataAnimation(ReorderableList list)
    {
        dataAnimations.Remove(dataAnimations[list.index]);
    }

    //Unlink delegate and reset list when this window is closed
    private void OnDisable()
    {
        UnloadedScene(SceneManager.GetActiveScene(), true);
        EditorSceneManager.sceneClosing -= UnloadedScene;
        EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
    }

    //Delegate to Stop all animation when we click on Play button to play in game viewport
    private void OnPlayModeStateChange(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
            StopAnimation();
    }

    //Callback called when we click on an Animator in the list, will give index of animator we clicked on and select this object into the scene
    private void DisplayAnimator(ReorderableList list)
    {
        indexAnimator = list.index;
        Selection.activeObject = animatorsInScene[indexAnimator];
        EditorGUIUtility.PingObject(animatorsInScene[indexAnimator]);

        isAnimatorSelected = true;

        //Will Show and init Animation list by referencing all animation this animator get.
        UpdateAnimationList();
    }

    //Callback called when we click on an Animation in the list, will give index of the animation we clicked on and prepare it to be play
    private void DisplayAnimation(ReorderableList list)
    {
        indexAnimation = list.index;
        isAnimationSelected = true;

        //Create new Data to add on the list
        DataAnimation dataAnimationCheck = new DataAnimation(animatorsInScene[indexAnimator], animationInAnimator[indexAnimation]);

        biggestLenght = (int)(animationInAnimator[indexAnimation].length * 60);

        //Replace Data if we already stock same animator in it else it will add it on data list
        if (dataAnimations.Count > 0)
            for (int i = 0; i < dataAnimations.Count; i++)
            {
                if (dataAnimations[i].animator == animatorsInScene[indexAnimator])
                {
                    dataAnimations[i] = dataAnimationCheck;
                    return;
                }
            }

        dataAnimations.Add(dataAnimationCheck);
    }

    //Method To draw on our window
    private void OnGUI()
    {
        //Toolbar for separate visual
        //toolbarInt = (StateToolBar)GUILayout.Toolbar((int)toolbarInt, stringToolBar);
        toolbarInt = (StateToolBar)GUILayout.SelectionGrid((int)toolbarInt, new GUIContent[] { new GUIContent(stringToolBar[0], "Getting List of Animator and Animation"), new GUIContent(stringToolBar[1], "Show list of selected animation"), new GUIContent(stringToolBar[2], "All Option to manipulate animations"), new GUIContent(stringToolBar[3], "Change Getter Animator between Scene and Prefab") }, 2);

        ToolBoxUtils.HorizontalLine(new Vector2(10f, 5f));

        //Disable Group on Playing Animation
        EditorGUI.BeginDisabledGroup(isPlayingMultipleAnim || Application.isPlaying || isPlayingAnim);
        EditorGUILayout.BeginHorizontal();

        //Header Text of the window
        GUILayout.Label("- Animator Toolbox -");

        //Button to clear data list
        if (GUILayout.Button(new GUIContent("Unselect Animation", "Clear All Selected Animation")))
        {
            dataAnimations.Clear();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();

        ToolBoxUtils.HorizontalLine(new Vector2(10f, 5f));

        //Switch to draw some GUI on toolbar choosen
        switch (toolbarInt)
        {
            //Draw List of Animator and list of Animation to be select and playable
            case StateToolBar.AnimatorList:

                EditorGUI.BeginDisabledGroup(isPlayingMultipleAnim || Application.isPlaying || isPlayingAnim);

                //SearchBar for Animator List
                string lastStringAnimator = searchStringAnimator;
                ToolBoxUtils.SearchArea(ref searchStringAnimator);
                //Update Animator List if search bar text as been update
                if (lastStringAnimator != searchStringAnimator)
                    FindAnimatorsInScene();

                ToolBoxUtils.GUIAdding(ToolBoxUtils.GUIAddingName.Space, 5);

                GUILayout.Label(headerlabelText);

                if (GUILayout.Button(new GUIContent(buttonlabelText, "Search and List All GameObject Enable with an Animator")))
                {
                    FindAnimatorsInScene();
                }

                ToolBoxUtils.GUIAdding(ToolBoxUtils.GUIAddingName.Space, 5);

                EditorGUILayout.BeginHorizontal();

                //Scrolling Bar for Animator list when we got to many animator
                animatorScrollPos = EditorGUILayout.BeginScrollView(animatorScrollPos, GUILayout.Height(100));

                //Draw Animator Reordorable list
                listAnimator.DoLayoutList();

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();

                EditorGUI.EndDisabledGroup();

                ToolBoxUtils.GUIAdding(ToolBoxUtils.GUIAddingName.Space, 10);

                //Add Animation Playable button when we have at least 1 animation select 
                if (isAnimationSelected && dataAnimations.Count > 0)
                {
                    GUIButtonAnimation(true);

                    ToolBoxUtils.GUIAdding(ToolBoxUtils.GUIAddingName.Space, 5);
                }

                EditorGUI.BeginDisabledGroup(isPlayingMultipleAnim || Application.isPlaying || isPlayingAnim);

                //Draw List of Animation when a Animator is Selected
                if (isAnimatorSelected)
                {
                    //SearchBar for Animator List
                    string lastString = searchStringAnimation;
                    ToolBoxUtils.SearchArea(ref searchStringAnimation);

                    //Update Animation List if search bar text as been update
                    if (lastString != searchStringAnimation)
                        FindAnimationInAnimator();

                    EditorGUILayout.BeginHorizontal();

                    //Scrolling Bar for animation list
                    animationScrollPos = EditorGUILayout.BeginScrollView(animationScrollPos);

                    //Draw Animation Reordorable list
                    listAnimation.DoLayoutList();

                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.EndDisabledGroup();
                break;

            //Draw List of all Animator with Animation selected in data list
            case StateToolBar.SelectedObject:

                //Add Animation Playable button when we have at least 1 animation select 
                if (isAnimationSelected && dataAnimations.Count > 0)
                {
                    GUIButtonAnimation(false);
                }

                EditorGUI.BeginDisabledGroup(isPlayingMultipleAnim || Application.isPlaying || isPlayingAnim);

                EditorGUILayout.BeginHorizontal();

                //Scrolling Bar for data list
                dataAnimationScrollPos = EditorGUILayout.BeginScrollView(dataAnimationScrollPos);

                //Draw Data Reordorable list
                listDataAnimations.DoLayoutList();

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();

                EditorGUI.EndDisabledGroup();
                break;

            case StateToolBar.OptionsAnimations:

                //Grid to make selection button for using Slider Frame
                intGridOptionsSlide = GUILayout.SelectionGrid(intGridOptionsSlide, new GUIContent[] { new GUIContent(gridOptionsSlide[0], "Enable Slider Frame"), new GUIContent(gridOptionsSlide[1], "Disable Slider Frame") }, 2);

                needUseSlider = ToolBoxUtils.IntToBoolean(intGridOptionsSlide);

                if (needUseSlider && pause)
                {
                    centerButtonTexture = pauseTexture;
                    centerButtonTextHover = "Pause";
                    pause = false;
                }

                EditorGUI.BeginDisabledGroup(needUseSlider);
                //Grid to make selection button for looping Animation
                intGridOptionsBoucle = GUILayout.SelectionGrid(intGridOptionsBoucle, new GUIContent[] { new GUIContent(gridOptionsBoucle[0], "Enable Loop Animation"), new GUIContent(gridOptionsBoucle[1], "Disable Loop Animation") }, 2);
                EditorGUI.EndDisabledGroup();

                loopingAnim = ToolBoxUtils.IntToBoolean(intGridOptionsBoucle);

                ToolBoxUtils.HorizontalLine(new Vector2(10f, 5f));

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(Screen.width / 2 - 130);

                EditorGUI.BeginDisabledGroup(!pause || needUseSlider);

                if (GUILayout.Button(new GUIContent(slowerTexture, "Rewind"), GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    // Improved System of rewind Helping by Nam
                    rewindLeft = true;
                    lastEditorTime = Time.realtimeSinceStartup;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(needUseSlider);

                if (GUILayout.Button(new GUIContent(slowerRewindTexture, "Slower"), GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    timeScale -= 1;
                }

                if (GUILayout.Button(new GUIContent(centerButtonTexture, centerButtonTextHover), GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    pause = !pause;
                    ChangeTextureBetweenPlayAndPause();
                    if (pause)
                        timePause = animTime;
                    else
                        lastEditorTime = Time.realtimeSinceStartup;
                }

                if (GUILayout.Button(new GUIContent(fasterRewindTexture, "Faster"), GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    timeScale += 1;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(!pause || needUseSlider);

                if (GUILayout.Button(new GUIContent(fasterTexture, "Fast forward"), GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    // Improved System of rewind Helping by Nam
                    rewindRight = true;
                    lastEditorTime = Time.realtimeSinceStartup;
                }
                EditorGUI.EndDisabledGroup();

                timeScale = Mathf.Clamp(timeScale, 1, 10);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Space(Screen.width / 2 - 50);
                EditorGUILayout.LabelField("Time Scale : " + timeScale);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.HelpBox("Rewind/Fast Forward Button will move animation frame by frame when you are in Pause Mode", MessageType.Info);
                EditorGUI.EndDisabledGroup();

                ToolBoxUtils.HorizontalLine(new Vector2(10f, 5f));

                //Add Animation Playable button when we have at least 1 animation select 
                if (dataAnimations.Count > 0)
                {
                    GUIButtonAnimation(false);
                }

                EditorGUI.BeginDisabledGroup(!needUseSlider);
                EditorGUILayout.LabelField("Animation Time (In frame)");

                //Slider to manipulation current animation played by Frame
                timeForAnimation = EditorGUILayout.IntSlider(timeForAnimation, 0, biggestLenght);
                EditorGUI.EndDisabledGroup();
                break;

            case StateToolBar.PrefabUtility:
                intprefabButton = GUILayout.SelectionGrid(intprefabButton, prefabButton, 2);

                //Change Selection Mode between Selection in Scene or Selection in Prefab
                if (!ToolBoxUtils.IntToBoolean(intprefabButton) && !callingOnce)
                {
                    UpdatingForMode(true, "Get all animator in this prefab and see all animation !", "Get all Animator in Prefab ");
                }
                else if (callingOnce && ToolBoxUtils.IntToBoolean(intprefabButton))
                {
                    UpdatingForMode(false, "Get all animator in this scene and see all animation !", "Get all Animator in Scene ");
                }

                ToolBoxUtils.HorizontalLine(new Vector2(10f, 5f));

                EditorGUILayout.HelpBox( "Using Scene Search will give you ability to take all animator in the current open scene ! \n \n" +
                    "Using Prefab Search will give you ability to take all animator in the current open prefab ! \n(If u don't have an open prefab it will return 0 animator)", MessageType.Info);
                break;
        }
    }

    private void GUIButtonAnimation(bool useSinglePlay)
    {
        EditorGUILayout.BeginHorizontal();

        if (useSinglePlay)
        {
            EditorGUI.BeginDisabledGroup(isPlayingMultipleAnim || Application.isPlaying || isPlayingAnim);

            if (GUILayout.Button("Play"))
            {
                animState = PlayingAnimState.SingleAnim;
                StartAnimation();
            }

            EditorGUI.EndDisabledGroup();
        }

        EditorGUI.BeginDisabledGroup(isPlayingMultipleAnim || Application.isPlaying || isPlayingAnim);

        if (GUILayout.Button("Play All Anim"))
        {
            animState = PlayingAnimState.MultipleAnim;
            StartAnimation();
        }

        EditorGUI.EndDisabledGroup();


        if (GUILayout.Button("Stop"))
        {
            StopAnimation();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void UpdatingForMode(bool b_value, string h_value, string bt_value)
    {
        animatorsInScene.Clear();
        ListAnimatorInit();
        animationInAnimator.Clear();
        ListAnimationInit();
        ReorderingDataAnimation();
        ListDataAnimationInit();
        callingOnce = b_value;
        headerlabelText = h_value;
        buttonlabelText = bt_value;
    }

    //Callback Update to start and play with editor delta time animation clip selected, if delta time bigger than animation length clip it will be stopped
    private void OnEditorUpdate()
    {
        switch (animState)
        {
            case PlayingAnimState.SingleAnim:

                AnimationClip animationClip = animationInAnimator[indexAnimation];
                if (animationClip == null) { return; }

                StoppingAnimationFromSlider(animationClip);

                Rewind(animationClip.length, animationClip);

                if (pause && !rewindRight && !rewindRight) { return; }

                LoopingAnimMethod(animationClip.length);

                TimeManagement();

                PlaySingleAnimation(animationClip, animTime);
                break;

            case PlayingAnimState.MultipleAnim:

                List<DataAnimation> tmp_DataAnimations = new List<DataAnimation>();
                float biggestLengthAnim = 0;
                foreach (DataAnimation dA in dataAnimations)
                {
                    tmp_DataAnimations.Add(dA);
                    if (biggestLengthAnim < dA.animationClip.length)
                        biggestLengthAnim = dA.animationClip.length;
                }

                Rewind(biggestLengthAnim, tmp_DataAnimations);

                StoppingAnimationFromSlider(tmp_DataAnimations);

                if (pause && !rewindRight && !rewindRight) { return; }

                LoopingAnimMethod(biggestLengthAnim);

                TimeManagement(biggestLengthAnim);

                PlayingAllAnimation(biggestLengthAnim, animTime, tmp_DataAnimations);
                break;
        }
    }

    private void StoppingAnimationFromSlider(List<DataAnimation> tmp_DataAnimations, AnimationClip animationClip)
    {
        if (canStopAnimFromSlider)
        {
            switch (animState)
            {
                case PlayingAnimState.SingleAnim:
                    AnimationMode.SampleAnimationClip(animatorsInScene[indexAnimator].gameObject, animationClip, animationClip.length);
                    break;
                case PlayingAnimState.MultipleAnim:
                    foreach (DataAnimation dA in tmp_DataAnimations)
                    {
                        if (dA.animator != null && dA.animationClip != null)
                            AnimationMode.SampleAnimationClip(dA.animator.gameObject, dA.animationClip, dA.animationClip.length);
                    }
                    break;
            }
            StopAnimation();
        }
    }
    ///<summary> Stopping Button when u use slider will make animation going to the end of the animation.</summary>
    private void StoppingAnimationFromSlider(AnimationClip animationClip) => StoppingAnimationFromSlider(null, animationClip);
    ///<summary> Stopping Button when u use slider will make animation going to the end of all animations.</summary>
    private void StoppingAnimationFromSlider(List<DataAnimation> tmp_DataAnimations) => StoppingAnimationFromSlider(tmp_DataAnimations, null);

    ///<summary> Looping Method to loop one or all animations when they are playing.</summary>
    private void LoopingAnimMethod(float lengthAnim)
    {
        switch (animState)
        {
            case PlayingAnimState.SingleAnim:
                if (loopingAnim && animTime >= lengthAnim)
                {
                    lastEditorTime = Time.realtimeSinceStartup;
                    timePause = 0f;
                }
                break;
            case PlayingAnimState.MultipleAnim:
                if (loopingAnim && animTime >= lengthAnim)
                {
                    lastEditorTime = Time.realtimeSinceStartup;
                    timePause = 0f;
                }
                break;
        }


    }

    //Using Slider Frame to navigate in all animation or let's editor time play all animation
    public void TimeManagement(float lengthAnim)
    {
        if (needUseSlider)
        {
            //Cast in Frame
            if (animState == PlayingAnimState.MultipleAnim)
                    biggestLenght = (int)(lengthAnim * 60);
            animTime = timeForAnimation / 60f;
        }
        else
        {
            animTime = Time.realtimeSinceStartup - lastEditorTime + timePause;
            animTime *= timeScale;
        }
    }
    public void TimeManagement() => TimeManagement(0);

    // Improved System of rewind Helping by Nam
    private void Rewind(float lenghtAnim, List<DataAnimation> tmp_DataAnimations, AnimationClip animationClip)
    {
        switch (animState)
        {
            case PlayingAnimState.SingleAnim:
                if (rewindLeft)
                {
                    timePause -= 1f / 60f;
                    timePause = Mathf.Clamp(timePause, 0, animationClip.length);
                    PlaySingleAnimation(animationClip, timePause);
                    rewindLeft = false;
                }
                if (rewindRight)
                {
                    timePause += 1f / 60f;
                    timePause = Mathf.Clamp(timePause, 0, animationClip.length);
                    PlaySingleAnimation(animationClip, timePause);
                    rewindRight = false;
                }
                break;

            case PlayingAnimState.MultipleAnim:
                if (rewindLeft)
                {
                    timePause -= 1f / 60f;
                    timePause = Mathf.Clamp(timePause, 0, lenghtAnim);
                    PlayingAllAnimation(lenghtAnim, timePause, tmp_DataAnimations);
                    rewindLeft = false;
                }
                if (rewindRight)
                {
                    timePause += 1f / 60f;
                    timePause = Mathf.Clamp(timePause, 0, lenghtAnim);
                    PlayingAllAnimation(lenghtAnim, timePause, tmp_DataAnimations);
                    rewindRight = false;
                }
                break;
        }
    }
    ///<summary> Rewind Method to move frame by frame on an animation.</summary>
    private void Rewind(float lenghtAnim, AnimationClip animationClip) => Rewind(lenghtAnim, null, animationClip);
    ///<summary> Rewind Method to move frame by frame on all animations.</summary>
    private void Rewind(float lenghtAnim, List<DataAnimation> tmp_DataAnimations) => Rewind(lenghtAnim, tmp_DataAnimations, null);

    private void PlaySingleAnimation(AnimationClip animationClip, float time)
    {
        if (time >= animationClip.length && !needUseSlider && !loopingAnim)
            StopAnimation();
        else
            if (AnimationMode.InAnimationMode())
                AnimationMode.SampleAnimationClip(animatorsInScene[indexAnimator].gameObject, animationClip, time);
    }

    private void PlayingAllAnimation(float biggestLenght, float time, List<DataAnimation> tmp_DataAnimations)
    {
        if (time >= biggestLenght && !needUseSlider && !loopingAnim)
        {
            isPlayingMultipleAnim = false;
            StopAnimation();
        }
        else
            if (AnimationMode.InAnimationMode())
                foreach (DataAnimation dA in tmp_DataAnimations)
                {
                    if (dA.animator != null && dA.animationClip != null)
                        AnimationMode.SampleAnimationClip(dA.animator.gameObject, dA.animationClip, time);
                }
    }

    //Method to call Update and Start animation, stock time to be use for a editor delta time
    private void StartAnimation()
    {
        AnimationMode.StartAnimationMode();
        EditorApplication.update += OnEditorUpdate;
        lastEditorTime = Time.realtimeSinceStartup;
        isPlayingAnim = true;
        if (animState == PlayingAnimState.MultipleAnim)
            isPlayingMultipleAnim = true;
    }

    //Method to stop calling Update and Stop animation
    private void StopAnimation()
    {
        AnimationMode.StopAnimationMode();
        EditorApplication.update -= OnEditorUpdate;
        isPlayingAnim = false;
        canStopAnimFromSlider = false;
        if (animState == PlayingAnimState.MultipleAnim)
            isPlayingMultipleAnim = false;
        pause = false;
        ChangeTextureBetweenPlayAndPause();
        timePause = 0f;
    }

    private void ReorderingDataAnimation()
    {
        for(int i = 0; i < dataAnimations.Count; i++)
        {
            if(dataAnimations[i].animator == null || dataAnimations[i].animationClip == null)
            {
                dataAnimations.RemoveAt(i);
            }
        }
    }

    //Method to find all animator in scene, it will search all rootGameObject and will try to find animator in all of rootGameObject
    private void FindAnimatorsInScene()
    {
        animatorsInScene.Clear();
        GameObject[] rootGameObjects = new GameObject[1];

        if (ToolBoxUtils.IntToBoolean(intprefabButton))
        {
            Scene scene = SceneManager.GetActiveScene();
            if (!scene.IsValid()) return;

            rootGameObjects = scene.GetRootGameObjects();
        }
        else
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage == null) return;

            rootGameObjects[0] = prefabStage.prefabContentsRoot;
        }

        foreach (GameObject rootGameObject in rootGameObjects)
        {
            if (!rootGameObject.activeInHierarchy) continue;

            for (int i = 0; i < rootGameObject.GetComponentsInChildren<Animator>().Length; i++)
            {
                if (!rootGameObject.GetComponentsInChildren<Animator>()[i].gameObject.activeInHierarchy) continue;
                //Stock Animator if search bar contains no chars or else name contain chars of the search bar
                if (searchStringAnimator == null || searchStringAnimator == "")
                    animatorsInScene.Add(rootGameObject.GetComponentsInChildren<Animator>()[i]);
                else
                    if (rootGameObject.GetComponentsInChildren<Animator>()[i].name.Contains(searchStringAnimator))
                        animatorsInScene.Add(rootGameObject.GetComponentsInChildren<Animator>()[i]);
            }
        }
    }

    //Method to find all animation in animator who got select, it will search all rootGameObject and will try to find animator in all of rootGameObject
    private void FindAnimationInAnimator()
    {
        animationInAnimator.Clear();

        if(animatorsInScene.Count > 0)
            if (animatorsInScene[indexAnimator].runtimeAnimatorController.animationClips.Length > 0)
                foreach (AnimationClip animClip in animatorsInScene[indexAnimator].runtimeAnimatorController.animationClips)
                {
                    //Stock Animations if search bar contains no chars or else name contain chars of the search bar
                    if (searchStringAnimation == null || searchStringAnimation == "")
                        animationInAnimator.Add(animClip);
                    else
                        if (animClip.name.Contains(searchStringAnimation))
                            animationInAnimator.Add(animClip);
                }
    }

    void ChangeTextureBetweenPlayAndPause()
    {
        if (pause)
        {
            centerButtonTexture = playTexture;
            centerButtonTextHover = "Play";
        }
        else
        {
            centerButtonTexture = pauseTexture;
            centerButtonTextHover = "Pause";
        }
    }

    //Method to call Animator Init and Find method to clean some duplicate code
    void UpdateAnimatorList()
    {
        ListAnimatorInit();
        FindAnimatorsInScene();
    }

    //Method to call Animation Init and Find method to clean some duplicate code
    void UpdateAnimationList()
    {
        ListAnimationInit();
        FindAnimationInAnimator();
    }
}