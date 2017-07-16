using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSound : MonoBehaviour {
	static TitleSound m_Instance;
	public static TitleSound Instance
	{
		get{
			if (!m_Instance)
				m_Instance = new TitleSound ();

			return m_Instance;
		}
	}

	void Awake()
	{
		if (m_Instance != null && m_Instance != this) {
			Destroy (this.gameObject);
			return;
		} else
			m_Instance = this;

		DontDestroyOnLoad (this.gameObject);
	}

	public bool m_IsDestroyOnLoad = false;

	// Use this for initialization
	void Start () {
		if (!m_IsDestroyOnLoad)
			DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
