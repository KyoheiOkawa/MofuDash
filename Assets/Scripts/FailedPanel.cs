﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailedPanel : MonoBehaviour {
	[SerializeField]
	string m_StageSelectSceneName = "StageSelectTest";
	[SerializeField]
	Text m_ProgressText;

	MainSceneManager m_SceneManager;

	// Use this for initialization
	void Start () {
		m_SceneManager = GameObject.FindObjectOfType<MainSceneManager> ();
		m_ProgressText.text = m_SceneManager.progress + "%";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnRetryButton()
	{
		var scene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (scene.name);
	}

	public void OnBackButton()
	{
		SceneManager.LoadScene (m_StageSelectSceneName);
	}
}