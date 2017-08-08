using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Reflection;
using System.Linq;

public class ButtonExt : MonoBehaviour {
	[SerializeField]
	StageSelect m_StageSelect;

	[SerializeField]
	float m_FlickInterval = 0.5f;

	float m_FlickCount = 0.0f;

	Vector3 m_StartLoc;

	bool m_IsBegin = false;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_IsBegin)
			m_FlickCount += Time.deltaTime;
	}
		
	public void OnTouchDown()
	{
		if (!m_IsBegin)
		{
			m_IsBegin = true;
			m_FlickCount = 0.0f;
			m_StartLoc = Input.mousePosition;
		}
	}

	public void OnTouchEnded()
	{
		if (m_FlickCount <= m_FlickInterval) 
		{
			float minLength = 50.0f;

			float slideLen = m_StartLoc.x - Input.mousePosition.x;

			if (Mathf.Abs (slideLen) <= minLength) 
			{
				m_StageSelect.OnStartButton ();
				return;
			}

			if (slideLen > 0) 
			{
				m_StageSelect.OnLeftButton ();
			} else 
			{
				m_StageSelect.OnRightButton ();
			}
		}

		m_IsBegin = false;
	}
}
