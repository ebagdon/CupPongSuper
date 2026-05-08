using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LanguageManager : MonoBehaviour
{
    // public instance can be accessed anywhere in our code
    public static LanguageManager instance;

    // all of our languages
    [HideInInspector]
    public string english = "English", french = "French", german = "German", spanish = "Spanish",
                    swedish = "Swedish", chinese_simplified = "Simplified Chinese", hindi = "Hindi";

    // current language
    [HideInInspector]
    public string language = "English";

    // new language for when we are trying to change languages
    private string newLanguage;

    private void Awake()
    {  
        // making it so this object doesn't get destroyed when a new scene is loaded
        DontDestroyOnLoad(this);

        // public instance can be accessed anywhere in our code
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // get and disable the prompt
        GameObject.Find(UIObjectNames.LANGUAGES_SELECT_CANVAS_NAME).transform.Find("LanguagePrompt").gameObject.SetActive(false);
    }

    public void ChangeLanguagePrompt()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // get the language of the button
        newLanguage = EventSystem.current.currentSelectedGameObject.
            GetComponent<LanguageButton>().language;

        // make sure the new language is different than the current language
        if (newLanguage != language)
        {
            // go through every button in the array and disable it
            Button[] buttons = FindObjectsOfType<Button>();
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].enabled = false;
                
            // show the prompt
            GameObject.Find(UIObjectNames.LANGUAGES_SELECT_CANVAS_NAME).transform.Find("LanguagePrompt").gameObject.SetActive(true);
        }
    }

    public void ChangeLanguage()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // save the new language and reset the new language variable in this script
        DataManager.instance.SaveLanguageData(newLanguage);
        language = newLanguage;
        newLanguage = "";

        // restart the scene
        SceneTransitionsManager.instance.canvas = GameObject.Find(UIObjectNames.LANGUAGES_SELECT_CANVAS_NAME).GetComponent<Canvas>();
        SceneTransitionsManager.instance.SetSceneTransitionToMainMenu();
    }

    public void CancelLanguagePrompt()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // reset the newLanguage variable in this script
        newLanguage = "";

        // go through every button in the array and disable it
        Button[] buttons = FindObjectsOfType<Button>();
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].enabled = true;

        // disable the prompt
        GameObject.Find(UIObjectNames.LANGUAGES_SELECT_CANVAS_NAME).transform.Find("LanguagePrompt").gameObject.SetActive(false);
    }
}