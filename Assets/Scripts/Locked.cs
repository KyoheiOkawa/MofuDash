﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Locked : MonoBehaviour
{
    [SerializeField]
    Text unlockHintComment;

    public void DestroyOwn()
    {
        Destroy(this.gameObject);
    }

    public void SetVisible(bool b)
    {
        gameObject.SetActive(b);
    }

    public void SetUnlockInfoText(int coinNum)
    {
        unlockHintComment.text = string.Format("コイン\n{0,0}枚で開錠", coinNum);
    }
}
