using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : Graphic
{
	[SerializeField,Range(0,1)]
	float m_Range = 0;

	[SerializeField]
	Texture m_MaskTex = null;

	[SerializeField]
	Texture m_MainTex = null;

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

	public override Texture mainTexture {
		get 
		{
			return m_MainTex;
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
		UpdateUniform ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	protected override void OnPopulateMesh( VertexHelper vh )
	{
		vh.Clear();

		var rTrans = GetComponent<RectTransform> ();

		// 左上
		UIVertex lt = UIVertex.simpleVert;
		lt.position = new Vector3( rTrans.rect.x, rTrans.rect.yMax, 0 );
		lt.uv0 = new Vector2(0, 1);

		// 右上
		UIVertex rt = UIVertex.simpleVert;
		rt.position = new Vector3(rTrans.rect.xMax, rTrans.rect.yMax, 0);
		rt.uv0 = new Vector2 (1, 1);

		// 右下
		UIVertex rb = UIVertex.simpleVert;
		rb.position = new Vector3( rTrans.rect.xMax, rTrans.rect.y, 0 );
		rb.uv0 = new Vector2 (1, 0);

		// 左下
		UIVertex lb = UIVertex.simpleVert;
		lb.position = new Vector3( rTrans.rect.x, rTrans.rect.y, 0 );
		lb.uv0 = new Vector2 (0, 0);


		vh.AddUIVertexQuad( new UIVertex[] {
			lb, rb, rt, lt
		} );
	}

	public void Transition(float time, string transSceneName)
	{
		StartCoroutine (FadeWithSceneChange (time, transSceneName));
	}

	IEnumerator FadeWithSceneChange(float time,string nextSceneName)
	{
		SetRayCastBlock (false);

		while (true)
		{
			m_Range += Time.deltaTime / time;
			UpdateUniform ();

			if (m_Range >= 1)
			{
				//SceneManager.LoadScene (nextSceneName);
				AsyncOperation async = SceneManager.LoadSceneAsync(nextSceneName);
				async.allowSceneActivation = false;

				while (async.progress <= 0.9f) {
					async.allowSceneActivation = true;
					yield return null;
				}

				SetRayCastBlock (false);

				yield return new WaitForSeconds(0.1f);

				m_Range = 1.0f;

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
		material.SetTexture ("_MainTex", m_MainTex);
		material.SetTexture ("_MaskTex", m_MaskTex);
		material.SetFloat ("_Fade", m_Range);
		material.SetColor ("_Color", color);
	}

	#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate ();

		UpdateUniform ();
	}
	#endif
}
