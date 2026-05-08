using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    // public instance we can access this anywhere in other scripts
    public static DataManager instance;

    // tutorial data
    [HideInInspector]
    public string classicGamemodeTutorialDoneData, partyGamemodeTutorialDoneData;

    // current language
    [HideInInspector]
    public string currentLanguageData;

    // volume datas
    [HideInInspector]
    public string masterVolumeData, musicVolumeData, soundEffectsVolumeData;

    // the amount of coins we have
    [HideInInspector]
    public string coinsData;

    // which ball throw mode we are using
    [HideInInspector]
    public string ballThrowMode_LEGACY_DATA, ballThrowMode_PULLBACK_DATA;

    // the amount of wins we have
    [HideInInspector]
    public string winsData;

    // the amount of time we played and if we asked for our user to review the app
    [HideInInspector]
    public string timePlayedData, askedToReviewAppData;

    // data for our remove ads in app purchase
    [HideInInspector]
    public string playAdsData;

    // language manager
    private LanguageManager languageManager;

    private void Awake()
    {
        // making it so this object doesn't get destroyed when a new scene is loaded
        DontDestroyOnLoad(this);

        // public instance
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // initialize the data file names
        InitFileNames();

        // getting the language manager and get the current language
        languageManager = GameObject.Find(ObjectNames.LanguageManager_NAME).GetComponent<LanguageManager>();
        languageManager.language = GetLanguageData();
    }

    void InitFileNames()
    {
        // tutorial data
        classicGamemodeTutorialDoneData = DataNames.classicGamemodeTutorialDoneData_NAME;
        partyGamemodeTutorialDoneData = DataNames.partyGamemodeTutorialDoneData_NAME;

        // the selected language
        currentLanguageData = DataNames.currentLanguageData_NAME;

        // volume settings
        masterVolumeData = DataNames.masterVolumeData_NAME;
        musicVolumeData = DataNames.musicVolumeData_NAME;
        soundEffectsVolumeData = DataNames.soundEffectsVolumeData_NAME;

        // the amount of coins we have
        coinsData = DataNames.coinsData_NAME;

        // which throw mode we have selected
        ballThrowMode_LEGACY_DATA = DataNames.ballThrowMode_LEGACY_DATA_NAME;
        ballThrowMode_PULLBACK_DATA = DataNames.ballThrowMode_PULLBACK_DATA_NAME;

        // how many wins we have
        winsData = DataNames.winsData_NAME;

        // the amount of time we played and if we asked for our user to review the app
        timePlayedData = DataNames.timePlayedData_NAME;
        askedToReviewAppData = DataNames.askedToReviewAppData_NAME;

        // the data for our remove ads in app purchase
        playAdsData = DataNames.playAdsData_NAME;
    }

    public void SaveInt(string filename, int value)
    {
        // create a new binary formatter and set up a dataPath
        BinaryFormatter formatter = new BinaryFormatter();
        string dataPath = Application.persistentDataPath + filename;

        // create a file at the dataPath
        FileStream stream = new FileStream(dataPath, FileMode.Create);

        // new data class
        Data Data = new Data();

        // check which variable we were trying to set and set it
        if (dataPath == Application.persistentDataPath + coinsData)
            Data.coinsData = value;
        else if (dataPath == Application.persistentDataPath + winsData)
            Data.winsData = value;

        // close the file
        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public int GetInt(string filename)
    {
        // set a dataPath and an int to return
        string dataPath = Application.persistentDataPath + filename;
        int value = 0;

        // checks if the file exists
        if(File.Exists(dataPath))
        {
            // create a new binary formatter and open the file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Open);

            // new data class
            Data Data = formatter.Deserialize(stream) as Data;

            // check which variable we were trying to get and set the return value
            if (dataPath == Application.persistentDataPath + coinsData)
                value = Data.coinsData;
            else if (dataPath == Application.persistentDataPath + winsData)
                value = Data.winsData;

            // close the file
            stream.Close();
        }
        else // does not exist set default values
        {
            // check which variable we were trying to get and set the return value to the default value for that var
            if (dataPath == Application.persistentDataPath + coinsData)
                value = 0;
            else if (dataPath == Application.persistentDataPath + winsData)
                value = 0;
        }

        // return the int value
        return value;
    }

    public void SaveFloat(string filename, float value)
    {   
        // setup a new binary formatter and data path
        BinaryFormatter formatter = new BinaryFormatter();
        string dataPath = Application.persistentDataPath + filename;

        // create the file at that dataPath
        FileStream stream = new FileStream(dataPath, FileMode.Create);

        // new data class
        Data Data = new Data();

        // see which variable we were trying to save and set it
        if (dataPath == Application.persistentDataPath + masterVolumeData)
            Data.masterVolumeData = value;
        else if (dataPath == Application.persistentDataPath + musicVolumeData)
            Data.musicVolumeData = value;
        else if (dataPath == Application.persistentDataPath + soundEffectsVolumeData)
            Data.soundEffectsVolumeData = value;
        else if (dataPath == Application.persistentDataPath + timePlayedData)
            Data.timePlayedData = value;

        // close the file
        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public float GetFloat(string saveFileName)
    {
        // dataPath and return float
        string dataPath = Application.persistentDataPath + saveFileName;
        float value = 0f;

        // check if the file at the dataPath exists
        if(File.Exists(dataPath))
        {
            // open the file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Open);

            // read the data
            Data Data = formatter.Deserialize(stream) as Data;

            // check for which variable we are getting and set the return value
            if (dataPath == Application.persistentDataPath + masterVolumeData)
                value = Data.masterVolumeData;
            else if (dataPath == Application.persistentDataPath + musicVolumeData)
                value = Data.musicVolumeData;
            else if (dataPath == Application.persistentDataPath + soundEffectsVolumeData)
                value = Data.soundEffectsVolumeData;
            else if (dataPath == Application.persistentDataPath + timePlayedData)
                value = Data.timePlayedData;

            // close the file
            stream.Close();
        }
        else // does not exist set default values
        {
            // see which variable we were trying to get and set the return value to the default value
            if (dataPath == Application.persistentDataPath + masterVolumeData)
                value = 1f;
            else if (dataPath == Application.persistentDataPath + musicVolumeData)
                value = 20f;
            else if (dataPath == Application.persistentDataPath + soundEffectsVolumeData)
                value = 20f;
            else if (dataPath == Application.persistentDataPath + timePlayedData)
                value = 0f;
        }

        // return the float value
        return value;
    }

    public void SaveBool(string filename, bool value)
    {
        // set up a new binary formatter and a dataPath
        BinaryFormatter formatter = new BinaryFormatter();
        string dataPath = Application.persistentDataPath + filename;

        // create a file at the dataPath
        FileStream stream = new FileStream(dataPath, FileMode.Create);

        // new data class
        Data Data = new Data();

        // check which variable we were trying to set and set it
        if (dataPath == Application.persistentDataPath + classicGamemodeTutorialDoneData)
            Data.classicGamemodeTutorialDoneData = value;
        else if (dataPath == Application.persistentDataPath + partyGamemodeTutorialDoneData)
            Data.partyGamemodeTutorialDoneData = value;
        else if (dataPath == Application.persistentDataPath + ballThrowMode_LEGACY_DATA)
            Data.ballThrowMode_LEGACY_DATA = value;
        else if (dataPath == Application.persistentDataPath + ballThrowMode_PULLBACK_DATA)
            Data.ballThrowMode_PULLBACK_DATA = value;
        else if (dataPath == Application.persistentDataPath + askedToReviewAppData)
            Data.askedToReviewAppData = value;
        
        if (dataPath == Application.persistentDataPath + playAdsData)
            Data.playAdsData = value;

        // close the file
        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public bool GetBool(string filename)
    {
        // set up a dataPath and bool to return
        string dataPath = Application.persistentDataPath + filename;
        bool value = false;

        // checks if a file exists at the dataPath
        if(File.Exists(dataPath))
        {   
            // set up a new binary formatter and open the file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Open);

            // new data class
            Data Data = formatter.Deserialize(stream) as Data;

            // check which variable we were trying to get and set the return value
            if (dataPath == Application.persistentDataPath + classicGamemodeTutorialDoneData)
                value = Data.classicGamemodeTutorialDoneData;
            else if (dataPath == Application.persistentDataPath + partyGamemodeTutorialDoneData)
                value = Data.partyGamemodeTutorialDoneData;
            else if (dataPath == Application.persistentDataPath + ballThrowMode_LEGACY_DATA)
                value = Data.ballThrowMode_LEGACY_DATA;
            else if (dataPath == Application.persistentDataPath + ballThrowMode_PULLBACK_DATA)
                value = Data.ballThrowMode_PULLBACK_DATA;
            else if (dataPath == Application.persistentDataPath + askedToReviewAppData)
                value = Data.askedToReviewAppData;

            if (dataPath == Application.persistentDataPath + playAdsData)
                value = Data.playAdsData;

            // close the file
            stream.Close();
        }
        else // does not exist set default values
        {
            // check which variable we were trying to get and set the return value
            if (dataPath == Application.persistentDataPath + classicGamemodeTutorialDoneData ||
                dataPath == Application.persistentDataPath + partyGamemodeTutorialDoneData)
            {
                value = false;
            }
            else if (dataPath == Application.persistentDataPath + ballThrowMode_LEGACY_DATA)
                value = true;
            else if (dataPath == Application.persistentDataPath + ballThrowMode_PULLBACK_DATA)
                value = false;
            else if (dataPath == Application.persistentDataPath + askedToReviewAppData)
                value = false;
            
            if (dataPath == Application.persistentDataPath + playAdsData)
                value = true;
        }

        // return the data
        return value;
    }

    public void SaveLanguageData(string languageToBeSaved)
    {
        // create a new BinaryFormatter and make a string for the dataPath
        BinaryFormatter formatter = new BinaryFormatter();
        string dataPath = Application.persistentDataPath + currentLanguageData;

        // create a file at the dataPath
        FileStream stream = new FileStream(dataPath, FileMode.Create);

        // set the data
        Data Data = new Data();
        Data.currentLanguageData = languageToBeSaved;

        // close the file
        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public string GetLanguageData()
    {
        // the path to the data and the default language variable
        string dataPath = Application.persistentDataPath + currentLanguageData;
        string value = "English";

        // checks if the file exists
        if (File.Exists(dataPath))
        {   
            // open the file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Open);

            // read the data
            Data Data = formatter.Deserialize(stream) as Data;

            // set the language and close the file
            value = Data.currentLanguageData;
            stream.Close();
        }

        // return the data
        return value;
    }
}