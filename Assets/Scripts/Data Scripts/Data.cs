using System;

[System.Serializable]
public class Data
{
    // SAVED DATA VALUES
    // IN THIS SCRIPT EVERYTHING IS ASSIGNED TO ITS DEFAULT VALUE

    // tutorial data
    public bool classicGamemodeTutorialDoneData = false;
    public bool partyGamemodeTutorialDoneData = false;

    // current language
    public string currentLanguageData = "English";

    // volume settings
    public float masterVolumeData = 1f;
    public float musicVolumeData = 20f;
    public float soundEffectsVolumeData = 20f;

    // how many coins we have
    public int coinsData = 0;

    // which ball throw mode we are using
    public bool ballThrowMode_LEGACY_DATA = true;
    public bool ballThrowMode_PULLBACK_DATA = false;

    // how many wins we have
    public int winsData = 0;

    // how much time we have played and if we have asked our user to review our app
    public float timePlayedData = 0f;
    public bool askedToReviewAppData = false;

    // data for our remove ads in app purchase
    public bool playAdsData = true;
}