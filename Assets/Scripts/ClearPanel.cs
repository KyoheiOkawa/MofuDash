using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearPanel : MonoBehaviour
{
    [SerializeField]
    Image[] coins = new Image[3];
    [SerializeField]
    Sprite coinSprite;

    [SerializeField]
    Button nextButton;

    MainSceneManager sceneManager;

    void Start()
    {
        sceneManager = GameObject.FindObjectOfType<MainSceneManager>();

        for (int i = 0; i < coins.Length; i++)
        {
            if (sceneManager.GetCoinState(i))
                coins[i].sprite = coinSprite;
        }

        //次のステージが存在しないまたはアンロックされていない場合ネクストボタンを押せなくする
        var manager = GameManager.Instance;
        int nowStage = manager.NowStageIndex;
        var stageInfos = manager.StageInfo;

        if (nowStage >= stageInfos.Count)
            nextButton.interactable = false;
        else
        {
            var nextStageInfo = stageInfos["Stage" + (nowStage + 1).ToString()];

            if (manager.GetCollectedCoinNum() < nextStageInfo.unlockCoin)
                nextButton.interactable = false;
        }
    }

    public void OnBackButton()
    {
        var stageSelect = GameManager.Instance.StageSelectSceneName;
        var fade = FadeManager.Instance;
        fade.Transition(0.5f, stageSelect);
    }

    public void OnRetryButton()
    {
        var scene = SceneManager.GetActiveScene();

        var fade = FadeManager.Instance;
        fade.Transition(0.5f, scene.name);
    }

    public void OnNextButton()
    {
        var manager = GameManager.Instance;
        var stageInfo = manager.StageInfo;

        int nextIndex = manager.NowStageIndex + 1;

        var next = "Stage" + nextIndex.ToString();
        manager.NowStageIndex = nextIndex;

        var fade = FadeManager.Instance;
        fade.Transition(0.5f, next);
    }
}
