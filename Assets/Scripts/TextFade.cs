using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour {
	[SerializeField,Range(0,1)]
	float m_FadeRange = 1;

	[SerializeField,HideInInspector]
	Text m_Text;

	[SerializeField,HideInInspector]
	Material m_Material;

	void Reset()
	{
		if(!m_Material)
			m_Material = GetComponent<Text> ().material;

		if (!m_Text)
			m_Text = GetComponent<Text> ();
	}

	// Use this for initialization
	void Start () {
		if(!m_Material)
			m_Material = GetComponent<Text> ().material;

		if (!m_Text)
			m_Text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateUniform()
	{
		m_Material.SetFloat ("_Fade", m_FadeRange);
	}

	public void FadeOutIn(float time, string nextStr)
	{
		StopCoroutine ("Fade");
		StartCoroutine(Fade (time, nextStr));
	}

	IEnumerator Fade(float time,string nextStr)
	{
		while (true)
		{
			m_FadeRange -= Time.deltaTime / time;

			if (m_FadeRange <= 0.0f) 
			{
				m_FadeRange = 0;
				UpdateUniform ();
				break;
			}

			UpdateUniform ();

			yield return null;
		}

		m_Text.text = nextStr;

		while (true) 
		{
			m_FadeRange += Time.deltaTime / time;

			if (m_FadeRange >= 1.0f) 
			{
				m_FadeRange = 1.0f;
				UpdateUniform ();
				break;
			}

			UpdateUniform ();

			yield return null;
		}
	}

	#if UNITY_EDITOR
	protected void OnValidate()
	{
		UpdateUniform ();
	}
	#endif
}
