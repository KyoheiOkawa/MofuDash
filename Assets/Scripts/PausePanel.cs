using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public void OnResumeButton()
    {
        SoundManager sound = SoundManager.Instance;
        sound.PlaySE("Button");

        Time.timeScale = 1.0f;
        Destroy(this.gameObject);
    }

    public void OnTitleButton()
    {
        Time.timeScale = 1.0f;
        string title = GameManager.Instance.StageSelectSceneName;
        var fade = FadeManager.Instance;
        fade.Transition(0.5f, title);
    }
}
