using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    // instance that can be accessed from anywhere in our code
    public static GameManager instance;

    // our camera
    [SerializeField] private CinemachineVirtualCamera cinemachineCam;
    
    // the name of our scene
    private string sceneName;

    // array of all the balls and their spawn positions
    [SerializeField] private GameObject[] balls;
    [SerializeField] private Transform ballSpawnPos, rampShotEventBallSpawnPosition;

    // initial ball rotation
    [HideInInspector] public Quaternion initialBallRotation;

    // array of all the cup groups and variable for the active cup group
    [SerializeField] private GameObject[] cupGroups;
    private GameObject activeCupGroup;

    // variables for the amount of cups we have left
    [HideInInspector] public int cupsLeft = 10;
    private int initialCupsLeft = 10;

    // list of all the cups and all the cup postions
    public List<GameObject> cupsList = new List<GameObject>();
    [SerializeField] private Transform[] startingCupPositions, sixCupRepositionPositions, threeCupRepositionPositions;

    // the speed at which we reposition the cups
    private float repositionSpeed = 6f;

    // bools for repositioning the cups
    private bool cup1Done;
    private bool cup2Done;
    private bool cup3Done;
    private bool cup4Done;
    private bool cup5Done;
    private bool cup6Done;
    private bool sixCupsDoneRepositioning;
    private bool threeCupsDoneRepositioning;

    // variables for how many misses we have left
    [HideInInspector] public int missesLeft = 7;
    private int initialMissesLeft = 7;

    // gameplay canvas and texts
    [SerializeField] private Canvas gameplayCanvas;
    [SerializeField] private Text missesLeftText, coinsEarnedText;

    // how many coins we earn and how many coins we remove for missing
    private int coinsMinEarnedFromCup = 12, coinsMaxEarnedFromCup = 18;
    private int coinsMinRemovedFromMiss = 2, coinsMaxRemovedFromMiss = 4;

    // variables for the stats in the game over ui
    [HideInInspector] public int totalCoinsEarnedFromCups;
    [HideInInspector] public int totalCoinsRemovedFromMisses;
    [HideInInspector] public int coinsEarned;

    // the length of our streak
    private int streakCount;

    // variables for the streak text
    [SerializeField]private Text streakText;
    private string streakTextString;
    private bool updatedStreakTextString;
    private bool disabledStreakTextLanguageChange;

    // streak text animation
    private StreakTextAnimation streakTextAnimation;

    // names of the current event, last event, and all of the events
    [HideInInspector] public string currentEvent;
    private string lastEvent;
    [HideInInspector] public string bankShotEvent = "BANK SHOT", rampShotEvent = "RAMP SHOT", targetEvent = "TARGET";

    // array of all of the events
    private List <string> events = new List<string>();

    // positions and rotations for our camera
    [SerializeField] private Transform defaultCameraPosRot, rampShotEventCameraPosRot;

    // the speed for our camera to move
    private float cameraMoveSpeed = 50f;

    // FOVS for our camera
    private float currentCameraFOV;
    private float defaultCameraFOV = 60f;
    private float bankShotEventCameraFOV = 72.8f;

    // speed to change the fov of our camera
    private float cameraFOVChangeSpeed = 26.5f;

    // array for all the bank shot boards and the material for those boards
    [SerializeField] private GameObject[] bankShotBoards;
    [SerializeField] private Material bankShotBoardMat;

    // alphas for the bank shot boards material
    private float bankShotBoardMatCurrentAlpha = 0.01f;
    private float bankShotBoardMatStartAlpha = 0.01f;
    private float bankShotBoardMatNormalAlpha = 1f;

    // if we did the bank shot
    [HideInInspector] public bool didBankShot;

    // our ramp, different meshes for the partially transparant ramp and the solid ramp, and material for the ramp
    [SerializeField] private GameObject ramp;
    [SerializeField] private MeshRenderer rampRenderer, rampFullRenderer;
    [SerializeField] private Material rampMat;

    // alphas for the ramp's material
    private float rampMatCurrentAlpha = 0.01f;
    private float rampMatStartAlpha = 0.01f;
    private float rampMatNormalAlpha = 1f;

    // timer for throwing during the ramp event
    [HideInInspector] public float currentRampThrowTime;
    [HideInInspector] public float rampThrowTimeThreshold = 0.7f;
    [HideInInspector] public bool addRampThrowTime;

    // target prefab and variables for the creation of targets during the target event
    [SerializeField] private GameObject targetPrefab;
    private GameObject instantiatedTargetPrefab1, instantiatedTargetPrefab2, instantiatedTargetPrefab3;
    private int randCupTargetNumber1, randCupTargetNumber2, randCupTargetNumber3;
    private bool added1STTarget, added2NDTarget, added3RDTarget;

    // materials for the targets
    [SerializeField] private Material targetRedMat, targetWhiteMat;

    // alphas for the target's materials
    private float targetRedWhiteMatCurrentAlpha = 0.01f;
    private float targetRedWhiteMatStartAlpha = 0.01f;
    private float targetRedWhiteMatNormalAlpha = 1f;

    // bool to know if we hit a target
    [HideInInspector] public bool hitTarget;

    // the speed that we change the alpha of materials
    private float matAlphaChangeSpeed = 70f;

    // bool for fading target materials
    private bool targetRedWhiteMatColorDoneGoingAway = true;

    // if the party gamemode tutorial is active
    [HideInInspector] public bool partyGamemodeTutorialActive;

    // different mixers for audio
    [SerializeField] private AudioMixer musicMixer, soundEffectsMixer;

    // if we have won or lost the game
    [HideInInspector] public bool loseGameOver, winGameOver;

    // components
    private LanguageManager languageManager;

    private void Awake()
    {
        // instance that can be accessed from anywhere in our code
        if (instance == null)
            instance = this;

        // the name of our scene
        sceneName = SceneManager.GetActiveScene().name;

        // if we are in the party gamemode
        if (sceneName == SceneNames.PARTY_GAMEMODE_SCENE_NAME)
        {   
            // add all the events
            events.Add(bankShotEvent);
            events.Add(rampShotEvent);
            events.Add(targetEvent);

            // if we have finished the party gamemode tutorial before
            if (DataManager.instance.GetBool(DataManager.instance.partyGamemodeTutorialDoneData))
            {   
                // new event
                currentEvent = events[Random.Range(0, events.Count)];
                lastEvent = currentEvent;
            }
            else
                partyGamemodeTutorialActive = true; // the tutorial is active
            
            // if the tutorial is not active
            if (!partyGamemodeTutorialActive)
            {   
                // SET MATERIAL ALPHA COLOR
                bankShotBoardMat.color = new Color(bankShotBoardMat.color.r,
                    bankShotBoardMat.color.g, bankShotBoardMat.color.b, bankShotBoardMatCurrentAlpha);

                // SET MATERIAL ALPHA COLOR
                rampMat.color = new Color(rampMat.color.r,
                    rampMat.color.g, rampMat.color.b, rampMatCurrentAlpha);

                // SET MATERIALS ALPHA COLOR
                targetRedMat.color = new Color(targetRedMat.color.r,
                    targetRedMat.color.g, targetRedMat.color.b, targetRedWhiteMatCurrentAlpha);
                targetWhiteMat.color = new Color(targetWhiteMat.color.r,
                    targetWhiteMat.color.g, targetWhiteMat.color.b, targetRedWhiteMatCurrentAlpha);
            }
        }

        // go through every ball that we have
        for (int i = 0; i < balls.Length; i++)
        {
            // if the ball is our equipped ball
            if (balls[i].GetComponent<BallName>().ballName == ShopInventoryDataManager.instance.
                GetString(ShopInventoryDataManager.instance.equipped_BALL_STRING_DATA))
            {
                // set the initial ball rotation
                initialBallRotation = balls[i].transform.rotation;

                // variable for our spawned ball
                GameObject instantiatedBall;

                // switch based on scene name
                switch (sceneName)
                {
                    // spawn ball for classic gamemode
                    case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                        instantiatedBall = Instantiate(balls[i], ballSpawnPos.position, initialBallRotation);
                        break;
                    
                    // spawn ball for party gamemode
                    case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                        if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent) {
                            instantiatedBall = Instantiate(balls[i], ballSpawnPos.position, initialBallRotation);
                        }
                        else {
                            instantiatedBall = Instantiate(balls[i],rampShotEventBallSpawnPosition.position, initialBallRotation);
                            BallRespawnManager.instance.canRespawn = false;
                            BallHover.instance.hover = false;
                            instantiatedBall.GetComponent<Rigidbody>().useGravity = true;
                        }
                        break;
                }
            }
        }

        // go through every cup group
        for (int i = 0; i < cupGroups.Length; i++)
        {
            // check if it is the equipped cup group
            if (cupGroups[i].GetComponent<CupName>().cupName == ShopInventoryDataManager.instance.
                GetString(ShopInventoryDataManager.instance.equipped_CUP_STRING_DATA))
            {
                // activate the cup group
                cupGroups[i].SetActive(true);
                activeCupGroup = cupGroups[i];
            }
        }

        // go through all the cups in the cup group
        for (int i = 0; i < 10; i++)
        {
            // add every cup to the cupsList and set their position
            cupsList.Add(activeCupGroup.transform.GetChild(i).gameObject);
            cupsList[i].GetComponent<Rigidbody>().position = startingCupPositions[i].position;
        }

        // set the cups left and misses left
        cupsLeft = initialCupsLeft;
        missesLeft = initialMissesLeft;

        // setting the default values
        totalCoinsEarnedFromCups = 0;
        totalCoinsRemovedFromMisses = 0;
        coinsEarned = 0;

        // initialize the streak text
        streakTextString = streakText.text;
        streakTextAnimation = streakText.GetComponent<StreakTextAnimation>();

        // get the language manager
        languageManager = GameObject.Find(ObjectNames.LanguageManager_NAME).GetComponent<LanguageManager>();

        // extra initialization for streak text
        if (languageManager.language == languageManager.english)
            updatedStreakTextString = true;
        else
            updatedStreakTextString = false;
    }

    private void Start()
    {
        // get the volume data for the music mixer and sound effects mixer
        float savedMusicData = DataManager.instance.GetFloat(DataManager.instance.musicVolumeData);
        float savedSoundEffectsData = DataManager.instance.GetFloat(DataManager.instance.soundEffectsVolumeData);

        // set the volume data for the music mixer and sound effects mixer
        musicMixer.SetFloat(MixerParameters.MIXER_VOLUME_PARAMETER_NAME, savedMusicData);
        soundEffectsMixer.SetFloat(MixerParameters.MIXER_VOLUME_PARAMETER_NAME, savedSoundEffectsData);
    }

    private void Update()
    {
        // reposition six cups
        if (cupsLeft <= 6 && cupsLeft > 3 && !sixCupsDoneRepositioning) {
            RepositionCupsAtSixLeft();
        }
        // reposition three cups
        else if (cupsLeft <= 3 && !threeCupsDoneRepositioning) {
            RepositionCupsAtThreeLeft();
        }

        // reset the bools when we are done repositioning cups
        if (sixCupsDoneRepositioning || threeCupsDoneRepositioning)
        {
            cup1Done = false;
            cup2Done = false;
            cup3Done = false;
            cup4Done = false;
            cup5Done = false;
            cup6Done = false;
        }

        // if all the cups are gone we win
        if (cupsLeft <= 0)
            winGameOver = true;
        else if (missesLeft <= 0) // if we lose all of our misses we lose
            loseGameOver = true;

        // update our streak text if needed
        if (streakTextString != streakText.text && !updatedStreakTextString)
        {
            streakTextString = streakText.text;
            updatedStreakTextString = true;
        }

        // if needed make sure our streak text can't switch languages
        if (!disabledStreakTextLanguageChange)
        {
            streakText.GetComponent<TextLanguageController>().enabled = false;
            disabledStreakTextLanguageChange = true;
        }

        // if we are in the party gamemode and not paused
        if (sceneName == SceneNames.PARTY_GAMEMODE_SCENE_NAME)
        {
            // if the tutorial is not active
            if (!partyGamemodeTutorialActive) {
                HandleBankShotBoards();
                HandleBankShotEventCameraFOV();
                HandleRampShotEvent();
            }

            // switch based on current event
            // handle the switching to and off the target event
            bool caseFound = false;
            switch (currentEvent) {
                case "TARGET":
                    DetermineTargetForTargetEvent();
                    caseFound = true;
                    break;
            }
            if (!caseFound) RemoveTargetForTargetEvent();
        }
    }

    void RepositionCupsAtSixLeft()
    {   
        // move the cup via it's rigidbody and say when it's done with a bool
        Rigidbody cupBody0 = cupsList[0].GetComponent<Rigidbody>();
        if (Vector3.Distance(cupsList[0].transform.position, sixCupRepositionPositions[0].position) > 0.01f)
        {
            Vector3 pos0 =  cupsList[0].transform.position = Vector3.MoveTowards(cupsList[0].transform.position,
                sixCupRepositionPositions[0].position, repositionSpeed * Time.deltaTime);

            cupBody0.position = pos0;
        }
        else {
            cup1Done = true;
        }

        // move the cup via it's rigidbody and say when it's done with a bool
        if (cupsList.Count >= 2)
        {
            Rigidbody cupBody1 = cupsList[1].GetComponent<Rigidbody>();
            if (Vector3.Distance(cupsList[1].transform.position, sixCupRepositionPositions[1].position) > 0.01f) {
                Vector3 pos1 =  cupsList[1].transform.position = Vector3.MoveTowards(cupsList[1].transform.position,
                    sixCupRepositionPositions[1].position, repositionSpeed * Time.deltaTime);

                cupBody1.position = pos1;
            }
            else {
                cup2Done = true;
            }
        }
        else {
            cup2Done = true;
        }

        // move the cup via it's rigidbody and say when it's done with a bool
        if (cupsList.Count >= 3)
        {
            Rigidbody cupBody2 = cupsList[2].GetComponent<Rigidbody>();
            if (Vector3.Distance(cupsList[2].transform.position, sixCupRepositionPositions[2].position) > 0.01f) {
                Vector3 pos2 =  cupsList[2].transform.position = Vector3.MoveTowards(cupsList[2].transform.position,
                    sixCupRepositionPositions[2].position, repositionSpeed * Time.deltaTime);

                cupBody2.position = pos2;
            }
            else {
                cup3Done = true;
            }
        }
        else {
            cup3Done = true;
        }

        // move the cup via it's rigidbody and say when it's done with a bool
        if (cupsList.Count >= 4)
        {
            Rigidbody cupBody3 = cupsList[3].GetComponent<Rigidbody>();
            if (Vector3.Distance(cupsList[3].transform.position, sixCupRepositionPositions[3].position) > 0.01f) {
                Vector3 pos3 =  cupsList[3].transform.position = Vector3.MoveTowards(cupsList[3].transform.position,
                    sixCupRepositionPositions[3].position, repositionSpeed * Time.deltaTime);

                cupBody3.position = pos3;
            }
            else {
                cup4Done = true;
            }
        }
        else {
            cup4Done = true;
        }

        // move the cup via it's rigidbody and say when it's done with a bool
        if (cupsList.Count >= 5)
        {
            Rigidbody cupBody4 = cupsList[4].GetComponent<Rigidbody>();
            if (Vector3.Distance(cupsList[4].transform.position, sixCupRepositionPositions[4].position) > 0.01f) {
                Vector3 pos4 =  cupsList[4].transform.position = Vector3.MoveTowards(cupsList[4].transform.position,
                    sixCupRepositionPositions[4].position, repositionSpeed * Time.deltaTime);

                cupBody4.position = pos4;
            }
            else {
                cup5Done = true;
            }
        }
        else {
            cup5Done = true;
        }

        // move the cup via it's rigidbody and say when it's done with a bool
        if (cupsList.Count >= 6)
        {
            Rigidbody cupBody5 = cupsList[5].GetComponent<Rigidbody>();
            if (Vector3.Distance(cupsList[5].transform.position, sixCupRepositionPositions[5].position) > 0.01f) {
                Vector3 pos5 =  cupsList[5].transform.position = Vector3.MoveTowards(cupsList[5].transform.position,
                    sixCupRepositionPositions[5].position, repositionSpeed * Time.deltaTime);

                cupBody5.position = pos5;
            }
            else {
                cup6Done = true;
            }
        }
        else {
            cup6Done = true;
        }

        // if all the cups are done repositioning
        if (cup1Done && cup2Done && cup3Done && cup4Done && cup5Done && cup6Done)
        {
            sixCupsDoneRepositioning = true;
        }

        // set the target's position
        if (instantiatedTargetPrefab1 != null)
        {
            instantiatedTargetPrefab1.transform.position = new Vector3
                (instantiatedTargetPrefab1.transform.parent.transform.position.x - 0.012f,
                instantiatedTargetPrefab1.transform.parent.transform.position.y + 2.595f,
                instantiatedTargetPrefab1.transform.parent.transform.position.z + 0.002f);
        }

        // set the target's position
        if (instantiatedTargetPrefab2 != null)
        {
            instantiatedTargetPrefab2.transform.position = new Vector3
                (instantiatedTargetPrefab2.transform.parent.transform.position.x - 0.012f,
                instantiatedTargetPrefab2.transform.parent.transform.position.y + 2.595f,
                instantiatedTargetPrefab2.transform.parent.transform.position.z + 0.002f);
        }

        // set the target's position
        if (instantiatedTargetPrefab3 != null)
        {
            instantiatedTargetPrefab3.transform.position = new Vector3
                (instantiatedTargetPrefab3.transform.parent.transform.position.x - 0.012f,
                instantiatedTargetPrefab3.transform.parent.transform.position.y + 2.595f,
                instantiatedTargetPrefab3.transform.parent.transform.position.z + 0.002f);
        }
    }

    void RepositionCupsAtThreeLeft()
    {
        // move the cup via it's rigidbody and say when it's done with a bool
        Rigidbody cupBody0 = cupsList[0].GetComponent<Rigidbody>();
        if (Vector3.Distance(cupsList[0].transform.position, sixCupRepositionPositions[0].position) > 0.01f)
        {
            Vector3 pos0 =  cupsList[0].transform.position = Vector3.MoveTowards(cupsList[0].transform.position,
                sixCupRepositionPositions[0].position, repositionSpeed * Time.deltaTime);

            cupBody0.position = pos0;
        }
        else {
            cup1Done = true;
        }
        
        // move the cup via it's rigidbody and say when it's done with a bool
        if (cupsList.Count >= 2)
        {
            Rigidbody cupBody1 = cupsList[1].GetComponent<Rigidbody>();
            if (Vector3.Distance(cupsList[1].transform.position, sixCupRepositionPositions[1].position) > 0.01f) {
                Vector3 pos1 =  cupsList[1].transform.position = Vector3.MoveTowards(cupsList[1].transform.position,
                    sixCupRepositionPositions[1].position, repositionSpeed * Time.deltaTime);

                cupBody1.position = pos1;
            }
            else {
                cup2Done = true;
            }
        }
        else {
            cup2Done = true;
        }

        // move the cup via it's rigidbody and say when it's done with a bool
        if (cupsList.Count >= 3)
        {
            Rigidbody cupBody2 = cupsList[2].GetComponent<Rigidbody>();
            if (Vector3.Distance(cupsList[2].transform.position, sixCupRepositionPositions[2].position) > 0.01f) {
                Vector3 pos2 =  cupsList[2].transform.position = Vector3.MoveTowards(cupsList[2].transform.position,
                    sixCupRepositionPositions[2].position, repositionSpeed * Time.deltaTime);

                cupBody2.position = pos2;
            }
            else {
                cup3Done = true;
            }
        }
        else {
            cup3Done = true;
        }

        // if all cups are done repositioning
        if (cup1Done && cup2Done && cup3Done)
        {
            threeCupsDoneRepositioning = true;
        }

        // set the target's position
        if (instantiatedTargetPrefab1 != null)
        {
            instantiatedTargetPrefab1.transform.position = new Vector3
                (instantiatedTargetPrefab1.transform.parent.transform.position.x - 0.012f,
                instantiatedTargetPrefab1.transform.parent.transform.position.y + 2.595f,
                instantiatedTargetPrefab1.transform.parent.transform.position.z + 0.002f);
        }

        // set the target's position
        if (instantiatedTargetPrefab2 != null)
        {
            instantiatedTargetPrefab2.transform.position = new Vector3
                (instantiatedTargetPrefab2.transform.parent.transform.position.x - 0.012f,
                instantiatedTargetPrefab2.transform.parent.transform.position.y + 2.595f,
                instantiatedTargetPrefab2.transform.parent.transform.position.z + 0.002f);
        }

        // set the target's position
        if (instantiatedTargetPrefab3 != null)
        {
            instantiatedTargetPrefab3.transform.position = new Vector3
                (instantiatedTargetPrefab3.transform.parent.transform.position.x - 0.012f,
                instantiatedTargetPrefab3.transform.parent.transform.position.y + 2.595f,
                instantiatedTargetPrefab3.transform.parent.transform.position.z + 0.002f);
        }
    }

    void HandleBankShotBoards()
    {
        // if we are on the bank shot event and the bank shot boards aren't fully solid
        if (currentEvent == bankShotEvent && bankShotBoardMat.color.a < bankShotBoardMatNormalAlpha)
        {
            // get the alpha
            bankShotBoardMatCurrentAlpha = bankShotBoardMat.color.a;
            // if the boards aren't active activate them
            if (!bankShotBoards[0].activeInHierarchy)
            {
                for (int i = 0; i < bankShotBoards.Length; i++)
                    bankShotBoards[i].SetActive(true);
            }

            // add to the alpha and set the alpha
            bankShotBoardMatCurrentAlpha += matAlphaChangeSpeed / 100 * Time.deltaTime;
            bankShotBoardMat.color = new Color(bankShotBoardMat.color.r,
                bankShotBoardMat.color.g, bankShotBoardMat.color.b, bankShotBoardMatCurrentAlpha);
        }

        // if were not on the bank shot event and the boards aren't invisible
        if (currentEvent != bankShotEvent && bankShotBoardMat.color.a > bankShotBoardMatStartAlpha)
        {
            // get the alpha and start decreasing it
            bankShotBoardMatCurrentAlpha = bankShotBoardMat.color.a;
            bankShotBoardMatCurrentAlpha -= matAlphaChangeSpeed / 28 * Time.deltaTime;

            // if the boards are invisible disable them
            if (bankShotBoardMatCurrentAlpha < bankShotBoardMatStartAlpha)
            {
                bankShotBoardMatCurrentAlpha = bankShotBoardMatStartAlpha;
                for (int i = 0; i < bankShotBoards.Length; i++)
                    bankShotBoards[i].SetActive(false);
            }

            // set the alpha
            bankShotBoardMat.color = new Color(bankShotBoardMat.color.r,
                bankShotBoardMat.color.g, bankShotBoardMat.color.b, bankShotBoardMatCurrentAlpha);
        }
    }

    void HandleBankShotEventCameraFOV()
    {   
        // get the current camera FOV
        currentCameraFOV = cinemachineCam.m_Lens.FieldOfView;

        // see if we need to increase or decrease the FOV
        if (currentEvent == bankShotEvent && currentCameraFOV < bankShotEventCameraFOV)
            currentCameraFOV += cameraFOVChangeSpeed * Time.deltaTime;
        else if (currentEvent != bankShotEvent && currentCameraFOV > defaultCameraFOV)
            currentCameraFOV -= cameraFOVChangeSpeed * Time.deltaTime;

        // set the camera fov
        cinemachineCam.m_Lens.FieldOfView = currentCameraFOV;
    }

    void HandleRampShotEvent()
    {
        // add to the timer if needed
        if (addRampThrowTime)
            currentRampThrowTime += Time.deltaTime;

        // move the camera to the ramp shot event camera position
        if (currentEvent == rampShotEvent && Vector3.Distance(cinemachineCam.transform.position, rampShotEventCameraPosRot.position) > 0.01f)
        {
            cinemachineCam.transform.position = Vector3.MoveTowards(cinemachineCam.transform.position,
                rampShotEventCameraPosRot.position, cameraMoveSpeed * Time.deltaTime);
        }
        else if (currentEvent != rampShotEvent && Vector3.Distance(cinemachineCam.transform.position, defaultCameraPosRot.position) > 0.01f) {
            // move the camera back to the default position
            cinemachineCam.transform.position = Vector3.MoveTowards(cinemachineCam.transform.position,
                defaultCameraPosRot.position, cameraMoveSpeed * Time.deltaTime);
        }

        // if we are on the ramp shot event
        if (currentEvent == rampShotEvent)
        {
            // enable the ramp
            ramp.SetActive(true);
        }

        // get the alpha
        rampMatCurrentAlpha = rampMat.color.a;
        if (currentEvent == rampShotEvent && rampMatCurrentAlpha < rampMatNormalAlpha)
        {
            // IF WE ARE IN THE RAMP SHOT EVENT AND THE RAMP ISN'T SOLID
            // increase the alpha when it is full enable the rampFullRenderer
            rampMatCurrentAlpha += matAlphaChangeSpeed / 15 * Time.deltaTime;
            if (rampMatCurrentAlpha >= rampMatNormalAlpha) {
                rampRenderer.enabled = false;
                rampFullRenderer.enabled = true;
            }
        }
        else if (currentEvent != rampShotEvent && rampMatCurrentAlpha > rampMatStartAlpha) {
            // IF WE AREN'T IN THE RAMP SHOT EVENT AND IT ISN'T FULLY TRANSPARANT
            // enable the rampRenderer and start decreasing alpha
            if (rampFullRenderer.enabled) {
                rampRenderer.enabled = true;
                rampFullRenderer.enabled = false;
            }
            rampMatCurrentAlpha -= matAlphaChangeSpeed / 15 * Time.deltaTime;
        }
        else if (currentEvent != rampShotEvent && rampMatCurrentAlpha <= rampMatStartAlpha) {
            // IF WE AREN'T IN THE RAMP SHOT EVENT AND IT IS FULLY TRANSPARANT
            // disable the ramp when fully transparent
            ramp.SetActive(false);
        }

        // set the alpha
        rampMat.color = new Color(rampMat.color.r,
                rampMat.color.g, rampMat.color.b, rampMatCurrentAlpha);
    }

    void DetermineTargetForTargetEvent()
    {   
        // set hit target to false if we have not added targets
        if (!added1STTarget)
            hitTarget = false;

        // determining the cup that is a target and spawning the visual target
        if (cupsLeft >= 1 && !added1STTarget)
        {
            randCupTargetNumber1 = Random.Range(0, cupsList.Count);
            cupsList[randCupTargetNumber1].transform.GetChild(3).GetComponent<CupCollector>().isTarget = true;

            instantiatedTargetPrefab1 = Instantiate(targetPrefab,
                new Vector3(cupsList[randCupTargetNumber1].transform.position.x - 0.012f,
                cupsList[randCupTargetNumber1].transform.position.y + 2.595f,
                cupsList[randCupTargetNumber1].transform.position.z + 0.002f), Quaternion.identity);

            instantiatedTargetPrefab1.transform.parent = cupsList[randCupTargetNumber1].transform;
            added1STTarget = true;
        }

        // determining the cup that is a target and spawning the visual target
        if (cupsLeft >= 2 && !added2NDTarget)
        {
            randCupTargetNumber2 = Random.Range(0, cupsList.Count);
            while(randCupTargetNumber2 == randCupTargetNumber1) {
                randCupTargetNumber2 = Random.Range(0, cupsList.Count);
            }

            cupsList[randCupTargetNumber2].transform.GetChild(3).GetComponent<CupCollector>().isTarget = true;

            instantiatedTargetPrefab2 = Instantiate(targetPrefab,
                new Vector3(cupsList[randCupTargetNumber2].transform.position.x - 0.012f,
                cupsList[randCupTargetNumber2].transform.position.y + 2.595f,
                cupsList[randCupTargetNumber2].transform.position.z + 0.002f), Quaternion.identity);

            instantiatedTargetPrefab2.transform.parent = cupsList[randCupTargetNumber2].transform;
            added2NDTarget = true;
        }

        // determining the cup that is a target and spawning the visual target
        if (cupsLeft >= 3 && !added3RDTarget)
        {
            randCupTargetNumber3 = Random.Range(0, cupsList.Count);
            while (randCupTargetNumber3 == randCupTargetNumber2 || randCupTargetNumber3 == randCupTargetNumber1) {
                randCupTargetNumber3 = Random.Range(0, cupsList.Count);
            }

            cupsList[randCupTargetNumber3].transform.GetChild(3).GetComponent<CupCollector>().isTarget = true;

            instantiatedTargetPrefab3 = Instantiate(targetPrefab,
                new Vector3(cupsList[randCupTargetNumber3].transform.position.x - 0.012f,
                cupsList[randCupTargetNumber3].transform.position.y + 2.595f,
                cupsList[randCupTargetNumber3].transform.position.z + 0.002f), Quaternion.identity);

            instantiatedTargetPrefab3.transform.parent = cupsList[randCupTargetNumber3].transform;
            added3RDTarget = true;
        }

        // get the alpha and add to it until fully solid
        targetRedWhiteMatCurrentAlpha = targetRedMat.color.a;
        if (targetRedWhiteMatCurrentAlpha < targetRedWhiteMatNormalAlpha)
        {
            targetRedWhiteMatCurrentAlpha += matAlphaChangeSpeed / 12 * Time.deltaTime;
        }

        // set the alpha values
        targetRedMat.color = new Color(targetRedMat.color.r, targetRedMat.color.g,
            targetRedMat.color.b, targetRedWhiteMatCurrentAlpha);
        targetWhiteMat.color = new Color(targetWhiteMat.color.r, targetWhiteMat.color.g,
            targetWhiteMat.color.b, targetRedWhiteMatCurrentAlpha);
    }

    void RemoveTargetForTargetEvent()
    {   
        // get the alpha and subtract from it until fully transparant
        targetRedWhiteMatCurrentAlpha = targetRedMat.color.a;
        if (instantiatedTargetPrefab1 && targetRedWhiteMatCurrentAlpha > targetRedWhiteMatStartAlpha)
        {
            targetRedWhiteMatColorDoneGoingAway = false;
            targetRedWhiteMatCurrentAlpha -= matAlphaChangeSpeed / 10 * Time.deltaTime;
        }
        else if (targetRedWhiteMatCurrentAlpha <= targetRedWhiteMatStartAlpha)
        {
            targetRedWhiteMatCurrentAlpha = targetRedWhiteMatStartAlpha;
            targetRedWhiteMatColorDoneGoingAway = true;
        }

        // set the alpha values
        targetRedMat.color = new Color(targetRedMat.color.r, targetRedMat.color.g,
            targetRedMat.color.b, targetRedWhiteMatCurrentAlpha);
        targetWhiteMat.color = new Color(targetWhiteMat.color.r, targetWhiteMat.color.g,
            targetWhiteMat.color.b, targetRedWhiteMatCurrentAlpha);

        // get all the CupCollectors and set them to not be targets
        CupCollector[] cupCollectors = FindObjectsOfType<CupCollector>();
        for (int i = 0; i < cupCollectors.Length; i++)
            cupCollectors[i].isTarget = false;

        // reset target variables and destroy the visual target
        if (added1STTarget && targetRedWhiteMatColorDoneGoingAway)
        {
            randCupTargetNumber1 = 0;

            Destroy(instantiatedTargetPrefab1);
            instantiatedTargetPrefab1 = null;
            added1STTarget = false;
        }
        
        // reset target variables and destroy the visual target
        if (added2NDTarget && targetRedWhiteMatColorDoneGoingAway)
        {
            randCupTargetNumber2 = 0;

            Destroy(instantiatedTargetPrefab2);
            instantiatedTargetPrefab2 = null;
            added2NDTarget = false;
        }

        // reset target variables and destroy the visual target
        if (added3RDTarget && targetRedWhiteMatColorDoneGoingAway)
        {
            randCupTargetNumber3 = 0;

            Destroy(instantiatedTargetPrefab3);
            instantiatedTargetPrefab3 = null;
            added3RDTarget = false;
        }
    }

    public void NewEvent()
    {
        // this is preventing a bug
        // set didBankShot to false
        didBankShot = false;

        // generate a new event until it isn't the same event as last time
        while (currentEvent == lastEvent)
        {
            int randNumber = Random.Range(0, events.Count);
            currentEvent = events[randNumber];
        }
        // set the last event
        lastEvent = currentEvent;
    }

    public void AddEarnedCoins()
    {
        // generate a random amount of coins;
        float randCoins = Random.Range(coinsMinEarnedFromCup, coinsMaxEarnedFromCup);

        // switch based on the current event
        // multiply the coins differently for each event
        switch (currentEvent)
        {
            case "BANK SHOT":
                randCoins *= 1.15f;
                break;

            case "RAMP SHOT":
                randCoins *= 1.15f;
                break;

            case "TARGET":
                randCoins *= 1.3f;
                break;
        }

        // if we have a streak of 2 or greater multiply the coins
        if (streakCount >= 2)
        {
            randCoins = randCoins + streakCount * 2.2f;
        }

        // set our coinsEarned from the current round
        coinsEarned += (int)randCoins;

        // for stats
        totalCoinsEarnedFromCups += (int)randCoins;

        // update text
        UpdateCoinsEarnedText();
    }

    public void RemoveEarnedCoins()
    {
        // generate a random amount of coins to subtract
        int randCoins = Random.Range(coinsMinRemovedFromMiss, coinsMaxRemovedFromMiss);

        // subtract from our earned coins
        coinsEarned -= randCoins;

        // for stats
        totalCoinsRemovedFromMisses += randCoins;

        // so our totalCoinsRemovedFromMisses stat cannot go higher
        // than the totalCoinsEarnedFromCups stat
        if (totalCoinsRemovedFromMisses > totalCoinsEarnedFromCups)
            totalCoinsRemovedFromMisses = totalCoinsEarnedFromCups;

        // we don't want our coinsEarned to go below zero
        if (coinsEarned < 0)
            coinsEarned = 0;

        // update text
        UpdateCoinsEarnedText();
    }

    void UpdateCoinsEarnedText()
    {
        // create a new string builder and clear it
        StringBuilder sb = new StringBuilder();
        sb.Clear();

        // set the text
        sb.Append(coinsEarned);
        coinsEarnedText.text = sb.ToString();
    }

    public void AddToStreak()
    {
        // add one to the streak
        streakCount++;

        // if the streak count is 2 or greater
        if(streakCount >= 2)
        {
            // update the text and animate the text
            UpdateStreakText();
            streakTextAnimation.InitiateAnim();

            // if the streak sound is playing return
            if (SoundManager.instance.streakSound.isPlaying)
                return;

            // play different variations of the same sound for different streak counts
            if (streakCount == 2)
                SoundManager.instance.PlayStreakSound(0.6f, 0.027f);
            else if (streakCount == 3)
                SoundManager.instance.PlayStreakSound(0.8f, 0.038f);
            else if (streakCount >= 4)
                SoundManager.instance.PlayStreakSound(1f, 0.053f);
        }
    }

    public void ResetStreak()
    {
        // reset the streak count to 0
        streakCount = 0;
    }

    void UpdateStreakText()
    {
        // create a string builder and clear it
        StringBuilder sb = new StringBuilder();
        sb.Clear();

        // set the text
        sb.Append(streakTextString + streakCount);
        streakText.text = sb.ToString();
    }

    public void UpdateMissesLeftText()
    {   
        // create a string builder and clear it
        StringBuilder sb = new StringBuilder();
        sb.Clear();

        // set the text
        if (missesLeft > 0)
            sb.Append(missesLeft - 1);
        missesLeftText.text = sb.ToString();
    }
}