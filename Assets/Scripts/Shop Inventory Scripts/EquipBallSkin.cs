using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EquipBallSkin : MonoBehaviour
{
    // the name of our scene
    private string sceneName;

    // equip and equipped texts
    private Text equipText, equippedText;

    // bools for updating the texts
    private bool updateText;
    private bool needFirstUpdateText = true;
    
    // our ball skin info
    private BallSkinInventoryInfo ballSkinInventoryInfo;

    private void Awake()
    {   
        // get the name of our scene
        sceneName = SceneManager.GetActiveScene().name;

        // get texts
        equipText = transform.GetChild(0).GetComponent<Text>();
        equippedText = transform.GetChild(1).GetComponent<Text>();

        // get ball skin info
        ballSkinInventoryInfo = transform.parent.GetComponent<BallSkinInventoryInfo>();
    }

    private void Update()
    {   
        // update the text if needed
        if (updateText || needFirstUpdateText)
            ShowEquipOrEquippedText();

        // set needFirstUpdateText to false
        needFirstUpdateText = false;
    }

    void ShowEquipOrEquippedText()
    {   
        // if our ballSkinName is equal to the equipped ball skin name
        if (ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_BALL_STRING_DATA) == ballSkinInventoryInfo.ballSkinName && !equippedText.enabled)
        {   
            // enable the equipped text
            equipText.enabled = false;
            equippedText.enabled = true;

            // set update text to false
            updateText = false;
        }
        else if (ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_BALL_STRING_DATA) != ballSkinInventoryInfo.ballSkinName && equippedText.enabled) {
            // IF OUR BALLSKINNAME IS NOT EQUAL TO THE EQUIPPED BALL SKIN NAME

            // enable the equip text
            equippedText.enabled = false;
            equipText.enabled = true;

            // set update text to false
            updateText = false;
        }
    }

    public void ChangeEquippedBallSkin()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // save the equipped ball
        ShopInventoryDataManager.instance.SaveString(ShopInventoryDataManager.instance.equipped_BALL_STRING_DATA, ballSkinInventoryInfo.ballSkinName);
        
        // set update text to true
        updateText = true;

        // update all the ball equip/equipped texts
        EquipBallSkin[] equipBallSkinScripts = FindObjectsOfType<EquipBallSkin>();
        for (int i = 0; i < equipBallSkinScripts.Length; i++)
            equipBallSkinScripts[i].updateText = true;
    }
}