using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    // THESE MUST MATCH WITH THE PRODUCTS ATTACHED TO THE BUY BUTTONS
    private string coin1000id = "com.qiveal.cuppongsuper.coin1000";
    private string coin4250id = "com.qiveal.cuppongsuper.coin4250";
    private string coin8750id = "com.qiveal.cuppongsuper.coin8750";
    private string coin16500id = "com.qiveal.cuppongsuper.coin16500";
    private string removeadsid = "com.qiveal.cuppongsuper.removeads";
    
    // called when a purchase is completed
    public void OnPurchaseComplete(Product product)
    {
        // check which purchase was completed and reward the user for that specific purchase
        if (product.definition.id == coin1000id)
        {
            int currentCoins = DataManager.instance.GetInt(DataManager.instance.coinsData);
            DataManager.instance.SaveInt(DataManager.instance.coinsData, currentCoins + 1000);
        }
        else if (product.definition.id == coin4250id)
        {
            int currentCoins = DataManager.instance.GetInt(DataManager.instance.coinsData);
            DataManager.instance.SaveInt(DataManager.instance.coinsData, currentCoins + 4250);
        }
        else if (product.definition.id == coin8750id)
        {
            int currentCoins = DataManager.instance.GetInt(DataManager.instance.coinsData);
            DataManager.instance.SaveInt(DataManager.instance.coinsData, currentCoins + 8750);
        }
        else if (product.definition.id == coin16500id)
        {
            int currentCoins = DataManager.instance.GetInt(DataManager.instance.coinsData);
            DataManager.instance.SaveInt(DataManager.instance.coinsData, currentCoins + 16500);
        }

        if (product.definition.id == removeadsid)
        {
            DataManager.instance.SaveBool(DataManager.instance.playAdsData, false);
        }
    }

    // called when a purchase fails
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(product.definition.id + "Failure Reason: " + failureReason);
    }
}