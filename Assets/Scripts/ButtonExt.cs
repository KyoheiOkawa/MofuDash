using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class ButtonExt : MonoBehaviour
{
    [SerializeField]
    StageSelect stageSelect;

    [SerializeField]
    float flickInterval = 0.5f;

    float flickCOunt = 0.0f;

    Vector3 startLocation;

    bool isBegin = false;

    void Update()
    {
        if (isBegin)
            flickCOunt += Time.deltaTime;
    }

    public void OnTouchDown()
    {
        if (!isBegin)
        {
            isBegin = true;
            flickCOunt = 0.0f;
            startLocation = Input.mousePosition;
        }
    }

    public void OnTouchEnded()
    {
        if (flickCOunt <= flickInterval)
        {
            float minLength = 50.0f;

            float slideLen = startLocation.x - Input.mousePosition.x;

            if (Mathf.Abs(slideLen) <= minLength)
            {
                stageSelect.OnStartButton();
                return;
            }

            if (slideLen > 0)
            {
                stageSelect.OnLeftButton();
            }
            else
            {
                stageSelect.OnRightButton();
            }
        }

        isBegin = false;
    }
}
