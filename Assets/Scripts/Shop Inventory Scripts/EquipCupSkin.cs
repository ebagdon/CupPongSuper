using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EquipCupSkin : MonoBehaviour
{
    // the name of our scene
    private string sceneName;

    // equip and equipped texts
    private Text equipText, equippedText;

    // bools for updating the texts
    private bool updateText;
    private bool needFirstUpdateText = true;

    // our cup skin info
    private CupSkinInventoryInfo cupSkinInventoryInfo;

    private void Awake()
    {
        // get the name of our scene
        sceneName = SceneManager.GetActiveScene().name;

        // get texts
        equipText = transform.GetChild(0).GetComponent<Text>();
        equippedText = transform.GetChild(1).GetComponent<Text>();

        // get cup skin info
        cupSkinInventoryInfo = transform.parent.GetComponent<CupSkinInventoryInfo>();
    }

    private void Update()
    {
        // update the text if needed
        if (updateText || needFirstUpdateText)
            ShowCorrectEquipOrEquippedText();

        // set needFirstUpdateText to false
        needFirstUpdateText = false;
    }

    void ShowCorrectEquipOrEquippedText()
    {
        // if our cupSkinName is equal to the equipped cup skin name
        if (ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_CUP_STRING_DATA) == cupSkinInventoryInfo.cupSkinName && !equippedText.enabled)
        {
            // enable the equipped text
            equipText.enabled = false;
            equippedText.enabled = true;

            // set update text to false
            updateText = false;
        }
        else if (ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_CUP_STRING_DATA) != cupSkinInventoryInfo.cupSkinName && equippedText.enabled) {
            // IF OUR CUPSKINNAME IS NOT EQUAL TO THE EQUIPPED CUP SKIN NAME
            
            // enable the equip text
            equippedText.enabled = false;
            equipText.enabled = true;

            // set update text to false
            updateText = false;
        }
    }

    public void ChangeEquippedCupSkin()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // save the equipped ball
        ShopInventoryDataManager.instance.SaveString(ShopInventoryDataManager.instance.equipped_CUP_STRING_DATA, cupSkinInventoryInfo.cupSkinName);

        // set update text to true
        updateText = true;

        // update all the cup equip/equipped texts
        EquipCupSkin[] equipCupSkinScripts = FindObjectsOfType<EquipCupSkin>();
        for (int i = 0; i < equipCupSkinScripts.Length; i++)
            equipCupSkinScripts[i].updateText = true;
    }
}