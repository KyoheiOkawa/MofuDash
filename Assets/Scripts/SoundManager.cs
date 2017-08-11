using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	static AudioSource m_Audio;

	static SoundManager m_Instance;

	static public SoundManager Instance
	{
		get
		{
			if (m_Instance == null) 
			{
				m_Instance = (SoundManager)FindObjectOfType (typeof(SoundManager));

				if (m_Instance == null)
				{
					GameObject obj = Instantiate (Resources.Load ("Prefabs/SoundManager")as GameObject);
					m_Instance = obj.GetComponent<SoundManager> ();
					m_Audio = obj.GetComponent<AudioSource> ();
				}
			}

			return m_Instance;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySE(string seName)
	{
		string path = "SE/" + seName;

		AudioClip clip = Resources.Load (path) as AudioClip;
		m_Audio.PlayOneShot (clip);
	}

	public void PlayJingle(string jingleName)
	{
		string path = "Jingle/" + jingleName;

		AudioClip clip = Resources.Load (path) as AudioClip;
		m_Audio.PlayOneShot (clip);
	}

	public void StopBGM()
	{
		GameObject sound = GameObject.FindGameObjectWithTag ("BGM");

		if (sound)
		{
			sound.GetComponent<AudioSource> ().Stop ();
		}
	}

	public void PauseBGM()
	{
		GameObject sound = GameObject.FindGameObjectWithTag ("BGM");

		if (sound)
		{
			sound.GetComponent<AudioSource> ().Pause ();
		}
	}

	public void ResumeBGM()
	{
		GameObject sound = GameObject.FindGameObjectWithTag ("BGM");

		if (sound)
		{
			sound.GetComponent<AudioSource> ().Play ();
		}
	}

	public bool IsFinishedAllSound()
	{
		return !m_Audio.isPlaying;
	}
}
