using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextLanguageController : MonoBehaviour
{
    // the text we are controller
    private Text text;

    // multiline makes it so we can use more than one line in the string field
    // the translations
    [Multiline]
    public string English_Text, French_Text, German_Text, Spanish_Text, Swedish_Text,
                    Chinese_Simplified_Text, Hindi_Text;

    // the line spacing for each language
    [SerializeField]
    private float englishLineSpacing, frenchLineSpacing, germanLineSpacing, spanishLineSpacing,
                swedishLineSpacing, chineseSimplifiedLineSpacing, hindiLineSpacing;

    private void Awake()
    {
        // get the text
        text = GetComponent<Text>();
    }

    private void Update()
    {
        // handle updating the text
        UpdateText();
    }

    void UpdateText()
    {
        // see which language we are using and make sure the text isn't already on that language
        if (LanguageManager.instance.language == LanguageManager.instance.english && text.text != English_Text)
        {
            // change the text's string and the line spacing
            ChangeText(English_Text);
            text.lineSpacing = englishLineSpacing;
        }
        else if (LanguageManager.instance.language == LanguageManager.instance.french && text.text != French_Text) {
            // change the text's string and the line spacing
            ChangeText(French_Text);
            text.lineSpacing = frenchLineSpacing;
        }
        else if (LanguageManager.instance.language == LanguageManager.instance.german && text.text != German_Text) {
            // change the text's string and the line spacing
            ChangeText(German_Text);
            text.lineSpacing = germanLineSpacing;
        }
        else if (LanguageManager.instance.language == LanguageManager.instance.spanish && text.text != Spanish_Text) {
            // change the text's string and the line spacing
            ChangeText(Spanish_Text);
            text.lineSpacing = spanishLineSpacing;
        }
        else if (LanguageManager.instance.language == LanguageManager.instance.swedish && text.text != Swedish_Text) {
            // change the text's string and the line spacing
            ChangeText(Swedish_Text);
            text.lineSpacing = swedishLineSpacing;
        }
        else if (LanguageManager.instance.language == LanguageManager.instance.chinese_simplified && text.text != Chinese_Simplified_Text) {
            // change the text's string and the line spacing
            ChangeText(Chinese_Simplified_Text);
            text.lineSpacing = chineseSimplifiedLineSpacing;
        }
        else if (LanguageManager.instance.language == LanguageManager.instance.hindi && text.text != Hindi_Text) {
            // change the text's string and the line spacing
            ChangeText(Hindi_Text);
            text.lineSpacing = hindiLineSpacing;
        }
    }

    void ChangeText(string message)
    {
        // use a string builder to change the text
        StringBuilder sb = new StringBuilder();
        sb.Clear();
        sb.Append(message);
        text.text = sb.ToString();

        // check if bold should be true
        bool bold = false;
        if (LanguageManager.instance.language == LanguageManager.instance.chinese_simplified) 
            bold = true;
        else if (LanguageManager.instance.language == LanguageManager.instance.hindi)
            bold = true;

        // see if we need the text to be bold or not
        // and make sure the text isn't already the needed FontStyle then set the FontStyle
        if (bold && text.fontStyle != FontStyle.Bold)
            text.fontStyle = FontStyle.Bold;
        else if (bold == false && text.fontStyle != FontStyle.Normal)
            text.fontStyle = FontStyle.Normal;
    }
}