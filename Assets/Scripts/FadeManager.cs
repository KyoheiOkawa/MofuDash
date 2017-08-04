using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : Graphic {
	[SerializeField,Range(0,1)]
	float m_Range = 0;

	static FadeManager m_Instance;

	static public FadeManager Instance
	{
		get
		{
			if (m_Instance == null) 
			{
				m_Instance = (FadeManager)FindObjectOfType (typeof(FadeManager));

				if (m_Instance == null)
				{
					GameObject obj = Instantiate (Resources.Load ("Prefabs/FadeCanvas")as GameObject);
					m_Instance = obj.GetComponent<FadeManager> ();
				}
			}

			return m_Instance;
		}
	}

	void Awake()
	{
		if (this != Instance) {
			Destroy (this);
			return;
		}

		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void WhiteOutTransition(float time, string transSceneName)
	{
		StartCoroutine (FadeWithSceneChange (time, transSceneName));
	}

	IEnumerator FadeWithSceneChange(float time,string nextSceneName)
	{
		while (true)
		{
			m_Range += Time.deltaTime / time;
			UpdateUniform ();

			if (m_Range >= 1)
			{
				//SceneManager.LoadScene (nextSceneName);
				AsyncOperation async = SceneManager.LoadSceneAsync(nextSceneName);

				while (async.progress <= 0.9f)
					yield return null;

				yield return new WaitForEndOfFrame ();

				SetRayCastBlock (false);

				break;
			}

			yield return null;
		}

		while (true)
		{
			m_Range -= Time.deltaTime / time;
			UpdateUniform ();

			if (m_Range <= 0) {
				m_Range = 0;

				SetRayCastBlock (true);
				
				break;
			}

			yield return null;
		}
	}

	/// <summary>
	/// シーン上のCanvasGroupのraycastBlockの値をセットする
	/// </summary>
	/// <param name="b">If set to <c>true</c> b.キャンバス上のボタンなどが反応する</param>
	void SetRayCastBlock(bool b)
	{
		CanvasGroup[] group = GameObject.FindObjectsOfType<CanvasGroup> () as CanvasGroup[];
		foreach (CanvasGroup obj in group)
			obj.blocksRaycasts = b;
	}

	public void UpdateUniform()
	{
		material.SetFloat ("_Fade", m_Range);
	}

	#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate ();

		UpdateUniform ();
	}
	#endif
}
