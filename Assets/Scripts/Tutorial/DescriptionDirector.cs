using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionDirector : MonoBehaviour
{
    [SerializeField]
    GameObject descriptionPanel = null;

    [SerializeField]
    Text descriptionText = null;

    [SerializeField]
    GameObject changeColorArrow = null;

    [SerializeField]
    GameObject jumpArrow = null;

    void Start()
    {
        HideChangeColorArrow();
        HideJumpArrow();
    }

    public void ShowPanel()
    {
        descriptionPanel.active = true;
    }

    public void HidePanel()
    {
        descriptionPanel.active = false;
    }

    public void SetDescriptionText(string desc)
    {
        descriptionText.text = desc;
    }

    public void ShowChangeColorArrow()
    {
        changeColorArrow.active = true;
    }

    public void HideChangeColorArrow()
    {
        changeColorArrow.active = false;
    }

    public void ShowJumpArrow()
    {
        jumpArrow.active = true;
    }

    public void HideJumpArrow()
    {
        jumpArrow.active = false;
    }
}
