using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_IOS
		if(PlayerPrefs.GetInt("IsFirstStart",0) == 0){
			PlayerPrefs.SetInt("IsFirstStart",1);

			string baseFilePath = Application.streamingAssetsPath + "/" + "CSV/StageInfo.csv";
			string filePath = Application.persistentDataPath + "/" + "StageInfo.csv";
			System.IO.File.Copy( baseFilePath, filePath);
		}
#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartButton()
    {
		var manager = GameManager.Instance;
		string nextName = manager.stageSelectSceneName;

		var fadeManager = FadeManager.Instance;
		fadeManager.WhiteOutTransition (1.0f, nextName);
    }
}
