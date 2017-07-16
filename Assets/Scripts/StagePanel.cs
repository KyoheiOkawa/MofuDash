using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StagePanel : MonoBehaviour {
	public string m_StageStr = "Stage1";
	public bool[] m_IsGetCoin = new bool[3];
	[Range(0,100)]
	public int m_Progress = 0;

	[SerializeField]
	Sprite m_Coin;
	[SerializeField]
	Sprite m_CoinBW;

	[SerializeField]
	private Text m_StageNameText;
	[SerializeField]
	private Image[] m_CoinImage = new Image[3];
	[SerializeField]
	private Image m_ProgressImage;
	[SerializeField]
	private Text m_ProgressText;
	// Use this for initialization
	void Start () {
		UpdateDisplayInfo ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void UpdateDisplayInfo()
	{
		m_StageNameText.text = m_StageStr;

		for (int i = 0; i < m_CoinImage.Length; i++) {
			if (m_IsGetCoin [i])
				m_CoinImage [i].sprite = m_Coin;
			else
				m_CoinImage [i].sprite = m_CoinBW;
		}

		m_ProgressImage.fillAmount = m_Progress / 100.0f;

		m_ProgressText.text = m_Progress + "%";
	}

	public void SetDisplayFromStageInfo(StageInfo stageInfo)
	{
		m_StageStr = stageInfo.stageName;
		for (int i = 0; i < stageInfo.coin.Length; i++) {
			m_IsGetCoin [i] = stageInfo.coin [i];
		}
		m_Progress = stageInfo.progress;

		UpdateDisplayInfo ();
	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(StagePanel))]
	public class StagePanelEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var sp = target as StagePanel;

			sp.UpdateDisplayInfo ();
		}
	}
	#endif
}
