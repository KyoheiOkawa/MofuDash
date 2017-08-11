using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour {
	[SerializeField]
	string m_TitleSceneName = "TitleTest";

	[SerializeField]
	StagePanel m_StagePanel;

	Locked m_Locked = null;

	int m_NowSelected = 1;

	bool m_IsRocked = false;

	Dictionary<string,StageInfo> m_StageInfo;

	// Use this for initialization
	void Start () {
		GameManager manager = GameManager.Instance;
		m_StageInfo = manager.stageInfo;

		SetStageInfo ();

		m_StagePanel.SetCollectedCoinsText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnRightButton()
	{
		SoundManager sound = SoundManager.Instance;
		sound.PlaySE ("Button");

		if (m_NowSelected >= m_StageInfo.Count)
			m_NowSelected = 1;
		else
			m_NowSelected++;
		
		SetStageInfo ();
	}

	public void OnLeftButton()
	{
		SoundManager sound = SoundManager.Instance;
		sound.PlaySE ("Button");

		if (m_NowSelected == 1)
			m_NowSelected = m_StageInfo.Count;
		else
			m_NowSelected--;

		SetStageInfo ();
	}

	public void OnBackButton()
	{
		//SceneManager.LoadScene (m_TitleSceneName);
		var fade = FadeManager.Instance;
		string titleScene = GameManager.Instance.titleSceneName;
		fade.Transition (0.5f, titleScene);
	}

	public void OnStartButton()
	{
		if (m_Locked)
			return;

		string startScene = "Stage" + m_NowSelected.ToString ();

		var titleSound = TitleSound.Instance;
		titleSound.DestroyOwn ();

		//SceneManager.LoadScene (startScene);
		var fade = FadeManager.Instance;
		fade.Transition (0.5f, startScene);

		var manager = GameManager.Instance;
		manager.nowStageIndex = m_NowSelected;
	}

	private void SetStageInfo()
	{
		var manager = GameManager.Instance;

		string nextStage = "Stage" + m_NowSelected.ToString ();
		StageInfo nextStageInfo = m_StageInfo [nextStage];
		m_StagePanel.SetDisplayFromStageInfo (nextStageInfo);

		m_IsRocked = nextStageInfo.unlockCoin > manager.GetCollectedCoinNum ();

		if (m_IsRocked) 
		{
			if (!m_Locked)
			{
				var obj = Instantiate (Resources.Load ("Prefabs/Locked")as GameObject, m_StagePanel.gameObject.GetComponent<Transform> ());
				m_Locked = obj.GetComponent<Locked> ();
			}

			m_Locked.SetUnlockInfoText (nextStageInfo.unlockCoin);
		} 
		else 
		{
			if (m_Locked) 
			{
				//Destroy (m_Locked.gameObject);
				m_Locked.GetComponent<Animator>().SetTrigger("Destroy");
				m_Locked = null;
			}
		}
	}
}
