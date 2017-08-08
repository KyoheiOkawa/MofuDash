using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StagePanel : MonoBehaviour {
	string m_StageStr = "Stage1";
	bool[] m_IsGetCoin = new bool[3];
	[Range(0,100)]
	int m_Progress = 0;

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
	[SerializeField]
	private Text m_CollectedCoinText;

	private float m_BackUpProgress = 0;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetCollectedCoinsText()
	{
		var manager = GameManager.Instance;

		if(m_CollectedCoinText)
			m_CollectedCoinText.text = string.Format ("×{0,00}", manager.GetCollectedCoinNum ());
	}

	private void UpdateDisplayInfo()
	{
		//m_StageNameText.text = m_StageStr;
		m_StageNameText.GetComponent<TextFade>().FadeOutIn(0.25f,m_StageStr);

		StopCoinsAnim ();

		StopAllCoroutines ();
		StartCoroutine (UpdateCoinAnim ());
		StartCoroutine(UpdateBarAnim());
	}

	void StopCoinsAnim()
	{
		for (int i = 0; i < m_CoinImage.Length; i++)
		{
			m_CoinImage [i].GetComponent<Animator> ().SetTrigger ("Stop");
		}
	}

	IEnumerator UpdateCoinAnim()
	{
		for (int i = 0; i < m_CoinImage.Length; i++) {
			if (m_IsGetCoin [i])
				m_CoinImage [i].sprite = m_Coin;
			else
				m_CoinImage [i].sprite = m_CoinBW;

			m_CoinImage [i].GetComponent<Animator> ().SetTrigger ("Change");

			yield return new WaitForSeconds (0.2f);
		}
	}

	IEnumerator UpdateBarAnim()
	{
		float vel = 0;
		while (true) {
			m_BackUpProgress = Mathf.SmoothDamp (m_BackUpProgress, m_Progress, ref vel, 0.5f);

			m_ProgressImage.fillAmount = m_BackUpProgress / 100.0f;
			m_ProgressText.text = string.Format("{0,3}%",(int)m_BackUpProgress);

			if (Mathf.Abs(vel) <= 3.0f) {
				m_ProgressImage.fillAmount = m_Progress / 100.0f;
				m_ProgressText.text = string.Format("{0,3}%",m_Progress);
				m_BackUpProgress = m_Progress;
				break;
			}

			yield return new WaitForEndOfFrame ();
		}
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
