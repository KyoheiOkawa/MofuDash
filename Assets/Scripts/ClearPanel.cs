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
	string m_StageSelectSceneName = "StageSelectTest";

	MainSceneManager m_SceneManager;

	// Use this for initialization
	void Start () {
		m_SceneManager = GameObject.FindObjectOfType<MainSceneManager> ();

		for (int i = 0; i < m_Coins.Length; i++) {
			if (m_SceneManager.GetCoinState (i))
				m_Coins [i].sprite = m_CoinSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnBackButton()
	{
		SceneManager.LoadScene (m_StageSelectSceneName);
	}

	public void OnRetryButton()
	{
		var scene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (scene.name);
	}

	public void OnNextButton()
	{

	}
}
