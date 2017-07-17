using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour {
	[SerializeField]
	string m_TitleSceneName = "TitleTest";
	[SerializeField]
	StagePanel m_StagePanel;

	int m_NowSelected = 1;

	Dictionary<string,StageInfo> m_StageInfo;

	// Use this for initialization
	void Start () {
		GameManager manager = GameManager.Instance;
		m_StageInfo = manager.stageInfo;

		SetStageInfo ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnRightButton()
	{
		if (m_NowSelected >= m_StageInfo.Count)
			m_NowSelected = 1;
		else
			m_NowSelected++;
		
		SetStageInfo ();
	}

	public void OnLeftButton()
	{
		if (m_NowSelected == 1)
			m_NowSelected = m_StageInfo.Count;
		else
			m_NowSelected--;

		SetStageInfo ();
	}

	public void OnBackButton()
	{
		SceneManager.LoadScene (m_TitleSceneName);
	}

	public void OnStartButton()
	{
		string startScene = "Stage" + m_NowSelected.ToString ();

		var titleSound = TitleSound.Instance;
		titleSound.DestroyOwn ();

		SceneManager.LoadScene (startScene);
	}

	private void SetStageInfo()
	{
		string nextStage = "Stage" + m_NowSelected.ToString ();
		StageInfo nextStageInfo = m_StageInfo [nextStage];
		m_StagePanel.SetDisplayFromStageInfo (nextStageInfo);
	}
}
