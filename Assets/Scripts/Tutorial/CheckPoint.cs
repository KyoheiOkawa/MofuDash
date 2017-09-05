using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    TutorialManager tutorialManager = null;

    [SerializeField]
    string message = "";

    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Player"))
        {
            tutorialManager.OnCheckPoint(message);
        }
    }
}
