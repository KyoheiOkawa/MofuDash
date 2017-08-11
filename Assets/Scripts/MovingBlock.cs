using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingBlock : MonoBehaviour {
	public enum MoveDir
	{
		UP,DOWN
	}

	[SerializeField]
	MoveDir m_MoveDir = MoveDir.UP;

	[SerializeField]
	float m_MoveSpeed = 1.0f;

	Rigidbody2D m_Rig;

	// Use this for initialization
	void Start () {
		if (!m_Rig)
			m_Rig = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Reset(){
		if (!m_Rig)
			m_Rig = GetComponent<Rigidbody2D> ();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag ("Player")) 
		{
			switch (m_MoveDir) 
			{
			case MoveDir.UP:
				m_Rig.velocity = Vector2.up * m_MoveSpeed;
				break;
			case MoveDir.DOWN:
				m_Rig.velocity = Vector2.down * m_MoveSpeed;
				break;
			}
		}
	}
}
