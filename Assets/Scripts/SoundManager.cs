using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static AudioSource audioSource;

    static SoundManager instance;

    static public SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null)
                {
                    GameObject obj = Instantiate(Resources.Load("Prefabs/SoundManager") as GameObject);
                    instance = obj.GetComponent<SoundManager>();
                    audioSource = obj.GetComponent<AudioSource>();
                }
            }

            return instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySE(string seName)
    {
        string path = "SE/" + seName;

        AudioClip clip = Resources.Load(path) as AudioClip;
        audioSource.PlayOneShot(clip);
    }

    public void PlayJingle(string jingleName)
    {
        string path = "Jingle/" + jingleName;

        AudioClip clip = Resources.Load(path) as AudioClip;
        audioSource.PlayOneShot(clip);
    }

    public void StopBGM()
    {
        GameObject sound = GameObject.FindGameObjectWithTag("BGM");

        if (sound)
        {
            sound.GetComponent<AudioSource>().Stop();
        }
    }

    public void PauseBGM()
    {
        GameObject sound = GameObject.FindGameObjectWithTag("BGM");

        if (sound)
        {
            sound.GetComponent<AudioSource>().Pause();
        }
    }

    public void ResumeBGM()
    {
        GameObject sound = GameObject.FindGameObjectWithTag("BGM");

        if (sound)
        {
            sound.GetComponent<AudioSource>().Play();
        }
    }

    public bool IsFinishedAllSound()
    {
        return !audioSource.isPlaying;
    }
}
