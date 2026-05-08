using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static SoundManager instance;

    // all of our sounds
    [SerializeField] private AudioSource buttonClickSound, ballThrowSound, tableHitSound, cupHitSound,
        ballInCupSound, endOfGameCoinSound, songAudioSource;
    public AudioSource streakSound;

    // array of all the songs and index variables for the playlist
    [SerializeField] private AudioClip[] songs;
    [HideInInspector] public int randIndex;
    private int lastIndex;

    // timer for slight delay in between songs
    private float currentTimeBetweenSongs;
    private float timeBetweenSongsThreshold = 0.6f;

    private void Awake()
    {
        // public instance that can be accessed anywhere in our code
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        // start playing the background music
        randIndex = Random.Range(0, songs.Length);
        lastIndex = randIndex;
        songAudioSource.clip = songs[randIndex];
        if (songAudioSource.enabled) // we get a warning if this if statement isn't here
            songAudioSource.Play();
    }

    private void Update()
    {   
        // if we are in the main menu and a song isn't playing start a new song
        if (!songAudioSource.isPlaying && SceneManager.GetActiveScene().name == SceneNames.MAIN_MENU_SCENE_NAME)
        {
            StartNewSong();
        }
        else if (songAudioSource.isPlaying && SceneManager.GetActiveScene().name != SceneNames.MAIN_MENU_SCENE_NAME) {
            // if we are in a gamemode and a song is playing stop it
            songAudioSource.Stop();
        }
    }

    void StartNewSong()
    {
        // start adding to the timer
        currentTimeBetweenSongs += Time.deltaTime;

        // if the timer is done
        if (currentTimeBetweenSongs >= timeBetweenSongsThreshold)
        {
            // will keep assigning a new rand index
            // until it is not the same one as last time
            // meaning that we will not have the same song play twice in a row
            if (randIndex == lastIndex)
                randIndex = Random.Range(0, songs.Length);

            // if the rand index isn't the same as the last index
            if (randIndex != lastIndex)
            {
                // play the song at the randIndex
                songAudioSource.clip = songs[randIndex];
                songAudioSource.Play();

                // set the last index
                lastIndex = randIndex;

                // reset the timer
                currentTimeBetweenSongs = 0f;
            }
        }
    }

    public void PlayButtonClickSound()
    {
        // play the sound
        buttonClickSound.Play();
    }

    public void PlayBallThrowSound()
    {
        // play the sound
        ballThrowSound.Play();
    }

    public void PlayTableHitSound()
    {
        // play the sound
        tableHitSound.Play();
    }
    
    public void PlayCupHitSound()
    {
        // play the sound
        cupHitSound.Play();
    }

    public void PlayBallInCupSound()
    {
        // play the sound
        ballInCupSound.Play();
    }

    public void PlayStreakSound(float pitch, float volume)
    {
        // assign the pitch and volume
        streakSound.pitch = pitch;
        streakSound.volume = volume;

        // play the sound
        streakSound.Play();
    }

    public void PlayEndOfGameCoinSound()
    {
        // play the sound
        endOfGameCoinSound.Play();
    }
}