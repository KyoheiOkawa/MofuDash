using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    Image m_ProgressBar;//現在の進行度を示すバー

    public float progress
    {
        get
        {
            return m_ProgressBar.fillAmount;
        }
        set
        {
            m_ProgressBar.fillAmount = value;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPauseButton()
	{
		Time.timeScale = 0.0f;
		Instantiate (Resources.Load ("Prefabs/PausePanel") as GameObject, GetComponent<Transform> ());
	}
}
