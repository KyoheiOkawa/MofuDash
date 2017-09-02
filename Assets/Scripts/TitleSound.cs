using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSound : MonoBehaviour
{
    static TitleSound instance;
    public static TitleSound Instance
    {
        get
        {
            if (!instance)
                instance = new TitleSound();

            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
            instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void DestroyOwn()
    {
        Destroy(this.gameObject);
    }
}
