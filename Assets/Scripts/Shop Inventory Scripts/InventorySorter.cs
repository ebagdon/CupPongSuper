using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySorter : MonoBehaviour
{
    // y positions for spawning the ui
    private float startingY = -570.5f;
    private float spawnY;

    // the difference in y position when we move to a new row
    private float yDifferenceInRows = 934.1f;

    // x positions for spawning the ui
    private float firstX = -303.33f;
    private float secondX = 317.7f;

    // the column we are in
    private int column = 1;

    // the row for the balls
    private int currentBallRow = 1;
    private int ballRow = 1;

    // the row for the cups
    private int currentCupRow = 1;
    private int cupRow = 1;
    
    // variables for the ball skins and cup skins scrolls
    [SerializeField] private RectTransform ballSkinsInventoryScrollContentRect, cupSkinsInventoryScrollContentRect;
    private float ballSkinsInventoryScrollOriginalContentSizeY, cupSkinsInventoryScrollOriginalContentSizeY;
    [SerializeField] private GameObject ballSkinsInventoryScroll;
    [SerializeField] private GameObject cupSkinsInventoryScroll;

    private void Awake()
    {
        // get the original sizes of the inventory ui scrolls
        ballSkinsInventoryScrollOriginalContentSizeY = ballSkinsInventoryScrollContentRect.sizeDelta.y;
        cupSkinsInventoryScrollOriginalContentSizeY = cupSkinsInventoryScrollContentRect.sizeDelta.y;
    }

    public void SortInventory()
    {
        // reset the sizes of the inventory ui scrolls and the starting column that we place ui elements in
        ResetBallRows();
        ResetCupRows();
        column = 1;

        // set thes spawn y
        spawnY = startingY;

        // activate everything in the ball skins inventory scroll
        ballSkinsInventoryScroll.gameObject.SetActive(true);
        foreach (Transform child in ballSkinsInventoryScrollContentRect.transform)
        {
            child.gameObject.SetActive(true);
        }

        // activate everything in the ball skins inventory scroll
        cupSkinsInventoryScroll.gameObject.SetActive(true);
        foreach (Transform child in cupSkinsInventoryScrollContentRect.transform)
        {
            child.gameObject.SetActive(true);
        }

        // get the ball skins and cups skins info
        BallSkinInventoryInfo[] ballSkinInventoryInfoTypes = FindObjectsOfType<BallSkinInventoryInfo>();
        CupSkinInventoryInfo[] cupSkinInventoryInfoTypes = FindObjectsOfType<CupSkinInventoryInfo>();

        // make two lists for all the ball and cup names
        List<string> ballSkinNames = new List<string>();
        List<string> cupSkinNames = new List<string>();

        // go through all the balls' info
        for (int i = 0; i < ballSkinInventoryInfoTypes.Length; i++)
        {
            // if we do not own a ball and it isn't the default ping pong ball
            if (!ShopInventoryDataManager.instance.GetOwnedData(ballSkinInventoryInfoTypes[i].savedFileName) &&
                ballSkinInventoryInfoTypes[i].gameObject.name != UIObjectNames.PING_PONG_BALL_BG_NAME)
            {
                // disable the ui object
                ballSkinInventoryInfoTypes[i].gameObject.SetActive(false);
            }

            // if the ui object is still active add it's ball skin name to the list
            if (ballSkinInventoryInfoTypes[i].gameObject.activeInHierarchy)
                ballSkinNames.Add(ballSkinInventoryInfoTypes[i].ballSkinName);
        }

        // go through all the cups' info
        for (int i = 0; i < cupSkinInventoryInfoTypes.Length; i++)
        {
            // if we do not own a cup and it isn't the default red cup
            if (!ShopInventoryDataManager.instance.GetOwnedData(cupSkinInventoryInfoTypes[i].savedFileName) &&
                cupSkinInventoryInfoTypes[i].gameObject.name != UIObjectNames.RED_CUP_BG_NAME)
            {
                // disable the ui object
                cupSkinInventoryInfoTypes[i].gameObject.SetActive(false);
            }

            // if the ui object is still active add it's cup skin name to the list
            if (cupSkinInventoryInfoTypes[i].gameObject.activeInHierarchy)
                cupSkinNames.Add(cupSkinInventoryInfoTypes[i].cupSkinName);
        }

        // sort the skin names alphabetically
        ballSkinNames.Sort();
        cupSkinNames.Sort();

        // go through all the ball skin names
        for (int a = 0; a < ballSkinNames.Count; a++)
        {   
            // go through all the balls' info
            for (int b = 0; b < ballSkinInventoryInfoTypes.Length; b++) {
                // if the ball skin info name is equal to the ballSkinName
                if (ballSkinInventoryInfoTypes[b].ballSkinName == ballSkinNames[a]) {
                    // if the column is the first column
                    if (column == 1) {
                        // set the ui object's position
                        ballSkinInventoryInfoTypes[b].gameObject.GetComponent<RectTransform>().anchoredPosition =
                            new Vector3(firstX, spawnY, transform.position.z);

                        // handle the row
                        if (currentBallRow < ballRow) {
                            NewBallRow();
                            currentBallRow = ballRow;
                        }

                        // add to the column
                        column++;
                    }
                    else if (column == 2) { // if the column is the second column 
                        // set the ui object's position
                        ballSkinInventoryInfoTypes[b].gameObject.GetComponent<RectTransform>().anchoredPosition =
                            new Vector3(secondX, spawnY, transform.position.z);

                        // reset the column to 1
                        column = 1;

                        // add to the row
                        ballRow++;
                        
                        // set the spawn y
                        spawnY -= yDifferenceInRows;
                    }
                }
            }
        }

        // resets the values before spawning the next set of UI elements
        spawnY = startingY;
        column = 1;

        // go through all the cup skin names
        for (int a = 0; a < cupSkinNames.Count; a++)
        {
            // go through all the cups' info
            for (int b = 0; b < cupSkinInventoryInfoTypes.Length; b++) {
                // if the cup skin info name is equal to the cupSkinName
                if (cupSkinInventoryInfoTypes[b].cupSkinName == cupSkinNames[a]) {
                    // if the column is the first column
                    if (column == 1) {
                        // set the ui object's position
                        cupSkinInventoryInfoTypes[b].gameObject.GetComponent<RectTransform>().anchoredPosition =
                            new Vector3(firstX, spawnY, transform.position.z);

                        // handle the row
                        if (currentCupRow < cupRow) {
                            NewCupRow();
                            currentCupRow = cupRow;
                        }

                        // add to the column
                        column++;
                    }
                    else if (column == 2) { // if the column is the last column
                        // set the ui object's position
                        cupSkinInventoryInfoTypes[b].gameObject.GetComponent<RectTransform>().anchoredPosition =
                            new Vector3(secondX, spawnY, transform.position.z);

                        // reset the column to 1
                        column = 1;

                        // add to the row
                        cupRow++;

                        // set the spawn y
                        spawnY -= yDifferenceInRows;
                    }
                }
            }
        }

        // set all the scrolls' positions
        ballSkinsInventoryScrollContentRect.anchoredPosition = 
            new Vector2(ballSkinsInventoryScrollContentRect.anchoredPosition.x, 0f);
        cupSkinsInventoryScrollContentRect.anchoredPosition = 
            new Vector2(cupSkinsInventoryScrollContentRect.anchoredPosition.x, 0f);

        // disable the cups skins scroll
        cupSkinsInventoryScroll.SetActive(false);
    }

    void ResetBallRows()
    {
        // reset the size of the ball inventory scroll
        ballSkinsInventoryScrollContentRect.sizeDelta = 
            new Vector2(ballSkinsInventoryScrollContentRect.sizeDelta.x, ballSkinsInventoryScrollOriginalContentSizeY);

        currentBallRow = 1;
        ballRow = 1;
    }

    void ResetCupRows()
    {
        // reset the size of the cup inventory scroll
        cupSkinsInventoryScrollContentRect.sizeDelta =
            new Vector2(cupSkinsInventoryScrollContentRect.sizeDelta.x, ballSkinsInventoryScrollOriginalContentSizeY);

        currentCupRow = 1;
        cupRow = 1;
    }

    void NewBallRow()
    {
        // increase the size of the ball skins scroll
        ballSkinsInventoryScrollContentRect.sizeDelta = 
            new Vector2(ballSkinsInventoryScrollContentRect.sizeDelta.x,
            ballSkinsInventoryScrollContentRect.sizeDelta.y + yDifferenceInRows);
    }

    void NewCupRow()
    {
        // increase the size of the cup skins scroll
        cupSkinsInventoryScrollContentRect.sizeDelta = 
            new Vector2(cupSkinsInventoryScrollContentRect.sizeDelta.x,
            cupSkinsInventoryScrollContentRect.sizeDelta.y + yDifferenceInRows);
    }
}