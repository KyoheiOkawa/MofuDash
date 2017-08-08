using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Locked : MonoBehaviour {
	[SerializeField]
	Text m_ComTex;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DestroyOwn()
	{
		Destroy(this.gameObject);
	}

	public void SetVisible(bool b)
	{
		gameObject.SetActive (b);
	}

	public void SetUnlockInfoText(int coinNum)
	{
		m_ComTex.text = string.Format ("コイン{0,2}枚で開錠", coinNum);
	}
}
