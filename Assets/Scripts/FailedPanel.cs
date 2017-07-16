using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailedPanel : MonoBehaviour {
	[SerializeField]
	string m_StageSelectSceneName = "StageSelectTest";
	[SerializeField]
	Text m_ProgressText;

	int m_Progress;
	public int prgress{
		set{
			m_Progress = value;
			m_ProgressText.text = value.ToString () + "%";
		}
	}

	// Use this for initialization
	void Start () {
		
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
