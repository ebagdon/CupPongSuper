using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlayedManager : MonoBehaviour
{
    // instance so this object will be in every scene and not duplicate
    private static TimePlayedManager instance;
    
    // the time that we have played this game
    private float timePlayed;

    // timer for when to save the time played
    private float timeSinceLastSaved;
    private float timeSinceLastSavedThreshold = 15f;

    private void Awake()
    {
        // instance so this object will be in every scene and not duplicate
        DontDestroyOnLoad(this);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // get the time played
        timePlayed = DataManager.instance.GetFloat(DataManager.instance.timePlayedData);
    }

    private void Update()
    {
        // add to the time played
        timePlayed += Time.deltaTime;

        // add to the timer and when it is done save the time played data
        timeSinceLastSaved += Time.deltaTime;
        if (timeSinceLastSaved >= timeSinceLastSavedThreshold)
        {
            DataManager.instance.SaveFloat(DataManager.instance.timePlayedData, timePlayed);
            timeSinceLastSaved = 0f;
        }
    }
}