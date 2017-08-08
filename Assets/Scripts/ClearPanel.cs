using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearPanel : MonoBehaviour {
	[SerializeField]
	Image[] m_Coins = new Image[3];
	[SerializeField]
	Sprite m_CoinSprite;

	[SerializeField]
	Button m_NextButton;

	MainSceneManager m_SceneManager;

	// Use this for initialization
	void Start () {
		m_SceneManager = GameObject.FindObjectOfType<MainSceneManager> ();

		for (int i = 0; i < m_Coins.Length; i++) {
			if (m_SceneManager.GetCoinState (i))
				m_Coins [i].sprite = m_CoinSprite;
		}

		//次のステージが存在しないまたはアンロックされていない場合ネクストボタンを押せなくする
		var manager = GameManager.Instance;
		int nowStage = manager.nowStageIndex;
		var stageInfos = manager.stageInfo;

		if (nowStage >= stageInfos.Count)
			m_NextButton.interactable = false;
		else
		{
			var nextStageInfo = stageInfos ["Stage" + (nowStage + 1).ToString ()];

			if (manager.GetCollectedCoinNum () < nextStageInfo.unlockCoin)
				m_NextButton.interactable = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnBackButton()
	{
		var stageSelect = GameManager.Instance.stageSelectSceneName;
		var fade = FadeManager.Instance;
		fade.Transition (0.5f, stageSelect);
	}

	public void OnRetryButton()
	{
		var scene = SceneManager.GetActiveScene ();

		var fade = FadeManager.Instance;
		fade.Transition (0.5f, scene.name);
	}

	public void OnNextButton()
	{
		var manager = GameManager.Instance;
		var stageInfo = manager.stageInfo;

		int nextIndex = manager.nowStageIndex + 1;

		var next = "Stage" + nextIndex.ToString();
		manager.nowStageIndex = nextIndex;

		var fade = FadeManager.Instance;
		fade.Transition (0.5f, next);
	}
}
