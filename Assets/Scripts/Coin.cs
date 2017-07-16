using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
	bool m_IsCatched = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			m_IsCatched = true;

			GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		}
	}
}
