using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailedPanel : MonoBehaviour
{
    [SerializeField]
    Text progressText;

    MainSceneManager sceneManager;

    void Start()
    {
        sceneManager = GameObject.FindObjectOfType<MainSceneManager>();
        progressText.text = sceneManager.Progress + "%";

        progressText.GetComponent<TextFade>().FadeIn(0.5f);
    }

    public void OnRetryButton()
    {
        var scene = SceneManager.GetActiveScene();

        var fade = FadeManager.Instance;
        fade.Transition(0.5f, scene.name);
    }

    public void OnBackButton()
    {
        string stageSelect = GameManager.Instance.StageSelectSceneName;
        var fade = FadeManager.Instance;
        fade.Transition(0.5f, stageSelect);
    }
}
