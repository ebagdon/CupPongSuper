using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ShopInventoryDataManager : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static ShopInventoryDataManager instance;

    // names of the data files for balls
    [HideInInspector]
    public string smile_BALL_OWNED_DATA, glow_BALL_OWNED_DATA, rainbow_BALL_OWNED_DATA, orangeSplash_BALL_OWNED_DATA, 
                    eight_BALL_OWNED_DATA, bluePearl_BALL_OWNED_DATA, pinkPearl_BALL_OWNED_DATA,
                    lightBulb_BALL_OWNED_DATA, crown_BALL_OWNED_DATA, diamond_BALL_OWNED_DATA, emerald_BALL_OWNED_DATA, 
                    spike_BALL_OWNED_DATA, meteor_BALL_OWNED_DATA, hat_BALL_OWNED_DATA, belt_BALL_OWNED_DATA,
                    blue_BALL_OWNED_DATA, yellow_BALL_OWNED_DATA;

    // names of the data files for cups
    [HideInInspector]
    public string redStripe_CUP_OWNED_DATA, retro_CUP_OWNED_DATA, glow_CUP_OWNED_DATA, brightStripe_CUP_OWNED_DATA,
                    rainbow_CUP_OWNED_DATA, glowStripe_CUP_OWNED_DATA;

    // names of the skins wwe have equipped
    [HideInInspector]
    public string equipped_BALL_STRING_DATA, equipped_CUP_STRING_DATA;

    private void Awake()
    {
        // this object won't be destroyed when a new scene is loaded
        DontDestroyOnLoad(this);

        // public instance that can be accessed anywhere in our code
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        // initialize the data file names
        InitFileNames();
    }

    void InitFileNames()
    {
        // the data that controls if we own a ball skin
        smile_BALL_OWNED_DATA = ShopInventoryDataNames.smile_BALL_OWNED_DATA_NAME;
        glow_BALL_OWNED_DATA = ShopInventoryDataNames.glow_BALL_OWNED_DATA_NAME;
        rainbow_BALL_OWNED_DATA = ShopInventoryDataNames.rainbow_BALL_OWNED_DATA_NAME;
        orangeSplash_BALL_OWNED_DATA = ShopInventoryDataNames.orangeSplash_BALL_OWNED_DATA_NAME;
        eight_BALL_OWNED_DATA = ShopInventoryDataNames.eight_BALL_OWNED_DATA_NAME;
        bluePearl_BALL_OWNED_DATA = ShopInventoryDataNames.bluePearl_BALL_OWNED_DATA_NAME;
        pinkPearl_BALL_OWNED_DATA = ShopInventoryDataNames.pinkPearl_BALL_OWNED_DATA_NAME;
        lightBulb_BALL_OWNED_DATA = ShopInventoryDataNames.lightBulb_BALL_OWNED_DATA_NAME;
        crown_BALL_OWNED_DATA = ShopInventoryDataNames.crown_BALL_OWNED_DATA_NAME;
        diamond_BALL_OWNED_DATA = ShopInventoryDataNames.diamond_BALL_OWNED_DATA_NAME;
        emerald_BALL_OWNED_DATA = ShopInventoryDataNames.emerald_BALL_OWNED_DATA_NAME;
        spike_BALL_OWNED_DATA = ShopInventoryDataNames.spike_BALL_OWNED_DATA_NAME;
        meteor_BALL_OWNED_DATA = ShopInventoryDataNames.meteor_BALL_OWNED_DATA_NAME;
        hat_BALL_OWNED_DATA = ShopInventoryDataNames.hat_BALL_OWNED_DATA_NAME;
        belt_BALL_OWNED_DATA = ShopInventoryDataNames.belt_BALL_OWNED_DATA_NAME;
        blue_BALL_OWNED_DATA = ShopInventoryDataNames.blue_BALL_OWNED_DATA_NAME;
        yellow_BALL_OWNED_DATA = ShopInventoryDataNames.yellow_BALL_OWNED_DATA_NAME;

        // the data that controls if we own a cup skin
        redStripe_CUP_OWNED_DATA = ShopInventoryDataNames.redStripe_CUP_OWNED_DATA_NAME;
        retro_CUP_OWNED_DATA = ShopInventoryDataNames.retro_CUP_OWNED_DATA_NAME;
        glow_CUP_OWNED_DATA = ShopInventoryDataNames.glow_CUP_OWNED_DATA_NAME;
        brightStripe_CUP_OWNED_DATA = ShopInventoryDataNames.brightStripe_CUP_OWNED_DATA_NAME;
        rainbow_CUP_OWNED_DATA = ShopInventoryDataNames.rainbow_CUP_OWNED_DATA_NAME;
        glowStripe_CUP_OWNED_DATA = ShopInventoryDataNames.glowStripe_CUP_OWNED_DATA_NAME;

        // the name of the skins we have equipped
        equipped_BALL_STRING_DATA = ShopInventoryDataNames.equipped_BALL_STRING_DATA_NAME;
        equipped_CUP_STRING_DATA = ShopInventoryDataNames.equipped_CUP_STRING_DATA_NAME;
    }

    public void SaveOwnedData(string filename, bool value)
    {
        // create a new binary formatter and set up a dataPath
        BinaryFormatter formatter = new BinaryFormatter();
        string dataPath = Application.persistentDataPath + filename;

        // create a file at the dataPath
        FileStream stream = new FileStream(dataPath, FileMode.Create);

        // new ShopInventoryData class
        ShopInventoryData ShopInventoryData = new ShopInventoryData();

        // check if we wanted to save a ball skin and then set it
        if (dataPath == Application.persistentDataPath + smile_BALL_OWNED_DATA)
            ShopInventoryData.smile_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + glow_BALL_OWNED_DATA)
            ShopInventoryData.glow_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + rainbow_BALL_OWNED_DATA)
            ShopInventoryData.rainbow_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + orangeSplash_BALL_OWNED_DATA)
            ShopInventoryData.orangeSplash_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + eight_BALL_OWNED_DATA)
            ShopInventoryData.eight_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + bluePearl_BALL_OWNED_DATA)
            ShopInventoryData.bluePearl_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + pinkPearl_BALL_OWNED_DATA)
            ShopInventoryData.pinkPearl_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + lightBulb_BALL_OWNED_DATA)
            ShopInventoryData.lightBulb_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + crown_BALL_OWNED_DATA)
            ShopInventoryData.crown_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + diamond_BALL_OWNED_DATA)
            ShopInventoryData.diamond_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + emerald_BALL_OWNED_DATA)
            ShopInventoryData.emerald_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + spike_BALL_OWNED_DATA)
            ShopInventoryData.spike_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + meteor_BALL_OWNED_DATA)
            ShopInventoryData.meteor_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + hat_BALL_OWNED_DATA)
            ShopInventoryData.hat_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + belt_BALL_OWNED_DATA)
            ShopInventoryData.belt_BALL_OWNED = value;
        else if (dataPath == Application.persistentDataPath + blue_BALL_OWNED_DATA)
            ShopInventoryData.blue_BALL_OWNED = value;

        // check if we wanted to save a cup skin and then set it
        if (dataPath == Application.persistentDataPath + redStripe_CUP_OWNED_DATA)
            ShopInventoryData.redStripe_CUP_OWNED = value;
        else if (dataPath == Application.persistentDataPath + retro_CUP_OWNED_DATA)
            ShopInventoryData.retro_CUP_OWNED = value;
        else if (dataPath == Application.persistentDataPath + glow_CUP_OWNED_DATA)
            ShopInventoryData.glow_CUP_OWNED = value;
        else if (dataPath == Application.persistentDataPath + brightStripe_CUP_OWNED_DATA)
            ShopInventoryData.brightStripe_CUP_OWNED = value;
        else if (dataPath == Application.persistentDataPath + rainbow_CUP_OWNED_DATA)
            ShopInventoryData.rainbow_CUP_OWNED = value;
        else if (dataPath == Application.persistentDataPath + glowStripe_CUP_OWNED_DATA)
            ShopInventoryData.glowStripe_CUP_OWNED = value;

        // close the file
        formatter.Serialize(stream, ShopInventoryData);
        stream.Close();
    }

    public bool GetOwnedData(string filename)
    {
        // set up a dataPath and a bool value to return
        string dataPath = Application.persistentDataPath + filename;
        bool value = false;

        // checks if the file exists
        if (File.Exists(dataPath))
        {
            // create a new binary formatter and open the file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Open);

            // read the data
            ShopInventoryData ShopInventoryData = formatter.Deserialize(stream) as ShopInventoryData;

            // check if we were trying to get a ball skin owned data and set the return value
            if (dataPath == Application.persistentDataPath + smile_BALL_OWNED_DATA)
                value = ShopInventoryData.smile_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + glow_BALL_OWNED_DATA)
                value = ShopInventoryData.glow_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + rainbow_BALL_OWNED_DATA)
                value = ShopInventoryData.rainbow_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + orangeSplash_BALL_OWNED_DATA)
                value = ShopInventoryData.orangeSplash_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + eight_BALL_OWNED_DATA)
                value = ShopInventoryData.eight_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + bluePearl_BALL_OWNED_DATA)
                value = ShopInventoryData.bluePearl_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + pinkPearl_BALL_OWNED_DATA)
                value = ShopInventoryData.pinkPearl_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + lightBulb_BALL_OWNED_DATA)
                value = ShopInventoryData.lightBulb_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + crown_BALL_OWNED_DATA)
                value = ShopInventoryData.crown_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + diamond_BALL_OWNED_DATA)
                value = ShopInventoryData.diamond_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + emerald_BALL_OWNED_DATA)
                value = ShopInventoryData.emerald_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + spike_BALL_OWNED_DATA)
                value = ShopInventoryData.spike_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + meteor_BALL_OWNED_DATA)
                value = ShopInventoryData.meteor_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + hat_BALL_OWNED_DATA)
                value = ShopInventoryData.hat_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + belt_BALL_OWNED_DATA)
                value = ShopInventoryData.belt_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + blue_BALL_OWNED_DATA)
                value = ShopInventoryData.blue_BALL_OWNED;
            else if (dataPath == Application.persistentDataPath + yellow_BALL_OWNED_DATA)
                value = ShopInventoryData.yellow_BALL_OWNED;

            // check if we were trying to get a cup skin owned data and set the return value
            if (dataPath == Application.persistentDataPath + redStripe_CUP_OWNED_DATA)
                value = ShopInventoryData.redStripe_CUP_OWNED;
            else if (dataPath == Application.persistentDataPath + retro_CUP_OWNED_DATA)
                value = ShopInventoryData.retro_CUP_OWNED;
            else if (dataPath == Application.persistentDataPath + glow_CUP_OWNED_DATA)
                value = ShopInventoryData.glow_CUP_OWNED;
            else if (dataPath == Application.persistentDataPath + brightStripe_CUP_OWNED_DATA)
                value = ShopInventoryData.brightStripe_CUP_OWNED;
            else if (dataPath == Application.persistentDataPath + rainbow_CUP_OWNED_DATA)
                value = ShopInventoryData.rainbow_CUP_OWNED;
            else if (dataPath == Application.persistentDataPath + glowStripe_CUP_OWNED_DATA)
                value = ShopInventoryData.glowStripe_CUP_OWNED;

            // close the file
            stream.Close();
        }
        else // does not exist set default values
        {
            // most skins are not owned by default
            value = false;

            // the yellow and blue ball are owned by default
            if (dataPath == Application.persistentDataPath + blue_BALL_OWNED_DATA)
                value = true;
            else if (dataPath == Application.persistentDataPath + yellow_BALL_OWNED_DATA)
                value = true;
        }

        // return the data
        return value;
    }

    public void SaveString(string filename, string value)
    {
        // create a new binary formatter and setup a dataPath
        BinaryFormatter formatter = new BinaryFormatter();
        string dataPath = Application.persistentDataPath + filename;

        // create a file at the dataPath
        FileStream stream = new FileStream(dataPath, FileMode.Create);

        // new ShopInventoryData class
        ShopInventoryData ShopInventoryData = new ShopInventoryData();

        // save the currently equipped ball skin
        if (dataPath == Application.persistentDataPath + equipped_BALL_STRING_DATA)
            ShopInventoryData.equipped_BALL_STRING = value;

        // save the currently equipped cup skin
        if (dataPath == Application.persistentDataPath + equipped_CUP_STRING_DATA)
            ShopInventoryData.equipped_CUP_STRING = value;

        // close the file
        formatter.Serialize(stream, ShopInventoryData);
        stream.Close();
    }

    public string GetString(string filename)
    {
        // set up a dataPath and a return string value
        string dataPath = Application.persistentDataPath + filename;
        string value = "";

        // checks if the file exists
        if(File.Exists(dataPath))
        {
            // create a new binary formatter and open the file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Open);

            // read the data
            ShopInventoryData ShopInventoryData = formatter.Deserialize(stream) as ShopInventoryData;

            // get the currently equipped ball skin
            if (dataPath == Application.persistentDataPath + equipped_BALL_STRING_DATA)
                value = ShopInventoryData.equipped_BALL_STRING;

            // get the currently equipped cup skin
            if (dataPath == Application.persistentDataPath + equipped_CUP_STRING_DATA)
                value = ShopInventoryData.equipped_CUP_STRING;

            // close the file
            stream.Close();
        }
        else // does not exist set default values
        {
            // set the default equipped ball skin
            if (dataPath == Application.persistentDataPath + equipped_BALL_STRING_DATA)
                value = BallSkinNames.PING_PONG_BALL_NAME;

            // set the default equipped ball skin
            if (dataPath == Application.persistentDataPath + equipped_CUP_STRING_DATA)
                value = CupSkinNames.RED_CUP_NAME;
        }

        // return the data
        return value;
    }
}