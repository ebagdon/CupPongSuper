using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicTextImageAlignment : MonoBehaviour
{
    // parent transform and anchor
    private RectTransform parentTransform;
    private float parentAnchorY;

    // image and it's transform
    [SerializeField] private Image image;
    private RectTransform imageTransform;

    // text and it's transform
    [SerializeField] private Text text;
    private RectTransform textTransform;

    private void Awake()
    {
        // get the parent transform and set it's y anchor
        parentTransform = gameObject.GetComponent<RectTransform>();
        parentAnchorY = parentTransform.anchoredPosition.y;

        // get the image and text transforms
        imageTransform = image.GetComponent<RectTransform>();
        textTransform = text.GetComponent<RectTransform>();
    }

    public void UpdateAlignmentTotalCupCoinsEarned()
    {
        // based on how many coins we earned from cups set the y anchor
        if (GameManager.instance.totalCoinsEarnedFromCups <= 9)
            parentAnchorY = -11f;
        else if (GameManager.instance.totalCoinsEarnedFromCups >= 10 && GameManager.instance.totalCoinsEarnedFromCups <= 99)
            parentAnchorY = 49.93f;
        else if (GameManager.instance.totalCoinsEarnedFromCups >= 100)
            parentAnchorY = 100.9f;

        // set the anchored position
        parentTransform.anchoredPosition = new Vector2(parentTransform.anchoredPosition.x, parentAnchorY);
    }

    public void UpdateAlignmentTotalMissCoinsRemoved()
    {
        // based on how many coins we lost set the y anchor
        if (GameManager.instance.totalCoinsRemovedFromMisses <= 9)
            parentAnchorY = -11f;
        else if (GameManager.instance.totalCoinsRemovedFromMisses >= 10 && GameManager.instance.totalCoinsRemovedFromMisses <= 99)
            parentAnchorY = 49.93f;
        else if (GameManager.instance.totalCoinsRemovedFromMisses >= 100)
            parentAnchorY = 100.9f;

        // set the anchored position
        parentTransform.anchoredPosition = new Vector2(parentTransform.anchoredPosition.x, parentAnchorY);
    }

    public void UpdateAlignmentTotalCoinsEarned()
    {
        // based on how many coins we earned set the y anchor
        if (GameManager.instance.coinsEarned <= 9)
            parentAnchorY = -11f;
        else if (GameManager.instance.coinsEarned >= 10 && GameManager.instance.coinsEarned <= 99)
            parentAnchorY = 49.93f;
        else if (GameManager.instance.coinsEarned >= 100)
            parentAnchorY = 100.9f;

        // set the anchored position
        parentTransform.anchoredPosition = new Vector2(parentTransform.anchoredPosition.x, parentAnchorY);
    }
}