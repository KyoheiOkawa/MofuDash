using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour {
    [SerializeField]
    string m_TestNextScene = "PlayerTest";//テスト用のシーン遷移先

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartButton()
    {
        SceneManager.LoadScene(m_TestNextScene);
    }
}
