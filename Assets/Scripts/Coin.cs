using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
	bool m_IsCatched = false;

	[SerializeField]
	MainSceneManager m_SceneManager;

	public bool isCatched
	{
		get{
			return m_IsCatched;
		}
		set{
			m_IsCatched = value;

			if (m_IsCatched)
				GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>("Sprites/catchedcoin");
		}
	}

	// Use this for initialization
	void Start () {
		if(!m_SceneManager)
			m_SceneManager = GameObject.FindObjectOfType<MainSceneManager> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Reset(){
		if (!m_SceneManager)
			m_SceneManager = GameObject.FindObjectOfType<MainSceneManager> ();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			m_IsCatched = true;

			m_SceneManager.UpdateCoinChatcedState ();

			GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		}
	}
}
