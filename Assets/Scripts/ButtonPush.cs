using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボタンを押した感をだすクラス
/// </summary>
public class ButtonPush : MonoBehaviour {
	Transform m_Trans;

	Image m_Image;

	Vector3 m_DefaultScale;

	Color m_DefaultColor;

	[SerializeField]
	float m_PushScale = 1.1f;
	[SerializeField]
	Color m_PushColor = Color.gray;

	// Use this for initialization
	void Start () {
		m_Trans = GetComponent<Transform> ();
		m_Image = GetComponent<Image> ();

		m_DefaultScale = m_Trans.localScale;
		m_DefaultColor = m_Image.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPush()
	{
		m_Trans.localScale = m_DefaultScale * m_PushScale;
		m_Image.color = m_PushColor;
	}

	public void OnUp()
	{
		m_Trans.localScale = m_DefaultScale;
		m_Image.color = m_DefaultColor;
	}
}
