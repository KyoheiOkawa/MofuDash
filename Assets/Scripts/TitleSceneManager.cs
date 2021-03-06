﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleSceneManager : MonoBehaviour
{
    void Start()
    {
#if UNITY_IOS
        if (PlayerPrefs.GetInt("IsFirstStart", 0) == 0)
        {
            PlayerPrefs.SetInt("IsFirstStart", 1);

            string baseFilePath = Application.streamingAssetsPath + "/" + "CSV/StageInfo.csv";
            string filePath = Application.persistentDataPath + "/" + "StageInfo.csv";
            System.IO.File.Copy(baseFilePath, filePath, true);
        }
#endif
    }

    public void ResetIOSSaveData()
    {
        string baseFilePath = Application.streamingAssetsPath + "/" + "CSV/StageInfo.csv";
        string filePath = Application.persistentDataPath + "/" + "StageInfo.csv";
        System.IO.File.Copy(baseFilePath, filePath, true);

        PlayerPrefs.SetInt("IsFirstStart", 0);
    }

    public void OnStartButton()
    {
        var manager = GameManager.Instance;
        string nextName = manager.StageSelectSceneName;

        var fadeManager = FadeManager.Instance;
        fadeManager.Transition(0.5f, nextName);
    }

    public void OnHowToPlayButton()
    {
        var manager = GameManager.Instance;
        string nextName = manager.TutorialSceneName;

        var titleSound = TitleSound.Instance;
        titleSound.DestroyOwn();

        var fadeManager = FadeManager.Instance;
        fadeManager.Transition(0.5f, nextName);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TitleSceneManager))]
    public class TitleSceneManagerEditor : Editor
    {

        /// <summary>
        /// InspectorのGUIを更新
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TitleSceneManager manager = target as TitleSceneManager;

            if (GUILayout.Button("ResetIOSSaveData"))
            {
                Debug.Log("ResetIOSSaveData");
                manager.ResetIOSSaveData();
            }

        }

    }
#endif
}