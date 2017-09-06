using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StagePanel : MonoBehaviour
{
    string stageString = "Stage1";

    bool[] hasCoin = new bool[3];

    [Range(0, 100)]
    int progress = 0;

    [SerializeField]
    Sprite coin;
    [SerializeField]
    Sprite coinGray;

    [SerializeField]
    private Text stageNameText;
    [SerializeField]
    private Image[] coinImage = new Image[3];
    [SerializeField]
    private Image progressImage;
    [SerializeField]
    private Text progressText;
    [SerializeField]
    private Text collectedCoinText;

    private float backUpProgress = 0;

    public void SetCollectedCoinsText()
    {
        var manager = GameManager.Instance;

        if (collectedCoinText)
            collectedCoinText.text = string.Format("×{0,00}", manager.GetCollectedCoinNum());
    }

    private void UpdateDisplayInfo()
    {
        stageNameText.GetComponent<TextFade>().FadeAndTextChange(0.25f, stageString);

        StopCoinsAnim();

        StopAllCoroutines();
        StartCoroutine(UpdateCoinAnim());
        StartCoroutine(UpdateBarAnim());
    }

    void StopCoinsAnim()
    {
        for (int i = 0; i < coinImage.Length; i++)
        {
            coinImage[i].GetComponent<Animator>().SetTrigger("Stop");
        }
    }

    IEnumerator UpdateCoinAnim()
    {
        for (int i = 0; i < coinImage.Length; i++)
        {
            if (hasCoin[i])
                coinImage[i].sprite = coin;
            else
                coinImage[i].sprite = coinGray;

            coinImage[i].GetComponent<Animator>().SetTrigger("Change");

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator UpdateBarAnim()
    {
        float vel = 0;
        while (true)
        {
            backUpProgress = Mathf.SmoothDamp(backUpProgress, progress, ref vel, 0.5f);

            progressImage.fillAmount = backUpProgress / 100.0f;
            progressText.text = string.Format("{0,3}%", (int)backUpProgress);

            if (Mathf.Abs(vel) <= 3.0f)
            {
                progressImage.fillAmount = progress / 100.0f;
                progressText.text = string.Format("{0,3}%", progress);
                backUpProgress = progress;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void SetDisplayFromStageInfo(StageInfo stageInfo)
    {

        stageString = stageInfo.stageName;
        for (int i = 0; i < stageInfo.coin.Length; i++)
        {
            hasCoin[i] = stageInfo.coin[i];
        }
        progress = stageInfo.progress;

        UpdateDisplayInfo();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(StagePanel))]
    public class StagePanelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var sp = target as StagePanel;

            sp.UpdateDisplayInfo();
        }
    }
#endif
}
