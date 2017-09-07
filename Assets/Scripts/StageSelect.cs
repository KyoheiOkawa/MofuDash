using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    StagePanel stagePanel;

    Locked locked = null;

    int nowSelected = 1;

    bool isLocked = false;

    Dictionary<string, StageInfo> stageInfo;

    void Start()
    {
        GameManager manager = GameManager.Instance;
        stageInfo = manager.StageInfo;
        nowSelected = manager.NowStageIndex;

        SetStageInfo();

        stagePanel.SetCollectedCoinsText();
    }

    public void OnRightButton()
    {
        SoundManager sound = SoundManager.Instance;
        sound.PlaySE("Button");

        if (nowSelected >= stageInfo.Count)
            nowSelected = 1;
        else
            nowSelected++;

        SetStageInfo();
    }

    public void OnLeftButton()
    {
        SoundManager sound = SoundManager.Instance;
        sound.PlaySE("Button");

        if (nowSelected == 1)
            nowSelected = stageInfo.Count;
        else
            nowSelected--;

        SetStageInfo();
    }

    public void OnBackButton()
    {
        var fade = FadeManager.Instance;
        string titleScene = GameManager.Instance.TitleSceneName;
        fade.Transition(0.5f, titleScene);
    }

    public void OnStartButton()
    {
        if (locked)
            return;

        string startScene = "Stage" + nowSelected.ToString();

        var titleSound = TitleSound.Instance;
        titleSound.DestroyOwn();

        var fade = FadeManager.Instance;
        fade.Transition(0.5f, startScene);

        var manager = GameManager.Instance;
        manager.NowStageIndex = nowSelected;
    }

    private void SetStageInfo()
    {
        var manager = GameManager.Instance;

        string nextStage = "Stage" + nowSelected.ToString();
        StageInfo nextStageInfo = stageInfo[nextStage];
        stagePanel.SetDisplayFromStageInfo(nextStageInfo);

        isLocked = nextStageInfo.unlockCoin > manager.GetCollectedCoinNum();

        if (isLocked)
        {
            if (!locked)
            {
                var obj = Instantiate(Resources.Load("Prefabs/Locked") as GameObject, stagePanel.gameObject.GetComponent<Transform>());
                locked = obj.GetComponent<Locked>();
            }

            locked.SetUnlockInfoText(nextStageInfo.unlockCoin);
        }
        else
        {
            if (locked)
            {
                locked.GetComponent<Animator>().SetTrigger("Destroy");
                locked = null;
            }
        }
    }
}
