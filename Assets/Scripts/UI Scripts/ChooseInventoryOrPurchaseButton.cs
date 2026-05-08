using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseInventoryOrPurchaseButton : MonoBehaviour
{
    // has to match with one in the ShopInventoryDataManager
    public string savedFileName;

    // purchase and inventory button
    public Button purchaseButton;
    public Button inventoryButton;

    // has to be done in the Start function because ShopInventoryDataManager.instance is initialized in
    // the Awake function
    private void Start()
    {
        // if we do own the skin
        if (ShopInventoryDataManager.instance.GetOwnedData(savedFileName))
        {
            // disable the purchase button
            purchaseButton.gameObject.SetActive(false);
        }
        else { // we don't own the skin
            inventoryButton.gameObject.SetActive(false); // disable the inventory button
        }
    }
}