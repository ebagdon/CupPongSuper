using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpecialButtonClickFunction : MonoBehaviour
{
    // components
    private LanguageManager languageManager;

    private void Awake()
    {
        // components
        languageManager = GameObject.Find(ObjectNames.LanguageManager_NAME).GetComponent<LanguageManager>();

        // transorm.parent gets us the parent of our gameObject
        if (transform.parent.name == UIObjectNames.BACK_TO_MAIN_MENU_BUTTON_NAME)
        {
            // gives our button the BackToMainMenu function for its click function
            GetComponent<Button>().onClick.AddListener(BackToMainMenu);
        }
        else if (transform.parent.name == UIObjectNames.RESTART_BUTTON_NAME) {
            // gives our button the RestartGame function for its click function
            GetComponent<Button>().onClick.AddListener(RestartGame);
        }
        else if (gameObject.CompareTag(Tags.LANGUAGE_BUTTON_TAG)) {
            // gives our button the SetLanguage function for its click function
            GetComponent<Button>().onClick.AddListener(ChangeLanguagePrompt);
        }
        else if (gameObject.name == "LanguagePromptYesButton") {
            // gives our button the ChangeLanguage function for its click function
            GetComponent<Button>().onClick.AddListener(ChangeLanguage);
        }
        else if (gameObject.name == "LanguagePromptNoButton") {
            // gives our button the CancelLanguagePrompt function for its click function
            GetComponent<Button>().onClick.AddListener(CancelLanguagePrompt);
        }
    }

    void BackToMainMenu()
    {
        // play the button sound and start the transition to the main menu
        SoundManager.instance.PlayButtonClickSound();
        SceneTransitionsManager.instance.SetSceneTransitionToMainMenu();
    }

    public void RestartGame()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // based on what scene we are in set the scene transition to that scene
        if (SceneManager.GetActiveScene().name == SceneNames.CLASSIC_GAMEMODE_SCENE_NAME)
            SceneTransitionsManager.instance.SetSceneTransitionToClassicGamemode();
        else if (SceneManager.GetActiveScene().name == SceneNames.PARTY_GAMEMODE_SCENE_NAME)
            SceneTransitionsManager.instance.SetSceneTransitionToPartyGamemode();
    }

    void ChangeLanguagePrompt()
    {
        // prompt to change the language
        languageManager.ChangeLanguagePrompt();
    }

    void ChangeLanguage()
    {
        // change the language
        languageManager.ChangeLanguage();
    }

    void CancelLanguagePrompt()
    {
        // cancel the language prompt
        languageManager.CancelLanguagePrompt();
    }

    public void StartSetSceneTransitionToMainMenuFromSkinsCanvas()
    {
        // set the SceneTransitionsManager's canvas to disable on scene transition
        // to the skins canvas and set the scene transition to the Main Menu
        SceneTransitionsManager.instance.canvas = GameObject.Find(UIObjectNames.SKINS_CANVAS_NAME).GetComponent<Canvas>();
        SceneTransitionsManager.instance.SetSceneTransitionToMainMenu();
    }
}