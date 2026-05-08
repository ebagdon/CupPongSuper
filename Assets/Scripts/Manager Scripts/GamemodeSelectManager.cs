using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeSelectManager : MonoBehaviour
{
    // our gamemode select canvas
    [SerializeField] private RectTransform gamemodeSelectCanvasContent;

    // masks for ui
    [SerializeField] private RectTransform classicDescriptionMask, partyDescriptionMask;

    // bgs and their original positions
    [SerializeField] private RectTransform classicDescriptionBG, partyDescriptionBG;
    private Vector3 classicDescriptionBGOriginalPos, partyDescriptionBGOriginalPos;

    // texts
    [SerializeField] private Text classicText, classicSelectText, partyText, partySelectText;

    // selection states
    private string currentSelectionState;
    private string selectionStateClassic = "Classic";
    private string selectionStateParty = "Party";

    // the speed we move the bgs at
    private float descriptionBGMoveSpeed = 1300f;

    private void Awake()
    {
        // initialize the original positions
        classicDescriptionBGOriginalPos = classicDescriptionBG.anchoredPosition;
        partyDescriptionBGOriginalPos = partyDescriptionBG.anchoredPosition;
    }

    private void Update()
    {
        // if the gamemodeSelectCanvas is not enabled and the
        // currentSelectionState is not empty
        if (gamemodeSelectCanvasContent.anchoredPosition != Vector2.zero && currentSelectionState != "")
        {
            Debug.Log("Running");

            // reset the selection state
            currentSelectionState = "";

            // reset all of the description bgs' positions
            classicDescriptionBG.anchoredPosition = classicDescriptionBGOriginalPos;
            partyDescriptionBG.anchoredPosition = partyDescriptionBGOriginalPos;

            // disable and enable texts
            classicSelectText.enabled = false;
            partySelectText.enabled = false;
            classicText.enabled = true;
            partyText.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        // handle the positions
        HandleClassicDescription();
        HandlePartyDescription();
    }

    void HandleClassicDescription()
    {
        // move the bg out and in based on the selection state
        if (currentSelectionState == selectionStateClassic && Vector3.Distance(classicDescriptionBG.anchoredPosition, new Vector3(0f, 76f, 0f)) > 0.01f)
        {
            classicDescriptionBG.anchoredPosition = Vector3.MoveTowards(classicDescriptionBG.anchoredPosition,
                new Vector3(0f, 76f, 0f), descriptionBGMoveSpeed * Time.deltaTime);
        }
        else if (currentSelectionState != selectionStateClassic && Vector3.Distance(classicDescriptionBG.anchoredPosition, classicDescriptionBGOriginalPos) > 0.01f) {
            classicDescriptionBG.anchoredPosition = Vector3.MoveTowards(classicDescriptionBG.anchoredPosition,
                classicDescriptionBGOriginalPos, descriptionBGMoveSpeed * Time.deltaTime);
        }
    }

    void HandlePartyDescription()
    {
        // move the bg out and in based on the selection state
        if (currentSelectionState == selectionStateParty && Vector3.Distance(partyDescriptionBG.anchoredPosition, new Vector3(0f, 5.76f, 0f)) > 0.01f)
        {
            partyDescriptionBG.anchoredPosition = Vector3.MoveTowards(partyDescriptionBG.anchoredPosition,
                new Vector3(0f, 5.76f, 0f), descriptionBGMoveSpeed * Time.deltaTime);
        }
        else if (currentSelectionState != selectionStateParty && Vector3.Distance(partyDescriptionBG.anchoredPosition, partyDescriptionBGOriginalPos) > 0.01f) {
            partyDescriptionBG.anchoredPosition = Vector3.MoveTowards(partyDescriptionBG.anchoredPosition,
                partyDescriptionBGOriginalPos, descriptionBGMoveSpeed * Time.deltaTime);
        }
    }

    public void SelectionStateToClassic()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // if the currentSelectionState is already selectionStateClassic load the scene 
        if (currentSelectionState == selectionStateClassic)
        {
            SceneTransitionsManager.instance.SetSceneTransitionToClassicGamemode();
            return;
        }

        // set the currentSelectionState
        currentSelectionState = selectionStateClassic;

        // enable texts
        classicText.enabled = false;
        classicSelectText.enabled = true;
        partyText.enabled = true;
        partySelectText.enabled = false;
    }

    public void SelectionStateToParty()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // if the currentSelectionState is already selectionStateParty load the scene    
        if (currentSelectionState == selectionStateParty)
        {
            SceneTransitionsManager.instance.SetSceneTransitionToPartyGamemode();
            return;
        }

        // set the currentSelectionState
        currentSelectionState = selectionStateParty;

        // enable texts
        classicText.enabled = true;
        classicSelectText.enabled = false;
        partyText.enabled = false;
        partySelectText.enabled = true;
    }
}