using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewEnable : MonoBehaviour
{
    // our parent canvas and scrollView
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private ScrollRect scrollView;

    private void Update()
    {
        // if the parent canvas is enabled and the vertical scrolling is disabled then enable it
        if (parentCanvas.enabled && scrollView.vertical == false)
            scrollView.vertical = true;
    }
}