using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    bool isCatched = false;

    [SerializeField]
    MainSceneManager sceneManager;

    [SerializeField]
    Animator animator;

    public bool IsCatched
    {
        get
        {
            return isCatched;
        }
        set
        {
            isCatched = value;

            if (isCatched)
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/catchedcoin");
        }
    }

    void Start()
    {
        if (!sceneManager)
            sceneManager = GameObject.FindObjectOfType<MainSceneManager>();
        if (!animator)
            animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager sound = SoundManager.Instance;
            sound.PlaySE("Coin");

            isCatched = true;

            sceneManager.UpdateCoinChatcedState();

            animator.SetTrigger("Get");
        }
    }
}
