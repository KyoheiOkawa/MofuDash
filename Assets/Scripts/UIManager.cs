using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //現在の進行度を示すバー
    [SerializeField]
    Image m_ProgressBar;

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

    public void OnPauseButton()
    {
        SoundManager sound = SoundManager.Instance;
        sound.PlaySE("Button");

        Time.timeScale = 0.0f;
        Instantiate(Resources.Load("Prefabs/PausePanel") as GameObject, GetComponent<Transform>());
    }
}
